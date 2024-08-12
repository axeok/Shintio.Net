using Shintio.Bots.Telegram.Common;
using Shintio.Bots.Telegram.Services;

namespace Shintio.Net;

public class TestBot : TelegramBotService
{
    public TestBot(TelegramBot bot) : base(bot)
    {
    }

    protected override async Task MessageHandler(TelegramUser user, TelegramMessage message)
    {
        Console.WriteLine(message.Text);

        await Bot.SendMessageAsync(user, new TelegramMessage("ЫЫЫ"));
    }
}