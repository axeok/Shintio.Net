using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Shintio.Database.Extensions;
using Shintio.Json.Interfaces;
using Shintio.Json.System.Common;

namespace Shintio.Hosting;

public abstract class ShintioApp<THost, TJson>
	where THost : IHost
	where TJson : IJson
{
	public readonly THost Host;

	protected readonly ILogger Logger;

	public ShintioApp(Action<HostBuilderContext, IServiceCollection> configureServices)
	{
		var host = BuildHost(configureServices);
		if (host is not THost castedHost)
		{
			throw new ApplicationException($"Host is not {typeof(THost).FullName}");
		}

		Host = castedHost;

		Logger = (ILogger)Host.Services.GetRequiredService(typeof(ILogger<>).MakeGenericType(GetType()));

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

	protected virtual IHost BuildHost(Action<HostBuilderContext, IServiceCollection> configureServices)
	{
		return Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder()
			.ConfigureHostConfiguration(ConfigureHostConfiguration)
			.ConfigureAppConfiguration(ConfigureAppConfiguration)
			.ConfigureServices(ConfigureBaseServices)
			.ConfigureServices(ConfigureServices)
			.ConfigureServices(configureServices)
			.Build();
	}

	protected virtual void ConfigureHostConfiguration(IConfigurationBuilder builder)
	{
	}

	protected virtual void ConfigureAppConfiguration(HostBuilderContext context, IConfigurationBuilder builder)
	{
	}

	protected virtual void ConfigureBaseServices(HostBuilderContext context, IServiceCollection services)
	{
		services.AddShintioNet<TJson>();
	}
}

public abstract class ShintioApp : ShintioApp<IHost, SystemJson>
{
	protected ShintioApp(Action<HostBuilderContext, IServiceCollection> configureServices) : base(configureServices)
	{
	}
}