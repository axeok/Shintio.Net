﻿using Shintio.Bots.Core.Components.Interfaces;

namespace Shintio.Bots.Telegram.Common;

public class TelegramUser : IUser
{
	public TelegramUser(long id, string name)
	{
		Id = id;
		Name = name;
	}

	public long Id { get; }
	public string Name { get; }
}