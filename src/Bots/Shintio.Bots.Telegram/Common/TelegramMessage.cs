using Shintio.Bots.Core.Interfaces;

namespace Shintio.Bots.Telegram.Common;

public class TelegramMessage : IMessage
{
	public TelegramMessage(string text)
	{
		Text = text;
	}

	public string Text { get; }
}