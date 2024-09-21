using System.IO.Pipes;
using Shintio.Communication.Core.Common;
using Shintio.Communication.SystemPipes.Streams;

namespace Shintio.Communication.SystemPipes.Common;

public class PipeClient : CommunicationClient
{
	private readonly PipeCommunicationStream<NamedPipeClientStream> _stream;

	private PipeClient(PipeCommunicationStream<NamedPipeClientStream> stream, MessageSerializer serializer)
		: base(stream, serializer)
	{
		_stream = stream;
	}

	public static PipeClient Create(string pipeName)
	{
		return new PipeClient(
			new PipeCommunicationStream<NamedPipeClientStream>(new NamedPipeClientStream(
				".",
				pipeName,
				PipeDirection.InOut,
				PipeOptions.Asynchronous
			)),
			new MessageSerializer()
		);
	}

	protected override async Task ConnectInternal()
	{
		await _stream.Pipe.ConnectAsync();
		_stream.Pipe.ReadMode = PipeTransmissionMode.Message;
	}
}