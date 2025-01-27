namespace Shintio.Bots.Stream.Core.Models;

public class StreamUser
{
	public StreamUser(string id, string username, string displayName)
	{
		Id = id;
		Username = username;
		DisplayName = displayName;
	}

	public string Id { get; }
	public string Username { get; }
	public string DisplayName { get; }
}