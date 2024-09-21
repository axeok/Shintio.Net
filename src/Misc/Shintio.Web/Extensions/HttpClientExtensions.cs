using System.Net;

namespace Shintio.Web.Extensions;

public static class HttpClientExtensions
{
	private const string IpCheckUrl = "https://icanhazip.com/";

	public static async Task<IPAddress?> GetSelfPublicIpAddress(this HttpClient client)
	{
		var response = await client.GetAsync(IpCheckUrl);
		if (!response.IsSuccessStatusCode)
		{
			return null;
		}
		
		return IPAddress.TryParse((await response.Content.ReadAsStringAsync()).Trim(), out var address) ? address : null;
	}
}