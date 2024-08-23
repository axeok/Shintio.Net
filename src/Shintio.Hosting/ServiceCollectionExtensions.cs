using Microsoft.Extensions.DependencyInjection;
using Shintio.Compression.Extensions;
using Shintio.Json.Interfaces;
using Shintio.Json.System.Common;
using Shintio.Json.Utils;
using Shintio.MachineTranslation.Extensions;
using Shintio.Web.Extensions;

namespace Shintio.Hosting;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddShintioNet(this IServiceCollection services)
	{
		var json = new SystemJson();
		
		JsonConverter.Instance = json;
		
		return services
			.AddSingleton<IJson, SystemJson>(_ => json)
			.AddCompression()
			.AddTranslation()
			.AddWebUtils();
	}
}