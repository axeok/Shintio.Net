using Microsoft.Extensions.DependencyInjection;
using Shintio.MachineTranslation.Abstractions;
using Shintio.MachineTranslation.GoogleApi;

namespace Shintio.MachineTranslation.Extensions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddTranslation(this IServiceCollection services)
	{
		return services.AddSingleton<ITranslator, GoogleTranslator>();
	}
}