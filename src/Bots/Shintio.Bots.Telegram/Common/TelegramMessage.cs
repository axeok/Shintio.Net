using Shintio.Bots.Core.Components.Interfaces;

namespace Shintio.Bots.Telegram.Common;

public class TelegramMessage : IMessage<TelegramUser>
{
	public TelegramMessage(int id, string text, TelegramUser sender)
	{
		Id = id;
		Text = text;
		Sender = sender;
	}

	public int Id { get; }
	public string Text { get; }
	public TelegramUser Sender { get; }
}