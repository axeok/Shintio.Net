using System;
using System.Threading.Tasks;
using Shintio.Communication.Core.Interfaces;

namespace Shintio.Communication.Core.Common
{
	public abstract class CommunicationClient : CommunicationUnit, ICommunicationClient
	{
		public event Action? Connected;

		public CommunicationClient(ICommunicationStream stream, MessageSerializer serializer) : base(stream, serializer)
		{
		}

		public async Task Connect()
		{
			await ConnectInternal();

			Connected?.Invoke();
		}

		protected abstract Task ConnectInternal();
	}
}