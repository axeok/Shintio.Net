using Shintio.Bots.Stream.Core.Models;

namespace Shintio.Bots.Stream.Core.Common.EventArgs;

public class MessageReceivedArgs : System.EventArgs
{
	public MessageReceivedArgs(StreamMessage message)
	{
		Message = message;
	}

	public StreamMessage Message { get; }
}