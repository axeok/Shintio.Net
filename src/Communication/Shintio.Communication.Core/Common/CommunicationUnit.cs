using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shintio.Communication.Core.Enums;
using Shintio.Communication.Core.Interfaces;

namespace Shintio.Communication.Core.Common
{
	public abstract class CommunicationUnit : ICommunicationUnit
	{
		private int _nextMessageId = 0;

		public event Action? Started;
		public event Action? Stopped;

		private readonly ICommunicationStream _stream;
		private readonly MessageSerializer _serializer;

		private readonly Dictionary<int, TaskCompletionSource<byte[]>> _responseSources =
			new Dictionary<int, TaskCompletionSource<byte[]>>();

		private readonly Dictionary<string, Action<object?[]>> _messagesHandlers =
			new Dictionary<string, Action<object?[]>>();

		private readonly Dictionary<string, Func<object?[], object?>> _requestsHandlers =
			new Dictionary<string, Func<object?[], object?>>();

		public CommunicationUnit(ICommunicationStream stream, MessageSerializer serializer)
		{
			_serializer = serializer;

			_stream = stream;
		}

		public async Task Send(string eventName, params object?[] args)
		{
			var message = _serializer.Serialize(new[] { (object)eventName }.Concat(args).ToArray());

			_ = await _stream.WriteAsync(new[] { (byte)MessageType.Message }.Concat(message).ToArray());
		}

		public async Task<object?> Get(string eventName, params object?[] args)
		{
			var message = _serializer.Serialize(new[] { (object)eventName }.Concat(args).ToArray());
			var id = _nextMessageId++;

			var header = new byte[1 + sizeof(int)];

			header[0] = (byte)MessageType.Request;
			Buffer.BlockCopy(BitConverter.GetBytes(id), 0, header, 1, sizeof(int));

			var source = new TaskCompletionSource<byte[]>();
			_responseSources[id] = source;

			_ = await _stream.WriteAsync(header.Concat(message).ToArray());

			return _serializer.Deserialize(await source.Task).FirstOrDefault();
		}

		private async Task Response(int id, object? result)
		{
			var message = _serializer.Serialize(new[] { result });
			var header = new byte[1 + sizeof(int)];

			header[0] = (byte)MessageType.Response;
			Buffer.BlockCopy(BitConverter.GetBytes(id), 0, header, 1, sizeof(int));

			_ = await _stream.WriteAsync(header.Concat(message).ToArray());
		}

		public virtual void Dispose()
		{
			Stopped?.Invoke();
			_stream.Dispose();
		}

		public async Task Start()
		{
			await Task.Factory.StartNew(async () =>
			{
				try
				{
					while (true)
					{
						OnMessageReceived(await _stream.ReadAsync());
					}
				}
				catch (InvalidOperationException e)
				{
					Dispose();
				}
			});

			Started?.Invoke();
		}

		public void AddEventHandler(string eventName, Action<object?[]> handler)
		{
			_messagesHandlers[eventName] = handler;
		}

		public void RemoveEventHandler(string eventName, Action<object?[]> handler)
		{
			_messagesHandlers.Remove(eventName);
		}

		public void AddEventHandler(string eventName, Func<object?[], object?> handler)
		{
			_requestsHandlers[eventName] = handler;
		}

		public void RemoveEventHandler(string eventName, Func<object?[], object?> handler)
		{
			_requestsHandlers.Remove(eventName);
		}

		private void OnMessageReceived(byte[] data)
		{
			var memory = new Memory<byte>(data);

			var type = (MessageType)memory.Span[0];
			memory = memory.Slice(1);

			switch (type)
			{
				case MessageType.Message:
				{
					var message = _serializer.Deserialize(memory);
					var nameObject = message.ElementAtOrDefault(0);
					if (nameObject is string name && _messagesHandlers.TryGetValue(name, out var handler))
					{
						try
						{
							handler.Invoke(message.Skip(1).ToArray());
						}
						catch (Exception e)
						{
							Console.Error.WriteLine(e);
						}
					}
				}
					break;
				case MessageType.Request:
				{
					var id = BitConverter.ToInt32(memory.Span);
					memory = memory.Slice(sizeof(int));

					var request = _serializer.Deserialize(memory);
					var nameObject = request.ElementAtOrDefault(0);
					if (nameObject is string name && _requestsHandlers.TryGetValue(name, out var handler))
					{
						var result = handler.Invoke(request.Skip(1).ToArray());

						Response(id, result).GetAwaiter().GetResult();
					}
				}
					break;
				case MessageType.Response:
				{
					var id = BitConverter.ToInt32(memory.Span);
					memory = memory.Slice(sizeof(int));

					if (_responseSources.TryGetValue(id, out var source))
					{
						source.TrySetResult(memory.ToArray());
					}
				}
					break;
			}
		}
	}
}