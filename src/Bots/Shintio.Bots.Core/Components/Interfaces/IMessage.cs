namespace Shintio.Bots.Core.Components.Interfaces;

public interface IMessage
{
	string Text { get; }
	IUser Sender { get; }
}

public interface IMessage<out TUser> : IMessage where TUser : IUser
{
	new TUser Sender { get; }
	
	IUser IMessage.Sender => Sender;
}