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
		connectionStringKey ??= typeof(TDbContext).Name;

		return services
			.AddPooledDbContextFactory<TDbContext>((serviceProvider, builder) =>
			{
				var connection = serviceProvider.GetRequiredService<IConfiguration>()
					.GetConnectionString(connectionStringKey);

				builder.UseMySql(connection, ServerVersion.AutoDetect(connection))
					.UseLazyLoadingProxies()
					.ConfigureWarnings(b => b.Log(
						(RelationalEventId.CommandExecuted, LogLevel.Debug),
						(RelationalEventId.ConnectionOpened, LogLevel.Debug),
						(RelationalEventId.ConnectionClosed, LogLevel.Debug)));
			});
	}
}