using Microsoft.Extensions.Logging;
using Shintio.Web.Interfaces;
using Shintio.Web.Utils;

namespace Shintio.Web.Services;

public class AutoProxyService
{
	private readonly ILogger<AutoProxyService> _logger;
	private readonly IProxyProvider _proxyProvider;
	private readonly IHttpClientFactory _factory;
	private readonly AutoProxyHttpClientHandler _handler;

	private HttpClient? _client;

	public AutoProxyService(
		ILogger<AutoProxyService> logger,
		IHttpClientFactory factory,
		IProxyProvider proxyProvider,
		AutoProxyHttpClientHandler handler
	)
	{
		_logger = logger;
		_factory = factory;
		_proxyProvider = proxyProvider;
		_handler = handler;
	}

	public HttpClient Client => UpdateClient();

	public async Task NewProxy(bool removeCurrent)
	{
		await _proxyProvider.RescanProxiesAsync(removeCurrent);
		_handler.UpdateProxy();
		UpdateClient();
	}

	public async Task<HttpResponseMessage> Wrap(Func<HttpClient, Task<HttpResponseMessage>> sendRequest)
	{
		try
		{
			return await sendRequest(Client);
		}
		catch (HttpRequestException exception)
		{
			_logger.LogWarning(exception, "Exception in wrap");

			if (exception.HttpRequestError != HttpRequestError.ConnectionError)
			{
				throw;
			}

			await NewProxy(true);
			UpdateClient();

			return await Wrap(sendRequest);
		}
	}

	private HttpClient UpdateClient()
	{
		return _client = _factory.CreateClient(nameof(AutoProxyService));
	}
}