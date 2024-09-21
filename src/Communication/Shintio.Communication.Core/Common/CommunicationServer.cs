using System;
using System.Threading.Tasks;
using Shintio.Communication.Core.Interfaces;

namespace Shintio.Communication.Core.Common
{
	public abstract class CommunicationServer : CommunicationUnit, ICommunicationServer
	{
		public event Action? ClientConnected;

		public CommunicationServer(ICommunicationStream stream, MessageSerializer serializer) : base(stream, serializer)
		{
		}

		public async Task WaitForConnection()
		{
			await WaitForConnectionInternal();

			ClientConnected?.Invoke();
		}

		protected abstract Task WaitForConnectionInternal();
	}
}