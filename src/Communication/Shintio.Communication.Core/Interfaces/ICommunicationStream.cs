using System;
using System.Threading.Tasks;

namespace Shintio.Communication.Core.Interfaces
{
	public interface ICommunicationStream : IDisposable
	{
		public Task<byte[]> ReadAsync();
		public Task<bool> WriteAsync(byte[] message);
	}
}