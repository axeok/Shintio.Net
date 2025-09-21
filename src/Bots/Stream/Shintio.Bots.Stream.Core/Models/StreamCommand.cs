namespace Shintio.Bots.Stream.Core.Models;

public class StreamCommand
{
	public StreamCommand(string name, string argument, IReadOnlyList<string> arguments, StreamMessage message)
	{
		Name = name;
		Argument = argument;
		Arguments = arguments;
		Message = message;
	}

	public string Name { get; }
	public string Argument { get; }
	public IReadOnlyList<string> Arguments { get; }
	public StreamMessage Message { get; }
}