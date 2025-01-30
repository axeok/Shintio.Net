using Shintio.Bots.Stream.Core.Common;
using Shintio.Bots.Stream.Core.Common.EventArgs;
using Shintio.Bots.Stream.Core.Models;

namespace Shintio.Bots.Stream.Core.Interfaces;

public interface IStreamBot
{
	public event StreamBotEventHandler<MessageReceivedArgs>? MessageReceived;
	public event StreamBotEventHandler<CommandReceivedArgs>? CommandReceived;

	Task Initialize(CancellationToken cancellationToken);

	Task SendMessage(string message, string? replyToId = null);
	Task DeleteMessage(string messageId);
	Task EditStreamTitle(string title);
	Task<IReadOnlyCollection<StreamChatter>> GetChatters();
}