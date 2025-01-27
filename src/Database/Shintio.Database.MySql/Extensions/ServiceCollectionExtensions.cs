using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Shintio.Database.MySql.Extensions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddDatabase<TDbContext>(
		this IServiceCollection services,
		string? connectionStringKey = null
	)
		where TDbContext : DbContext
	{
		return services
			.AddTransient<TDbContext>()
			.AddPooledDbContextFactory<TDbContext>((serviceProvider, builder) =>
			{
				builder.AddDatabase<TDbContext>(
					serviceProvider.GetRequiredService<IConfiguration>(),
					connectionStringKey
				);
			});
	}

	public static DbContextOptionsBuilder AddDatabase<TDbContext>(
		this DbContextOptionsBuilder builder,
		IConfiguration configuration,
		string? connectionStringKey = null
	) 
		where TDbContext : DbContext
	{
		connectionStringKey ??= typeof(TDbContext).Name;
		var connectionString = configuration.GetConnectionString(connectionStringKey)!;
		return builder.AddDatabase(connectionString);
	}

	public static DbContextOptionsBuilder AddDatabase(
		this DbContextOptionsBuilder builder, 
		string connectionString
	)
	{
		return builder
			.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
			.UseLazyLoadingProxies()
			.ConfigureWarnings(b => b.Log(
				(RelationalEventId.CommandExecuted, LogLevel.Debug),
				(RelationalEventId.ConnectionOpened, LogLevel.Debug),
				(RelationalEventId.ConnectionClosed, LogLevel.Debug)));
	}
}