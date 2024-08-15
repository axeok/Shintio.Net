using Microsoft.Extensions.DependencyInjection;
using Shintio.Web.Interfaces;
using Shintio.Web.Services;
using Shintio.Web.Utils;
using Shintio.Web.Utils.ProxyProviders;

namespace Shintio.Web.Extensions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddWebUtils(this IServiceCollection services)
	{
		services.AddHttpClient<FreeProxyProvider>(nameof(FreeProxyProvider));
		services.AddHttpClient(nameof(AutoProxyService))
			.ConfigurePrimaryHttpMessageHandler<AutoProxyHttpClientHandler>();

		return services
			.AddMemoryCache()
			.AddTransient<AutoProxyService>()
			.AddSingleton<IProxyProvider, FreeProxyProvider>()
			.AddSingleton<AutoProxyHttpClientHandler>();
	}
}