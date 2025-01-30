// ReSharper disable PropertyCanBeMadeInitOnly.Global

namespace Shintio.Bots.Stream.Twitch.Configuration;

public sealed class TwitchSecrets
{
	public string UserId { get; set; } = "";
	public string Username { get; set; } = "";
	public string ClientId { get; set; } = "";
	public string AccessToken { get; set; } = "";
	public string RefreshToken { get; set; } = "";
}