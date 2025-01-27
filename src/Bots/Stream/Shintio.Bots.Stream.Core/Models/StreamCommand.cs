namespace Shintio.Bots.Stream.Core.Models;

public class StreamCommand
{
	public StreamCommand(string name, string argument, IReadOnlyCollection<string> arguments, StreamMessage message)
	{
		Name = name;
		Argument = argument;
		Arguments = arguments;
		Message = message;
	}

	public string Name { get; }
	public string Argument { get; }
	public IReadOnlyCollection<string> Arguments { get; }
	public StreamMessage Message { get; }
}