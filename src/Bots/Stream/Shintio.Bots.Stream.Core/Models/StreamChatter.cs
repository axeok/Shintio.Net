namespace Shintio.Bots.Stream.Core.Models;

public class StreamChatter
{
	public StreamChatter(string username)
	{
		Username = username;
	}

	public string Username { get; }
}