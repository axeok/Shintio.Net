using System.Net;
using Shintio.Web.Interfaces;

namespace Shintio.Web.Utils;

public class AutoProxyHttpClientHandler : HttpClientHandler
{
	private readonly IProxyProvider _proxyProvider;
	private readonly WebProxy _proxy;

	public AutoProxyHttpClientHandler(IProxyProvider proxyProvider)
	{
		_proxyProvider = proxyProvider;

		_proxy = new WebProxy();
		Proxy = _proxy;

		UpdateProxy();
	}

	public void UpdateProxy()
	{
		_proxy.Address = _proxyProvider.GetProxyAsync().GetAwaiter().GetResult()?.Address;
	}
}