using System.IO.Pipes;
using Shintio.Communication.Core.Common;
using Shintio.Communication.SystemPipes.Streams;

namespace Shintio.Communication.SystemPipes.Common;

public class PipeServer : CommunicationServer
{
	private readonly PipeCommunicationStream<NamedPipeServerStream> _stream;

	private PipeServer(PipeCommunicationStream<NamedPipeServerStream> stream, MessageSerializer serializer)
		: base(stream, serializer)
	{
		_stream = stream;
	}

	public static PipeServer Create(string pipeName)
	{
		return new PipeServer(
			new PipeCommunicationStream<NamedPipeServerStream>(new NamedPipeServerStream(
				pipeName,
				PipeDirection.InOut,
				1,
				PipeTransmissionMode.Message,
				PipeOptions.Asynchronous
			)),
			new MessageSerializer()
		);
	}

	protected override async Task WaitForConnectionInternal()
	{
		await _stream.Pipe.WaitForConnectionAsync();
	}
}