namespace Shintio.Bots.Core.Interfaces;

public interface IBot<TUser, TMessage>
    where TUser : IUser
    where TMessage : IMessage

{
    public event Func<TUser, TMessage, Task> MessageReceived;

    public Task SendMessageAsync(TUser user, TMessage message);
}