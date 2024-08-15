using Shintio.Web.Interfaces;
using Shintio.Web.Utils;

namespace Shintio.Web.Services;

public class AutoProxyService
{
	private readonly IProxyProvider _proxyProvider;
	private readonly IHttpClientFactory _factory;
	private readonly AutoProxyHttpClientHandler _handler;

	public AutoProxyService(IHttpClientFactory factory, IProxyProvider proxyProvider, AutoProxyHttpClientHandler handler)
	{
		_factory = factory;
		_proxyProvider = proxyProvider;
		_handler = handler;
	}

	public HttpClient GetClient()
	{
		return _factory.CreateClient(nameof(AutoProxyService));
	}

	public async Task NewProxy(bool removeCurrent)
	{
		await _proxyProvider.RescanProxiesAsync(removeCurrent);
		_handler.UpdateProxy();
	}
}