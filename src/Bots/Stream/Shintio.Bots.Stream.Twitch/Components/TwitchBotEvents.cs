using Shintio.Bots.Stream.Core.Common.EventArgs;
using Shintio.Bots.Stream.Core.Models;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;

namespace Shintio.Bots.Stream.Twitch.Components;

public partial class TwitchBot
{
	private void SetupEvents()
	{
		_client.OnMessageReceived += OnMessageReceived;
		_client.OnChatCommandReceived += OnChatCommandReceived;
	}
	
	private void OnMessageReceived(object? sender, OnMessageReceivedArgs e)
	{
		MessageReceived?.Invoke(
			this,
			new MessageReceivedArgs(ConvertMessage(e.ChatMessage))
		);
	}
	
	private void OnChatCommandReceived(object? sender, OnChatCommandReceivedArgs e)
	{
		var command = new StreamCommand(
			e.Command.CommandText, 
			e.Command.ArgumentsAsString,
			e.Command.ArgumentsAsList,
			ConvertMessage(e.Command.ChatMessage)
		);

		CommandReceived?.Invoke(
			this, 
			new CommandReceivedArgs(command)
		);
	}

	private StreamMessage ConvertMessage(ChatMessage message)
	{
		var senderUser = new StreamUser(message.UserId, message.Username, message.DisplayName);
		var channel = new StreamChannel(message.Channel);
		return new StreamMessage(message.Id, message.Message, senderUser, channel);
	}
}