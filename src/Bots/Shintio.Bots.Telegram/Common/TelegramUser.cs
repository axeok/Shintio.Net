using Shintio.Bots.Core.Interfaces;

namespace Shintio.Bots.Telegram.Common;

public class TelegramUser : IUser
{
    public TelegramUser(long id)
    {
        Id = id;
    }

    public long Id { get; }
}