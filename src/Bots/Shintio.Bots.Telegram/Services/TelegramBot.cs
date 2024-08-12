using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shintio.Bots.Core.Interfaces;
using Shintio.Bots.Telegram.Common;
using Shintio.Bots.Telegram.Configuration;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Shintio.Bots.Telegram.Services;

public class TelegramBot : IBot<TelegramUser, TelegramMessage>
{
    public event Func<TelegramUser, TelegramMessage, Task>? MessageReceived;

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

    public async Task SendMessageAsync(TelegramUser user, TelegramMessage message)
    {
        await _client.SendTextMessageAsync(user.Id, message.Text);
    }

    private async Task UpdateHandler(ITelegramBotClient _, Update update, CancellationToken cancellationToken)
    {
        if (update.Type != UpdateType.Message)
        {
            return;
        }

        await MessageReceived?.Invoke(
            new TelegramUser(update.Message.Chat.Id),
            new TelegramMessage(update.Message.Text)
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