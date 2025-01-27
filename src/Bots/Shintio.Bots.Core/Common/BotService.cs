using Microsoft.Extensions.Hosting;
using Shintio.Bots.Core.Components.Interfaces;

namespace Shintio.Bots.Core.Common;

public abstract class BotService<TBot, TRoom, TUser, TMessage> : BackgroundService
	where TBot : IBot<TRoom, TUser, TMessage>
	where TRoom : IRoom
	where TUser : IUser
	where TMessage : IMessage<TUser>
{
	protected readonly TBot Bot;

	public BotService(TBot bot)
	{
		Bot = bot;

		Bot.MessageReceived += BotOnMessageReceived;
	}

	protected abstract Task MessageHandler(TRoom room, TMessage message);

	protected override Task ExecuteAsync(CancellationToken stoppingToken)
	{
		return Task.CompletedTask;
	}

	private async Task BotOnMessageReceived(TRoom room, TMessage message)
	{
		await MessageHandler(room, message);
	}
}