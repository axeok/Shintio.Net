using Microsoft.Extensions.DependencyInjection;
using Shintio.Vision.Abstractions;
using Shintio.Vision.Tesseract;

namespace Shintio.Vision.Extensions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddVision(this IServiceCollection services)
	{
		return services
			.AddSingleton<IOcr, TesseractOcr>()
			.AddHostedService<TesseractService>();
	}
}