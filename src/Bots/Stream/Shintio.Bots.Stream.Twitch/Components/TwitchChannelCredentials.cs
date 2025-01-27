namespace Shintio.Bots.Stream.Twitch.Components;

public record TwitchChannelCredentials(
	string ChannelId,
	string ChannelName,
	string AccessToken,
	string RefreshToken,
	string ClientId
);