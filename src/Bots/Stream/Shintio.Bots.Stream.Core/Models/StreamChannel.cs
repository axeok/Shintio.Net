namespace Shintio.Bots.Stream.Core.Models;

public class StreamChannel
{
	public StreamChannel(string name)
	{
		Name = name;
	}
	
	public string Name { get; }
}