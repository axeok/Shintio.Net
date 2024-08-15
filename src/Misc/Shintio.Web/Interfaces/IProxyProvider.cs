using System.Net;

namespace Shintio.Web.Interfaces;

public interface IProxyProvider
{
	public Task<WebProxy?> GetProxyAsync();

	public Task RescanProxiesAsync(bool ignoreCurrent);
}