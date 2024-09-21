using System;

namespace Shintio.Communication.Core.Interfaces
{
	public interface ICommunicationServer : ICommunicationUnit
	{
		public event Action ClientConnected;
	}
}