using Microsoft.Extensions.Hosting;
using Shintio.Bots.Core.Interfaces;

namespace Shintio.Bots.Core.Common;

public abstract class BotService<TBot, TUser, TMessage> : BackgroundService
    where TBot : IBot<TUser, TMessage>
    where TUser : IUser
    where TMessage : IMessage
{
    protected readonly TBot Bot;

    public BotService(TBot bot)
    {
        Bot = bot;

        Bot.MessageReceived += BotOnMessageReceived;
    }

    protected abstract Task MessageHandler(TUser user, TMessage message);

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return Task.CompletedTask;
    }

    private async Task BotOnMessageReceived(TUser user, TMessage message)
    {
        await MessageHandler(user, message);
    }
}