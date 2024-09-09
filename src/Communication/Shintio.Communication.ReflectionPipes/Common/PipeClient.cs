using System.Threading.Tasks;
using Shintio.Communication.Core.Common;
using Shintio.Communication.ReflectionPipes.Streams;
using Shintio.Communication.ReflectionPipes.Types;

namespace Shintio.Communication.ReflectionPipes.Common
{
	public class PipeClient : CommunicationClient
	{
		private readonly PipeCommunicationStream<NamedPipeClientStreamWrapper> _stream;

		private PipeClient(PipeCommunicationStream<NamedPipeClientStreamWrapper> stream, MessageSerializer serializer)
			: base(stream, serializer)
		{
			_stream = stream;
		}

		public static PipeClient Create(string pipeName)
		{
			return new PipeClient(
				new PipeCommunicationStream<NamedPipeClientStreamWrapper>(new NamedPipeClientStreamWrapper(
					".",
					pipeName,
					PipeDirectionWrapper.InOut,
					PipeOptionsWrapper.Asynchronous
				)),
				new MessageSerializer()
			);
		}

		protected override async Task ConnectInternal()
		{
			await _stream.Pipe.ConnectAsync();
			_stream.Pipe.ReadMode = PipeTransmissionModeWrapper.Message;
		}
	}
}