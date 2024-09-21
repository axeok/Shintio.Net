using System;
using System.Threading.Tasks;

namespace Shintio.Communication.Core.Interfaces
{
	public interface ICommunicationUnit : IDisposable
	{
		public event Action Started;
		public event Action Stopped;

		public Task Send(string eventName, params object?[] arguments);
		public Task<object?> Get(string eventName, params object?[] arguments);
		public Task Start();
	}
}