using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Shintio.Database.Extensions;

public static class HostExtensions
{
	public static async Task ApplyMigrationsAsync<TDbContext>(this IHost host) where TDbContext : DbContext
	{
		await using var context = await host.Services
			.GetRequiredService<IDbContextFactory<TDbContext>>()
			.CreateDbContextAsync();

		await context.Database.MigrateAsync();
	}
}