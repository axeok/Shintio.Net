using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Shintio.Hosting;

public abstract class ShintioApp
{
	protected readonly IHost Host;

	private readonly ILogger<ShintioApp> _logger;

	public ShintioApp(Action<HostBuilderContext, IServiceCollection> configureServices)
	{
		Host = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder()
			.ConfigureServices(ConfigureServices)
			.ConfigureServices(configureServices)
			.Build();

		_logger = Host.Services.GetRequiredService<ILogger<ShintioApp>>();

		_logger.LogInformation("Created");
	}

	public virtual async Task PrepareAsync()
	{
		_logger.LogInformation("Preparing...");

		await PrepareInternalAsync();
	}

	public async Task RunAsync()
	{
		_logger.LogInformation("Running...");

		await BeforeRun();

		await Host.RunAsync();
	}

	protected abstract void ConfigureServices(HostBuilderContext context, IServiceCollection services);
	protected abstract Task PrepareInternalAsync();
	protected abstract Task BeforeRun();
}