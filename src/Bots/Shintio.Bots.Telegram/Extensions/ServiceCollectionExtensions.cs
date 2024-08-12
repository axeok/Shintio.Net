using Microsoft.Extensions.DependencyInjection;
using Shintio.Bots.Telegram.Services;

namespace Shintio.Bots.Telegram.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTelegramBot<TService>(this IServiceCollection services)
        where TService : TelegramBotService
    {
        return services
            .AddScoped<TelegramBot>()
            .AddHostedService<TService>();
    }
}