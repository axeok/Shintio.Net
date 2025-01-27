using Shintio.Bots.Core.Components.Interfaces;

namespace Shintio.Bots.Telegram.Common;

public class TelegramRoom : IRoom
{
	public TelegramRoom(long id, string name)
	{
		Id = id;
		Name = name;
	}

	public long Id { get; }
	public string Name { get; }
}