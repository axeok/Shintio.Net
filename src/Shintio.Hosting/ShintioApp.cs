using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Shintio.Database.Extensions;

namespace Shintio.Hosting;

public abstract class ShintioApp
{
	public readonly IHost Host;

	protected readonly ILogger<ShintioApp> Logger;

	public ShintioApp(Action<HostBuilderContext, IServiceCollection> configureServices)
	{
		Host = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder()
			.ConfigureServices(services => services.AddShintioNet())
			.ConfigureServices(ConfigureServices)
			.ConfigureServices(configureServices)
			.Build();

		Logger = Host.Services.GetRequiredService<ILogger<ShintioApp>>();

		Logger.LogInformation("Created");
	}

	public virtual async Task PrepareAsync()
	{
		Logger.LogInformation("Preparing...");

		await PrepareInternalAsync();
	}

	public async Task MigrateAsync<TDbContext>() where TDbContext : DbContext
	{
		Logger.LogInformation("Applying migrations...");

		await Host.ApplyMigrationsAsync<TDbContext>();
	}

	public async Task RunAsync()
	{
		Logger.LogInformation("Running...");

		await BeforeRun();

		await Host.RunAsync();
	}

	protected abstract void ConfigureServices(HostBuilderContext context, IServiceCollection services);
	protected abstract Task PrepareInternalAsync();
	protected abstract Task BeforeRun();
}