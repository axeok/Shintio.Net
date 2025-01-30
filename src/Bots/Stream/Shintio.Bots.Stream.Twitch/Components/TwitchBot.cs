using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shintio.Bots.Stream.Core.Common;
using Shintio.Bots.Stream.Core.Common.EventArgs;
using Shintio.Bots.Stream.Core.Interfaces;
using Shintio.Bots.Stream.Core.Models;
using Shintio.Bots.Stream.Twitch.Configuration;
using TwitchLib.Api;
using TwitchLib.Api.Core;
using TwitchLib.Api.Core.Enums;
using TwitchLib.Api.Helix.Models.Channels.ModifyChannelInformation;
using TwitchLib.Client;
using TwitchLib.Client.Extensions;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Models;
using TwitchLib.PubSub;

namespace Shintio.Bots.Stream.Twitch.Components;

public partial class TwitchBot : IStreamBot
{
	public event StreamBotEventHandler<MessageReceivedArgs>? MessageReceived;
	public event StreamBotEventHandler<CommandReceivedArgs>? CommandReceived;

	private readonly string _channelId;
	private readonly string _channelName;
	private readonly TwitchChannelCredentials _channelCredentials;
	private readonly TwitchSecrets _secrets;

	private readonly TwitchClient _client;
	private readonly TwitchPubSub _pubSub;
	private readonly TwitchAPI _api;

	public TwitchBot(
		TwitchChannelCredentials channelCredentials,
		IOptions<TwitchSecrets> secrets,
		ILoggerFactory loggerFactory
	)
	{
		_channelId = channelCredentials.ChannelId;
		_channelName = channelCredentials.ChannelName;
		_channelCredentials = channelCredentials;
		_secrets = secrets.Value;

		var clientOptions = new ClientOptions
		{
			MessagesAllowedInPeriod = 750,
			ThrottlingPeriod = TimeSpan.FromSeconds(30),
		};
		var webSocketClient = new WebSocketClient(clientOptions);
		_client = new TwitchClient(webSocketClient, logger: new Logger<TwitchClient>(loggerFactory));
		SetupEvents();

		_pubSub = new TwitchPubSub(new Logger<TwitchPubSub>(loggerFactory));
		_pubSub.ListenToChannelPoints(_channelId);
		_pubSub.OnPubSubServiceConnected += (_, _) => { _pubSub.SendTopics(_channelCredentials.AccessToken); };

		var apiSettings = new ApiSettings
		{
			AccessToken = _secrets.AccessToken,
			ClientId = _secrets.ClientId,
			Secret = "Nortages",
			Scopes = new List<AuthScopes>(),
		};
		_api = new TwitchAPI(loggerFactory, settings: apiSettings);
	}

	~TwitchBot()
	{
		_client.Disconnect();
		_pubSub.Disconnect();
	}

	public Task Initialize(CancellationToken cancellationToken)
	{
		var credentials = new ConnectionCredentials(_secrets.Username, _secrets.AccessToken);
		_client.Initialize(credentials, _channelName);
		_client.Connect();

		_pubSub.Connect();

		return Task.CompletedTask;
	}

	public Task SendMessage(string message, string? replyToId = null)
	{
		if (replyToId != null)
		{
			_client.SendReply(_channelName, replyToId, message);
		}
		else
		{
			_client.SendMessage(_channelName, message);
		}

		return Task.CompletedTask;
	}

	public Task DeleteMessage(string messageId)
	{
		_client.DeleteMessage(_channelName, messageId);
		return Task.CompletedTask;
	}

	public Task EditStreamTitle(string title)
	{
		return _api.Helix.Channels.ModifyChannelInformationAsync(_channelId, new ModifyChannelInformationRequest
		{
			Title = title,
		}, _channelCredentials.AccessToken);
	}

	public async Task<IReadOnlyCollection<StreamChatter>> GetChatters()
	{
		var response = await _api.Helix.Chat.GetChattersAsync(_channelId, _secrets.UserId);
		return response.Data.Select(x => new StreamChatter(x.UserLogin)).ToArray();
	}
}