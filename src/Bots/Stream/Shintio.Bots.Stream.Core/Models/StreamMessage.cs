namespace Shintio.Bots.Stream.Core.Models;

public class StreamMessage
{
	public StreamMessage(string id, string text, StreamUser sender, StreamChannel channel)
	{
		Id = id;
		Text = text;
		Sender = sender;
		Channel = channel;
	}

	public string Id { get; }
	public string Text { get; }
	public StreamUser Sender { get; }
	public StreamChannel Channel { get; }
}