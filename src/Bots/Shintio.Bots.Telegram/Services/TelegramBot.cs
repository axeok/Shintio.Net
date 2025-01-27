using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shintio.Bots.Core.Components.Interfaces;
using Shintio.Bots.Telegram.Common;
using Shintio.Bots.Telegram.Configuration;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Shintio.Bots.Telegram.Services;

public class TelegramBot : IBot<TelegramRoom, TelegramUser, TelegramMessage>
{
	public event Func<TelegramRoom, TelegramMessage, Task>? MessageReceived;

	private readonly TelegramSecrets _secrets;
	private readonly TelegramBotClient _client;

	private readonly ILogger<TelegramBot> _logger;

	public TelegramBot(IOptions<TelegramSecrets> secrets, ILogger<TelegramBot> logger)
	{
		_logger = logger;

		_secrets = secrets.Value;

		_client = new TelegramBotClient(_secrets.AccessToken);
		_client.StartReceiving(
			updateHandler: UpdateHandler,
			pollingErrorHandler: HandlePollingErrorAsync
		);
	}

	public async Task SendMessageAsync(TelegramRoom room, string text, TelegramMessage? replyTo = null)
	{
		await _client.SendTextMessageAsync(room.Id, text, replyToMessageId: replyTo?.Id);
	}
	
	public async Task SendMessageAsync(IRoom room, string text, IMessage? replyTo)
	{
		await SendMessageAsync((TelegramRoom)room, text, replyTo as TelegramMessage);
	}

	private async Task UpdateHandler(ITelegramBotClient _, Update update, CancellationToken cancellationToken)
	{
		if (update.Type != UpdateType.Message)
		{
			return;
		}

		var senderUser = new TelegramUser(update.Message.From.Id, update.Message.From.Username);
		await MessageReceived?.Invoke(
			new TelegramRoom(update.Message.Chat.Id, update.Message.Chat.Username),
			new TelegramMessage(update.Message.MessageId, update.Message.Text, senderUser)
		);
	}

	private Task HandlePollingErrorAsync(
		ITelegramBotClient botClient,
		Exception exception,
		CancellationToken cancellationToken
	)
	{
		var errorMessage = exception switch
		{
			ApiRequestException apiRequestException
				=> $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
			_ => exception.ToString()
		};

		_logger.LogError(errorMessage);

		return Task.CompletedTask;
	}
}