using Shintio.Bots.Core.Common;
using Shintio.Bots.Telegram.Common;

namespace Shintio.Bots.Telegram.Services;

public abstract class TelegramBotService : BotService<TelegramBot, TelegramUser, TelegramMessage>
{
	public TelegramBotService(TelegramBot bot) : base(bot)
	{
	}
}