using Shintio.Bots.Stream.Core.Models;

namespace Shintio.Bots.Stream.Core.Common.EventArgs;

public class CommandReceivedArgs : System.EventArgs
{
	public CommandReceivedArgs(StreamCommand command)
	{
		Command = command;
	}

	public StreamCommand Command { get; }
}