namespace Shintio.Bots.Core.Components.Interfaces;

public interface IBot
{
	Task SendMessageAsync(IRoom room, string text, IMessage? replyTo);
}

public interface IBot<TRoom, TUser, TMessage> : IBot
	where TRoom : IRoom
	where TUser : IUser
	where TMessage : IMessage<TUser>
{
	event Func<TRoom, TMessage, Task> MessageReceived;

	Task SendMessageAsync(TRoom room, string text, TMessage? replyTo);
}