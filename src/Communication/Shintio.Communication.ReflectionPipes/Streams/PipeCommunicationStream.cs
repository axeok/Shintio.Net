using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shintio.Communication.Core.Interfaces;
using Shintio.Communication.ReflectionPipes.Types;

namespace Shintio.Communication.ReflectionPipes.Streams
{
	public class PipeCommunicationStream<TPipe> : ICommunicationStream where TPipe : PipeStreamWrapper
	{
		private readonly TPipe _pipe;

		public PipeCommunicationStream(TPipe pipe)
		{
			_pipe = pipe;
		}

		public TPipe Pipe => _pipe;

		public async Task<byte[]> ReadAsync()
		{
			var result = new List<byte>();
			var messageBytes = new byte[256];
			if (!_pipe.CanRead)
			{
				return Array.Empty<byte>();
			}

			do
			{
				var bytesRead = await _pipe.ReadAsync(messageBytes, 0, messageBytes.Length);

				if (bytesRead > 0)
				{
					result.AddRange(messageBytes.Take(bytesRead));
					Array.Clear(messageBytes, 0, messageBytes.Length);
				}
				else
				{
					throw new InvalidOperationException("Disconnected");
				}
			} while (!_pipe.IsMessageComplete);

			return result.ToArray();
		}

		public async Task<bool> WriteAsync(byte[] message)
		{
			if (_pipe.CanWrite && message.Length > 0)
			{
				await _pipe.WriteAsync(message);
				await _pipe.FlushAsync();
				_pipe.WaitForPipeDrain();

				return true;
			}

			return false;
		}

		public void Dispose()
		{
			_pipe.Dispose();
		}
	}
}