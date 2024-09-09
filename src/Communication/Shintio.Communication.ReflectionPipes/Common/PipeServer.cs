using System.Threading.Tasks;
using Shintio.Communication.Core.Common;
using Shintio.Communication.ReflectionPipes.Streams;
using Shintio.Communication.ReflectionPipes.Types;

namespace Shintio.Communication.ReflectionPipes.Common
{

	public class PipeServer : CommunicationServer
	{
		private readonly PipeCommunicationStream<NamedPipeServerStreamWrapper> _stream;

		private PipeServer(PipeCommunicationStream<NamedPipeServerStreamWrapper> stream, MessageSerializer serializer)
			: base(stream, serializer)
		{
			_stream = stream;
		}

		public static PipeServer Create(string pipeName)
		{
			return new PipeServer(
				new PipeCommunicationStream<NamedPipeServerStreamWrapper>(new NamedPipeServerStreamWrapper(
					pipeName,
					PipeDirectionWrapper.InOut,
					1,
					PipeTransmissionModeWrapper.Message,
					PipeOptionsWrapper.Asynchronous
				)),
				new MessageSerializer()
			);
		}

		protected override async Task WaitForConnectionInternal()
		{
			await _stream.Pipe.WaitForConnectionAsync();
		}
	}
}