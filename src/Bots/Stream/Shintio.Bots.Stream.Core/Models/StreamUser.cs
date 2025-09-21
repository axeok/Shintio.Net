using Shintio.Bots.Stream.Core.Common;

namespace Shintio.Bots.Stream.Core.Models;

public class StreamUser
{
	public StreamUser(string id, string username, string displayName, ChatterType chatterType)
	{
		Id = id;
		Username = username;
		DisplayName = displayName;
		ChatterType = chatterType;
	}

	public string Id { get; }
	public string Username { get; }
	public string DisplayName { get; }
	public ChatterType ChatterType { get; set; }
}