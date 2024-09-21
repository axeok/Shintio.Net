using System.Net;
using System.Net.Http.Json;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Shintio.Web.Extensions;
using Shintio.Web.Interfaces;

namespace Shintio.Web.Utils.ProxyProviders;

public class FreeProxyProvider : IProxyProvider
{
	private const string CacheKey = $"{nameof(FreeProxyProvider)}.ProxiesList";

	private static readonly TimeSpan ValidateTimeout = TimeSpan.FromSeconds(2);

	private const string ProxyListUrl =
		"https://api.proxyscrape.com/v3/free-proxy-list/get?request=displayproxies&proxy_format=ipport&format=json";

	private readonly ILogger<FreeProxyProvider> _logger;
	private readonly IMemoryCache _cache;
	private readonly HttpClient _httpClient;

	private readonly HashSet<Uri> _blockedProxies = new();

	private readonly object _lock = 0;

	public FreeProxyProvider(ILogger<FreeProxyProvider> logger, IMemoryCache cache, HttpClient httpClient)
	{
		_logger = logger;
		_cache = cache;
		_httpClient = httpClient;
	}

	public async Task<WebProxy?> GetProxyAsync()
	{
		return (await _cache.GetOrCreateAsync(CacheKey, ScanProxies))?.FirstOrDefault();
	}

	public async Task RescanProxiesAsync(bool ignoreCurrent)
	{
		if (ignoreCurrent)
		{
			var current = await GetProxyAsync();
			if (current?.Address != null)
			{
				_blockedProxies.Add(current.Address);
			}
		}

		_cache.Set(CacheKey, await ScanProxies(null));
	}

	private async Task<WebProxy[]> ScanProxies(ICacheEntry? _)
	{
		_logger.LogInformation("Scanning proxies...");

		var response = await _httpClient.GetAsync(ProxyListUrl);
		if (!response.IsSuccessStatusCode)
		{
			return [];
		}

		var proxies = new Dictionary<Uri, decimal>();

		var json = (await response.Content.ReadFromJsonAsync<JsonNode>())!;
		foreach (var proxy in json["proxies"]!.AsArray())
		{
			if (proxy == null)
			{
				continue;
			}

			if (!proxy["alive"]!.AsValue().GetValue<bool>())
			{
				continue;
			}

			var protocol = proxy["protocol"]!.AsValue().GetValue<string>();
			var ip = proxy["ip"]!.AsValue().GetValue<string>();
			var port = proxy["port"]!.AsValue().GetValue<int>();

			var uri = new Uri($"{protocol}://{ip}:{port}");
			if (_blockedProxies.Contains(uri))
			{
				continue;
			}

			proxies[uri] = proxy["average_timeout"]!.AsValue().GetValue<decimal>();
		}

		var result = proxies
			.OrderBy(x => x.Value)
			.Select(p => new WebProxy
			{
				Address = p.Key,
			})
			.ToList();

		return (await ValidateProxies(result)).ToArray();
	}

	private async Task<IEnumerable<WebProxy>> ValidateProxies(IEnumerable<WebProxy> proxies)
	{
		_logger.LogInformation("Validating proxies...");

		var result = proxies.ToList();

		var cts = new CancellationTokenSource();
		try
		{
			await Parallel.ForEachAsync(result.ToArray(), cts.Token, async (proxy, _) =>
			{
				try
				{
					var client = new HttpClient(new HttpClientHandler
					{
						Proxy = proxy,
					});
					client.Timeout = ValidateTimeout;

					var ip = await client.GetSelfPublicIpAddress();
					if (ip == null)
					{
						RemoveProxy(result, proxy);
					}
					else
					{
						_logger.LogDebug("{proxy} is valid", proxy.Address);
						await cts.CancelAsync();
					}
				}
				catch
				{
					RemoveProxy(result, proxy);
				}
			});
		}
		catch
		{
			// ignored
		}

		if (result.Count == 0)
		{
			_logger.LogWarning("No valid proxies!");
		}
		else
		{
			_logger.LogInformation("First valid proxy: {proxy}", result.First().Address);
		}

		return result;
	}

	private void RemoveProxy(List<WebProxy> collection, WebProxy proxy)
	{
		lock (_lock)
		{
			if (!collection.Contains(proxy))
			{
				return;
			}

			_logger.LogDebug("{proxy} not valid, removing", proxy.Address);
			collection.Remove(proxy);

			if (proxy.Address != null)
			{
				_blockedProxies.Add(proxy.Address);
			}
		}
	}
}