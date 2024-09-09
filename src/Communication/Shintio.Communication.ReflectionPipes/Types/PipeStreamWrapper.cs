using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Shintio.ReflectionBomb.Types;

namespace Shintio.Communication.ReflectionPipes.Types
{
	public class PipeStreamWrapper : IDisposable
	{
		protected readonly object Stream;

		public static readonly Type PipeStreamType =
			AppDomainWrapper.GetOrLoadAssembly("System." + "IO" + ".Pipes")!
				.GetNativeType("System." + "IO" + ".Pipes" + ".PipeStream")!;

		public static readonly PropertyInfo CanReadProperty = PipeStreamType.GetProperty("CanRead")!;
		public static readonly PropertyInfo CanWriteProperty = PipeStreamType.GetProperty("CanWrite")!;

		public static readonly PropertyInfo
			IsMessageCompleteProperty = PipeStreamType.GetProperty("IsMessageComplete")!;

		public static readonly PropertyInfo ReadModeProperty = PipeStreamType.GetProperty("ReadMode")!;

		public static readonly MethodInfo ReadAsyncMethod =
			PipeStreamType.GetMethod("ReadAsync", new[] { typeof(byte[]), typeof(int), typeof(int) })!;

		public static readonly MethodInfo WriteAsyncMethod =
			PipeStreamType.GetMethod("WriteAsync", new[] { typeof(ReadOnlyMemory<byte>), typeof(CancellationToken) })!;

		public static readonly MethodInfo FlushAsyncMethod =
			PipeStreamType.GetMethod("FlushAsync", new[] { typeof(CancellationToken) })!;

		public static readonly MethodInfo WaitForPipeDrainMethod = PipeStreamType.GetMethod("WaitForPipeDrain")!;

		public PipeStreamWrapper(object stream)
		{
			Stream = stream;
		}

		public bool CanRead => (bool)CanReadProperty.GetValue(Stream);
		public bool CanWrite => (bool)CanWriteProperty.GetValue(Stream);
		public bool IsMessageComplete => (bool)IsMessageCompleteProperty.GetValue(Stream);

		public object ReadMode
		{
			get => (object)ReadModeProperty.GetValue(Stream);
			set => ReadModeProperty.SetValue(Stream, value);
		}

		public void WaitForPipeDrain() => WaitForPipeDrainMethod.Invoke(Stream, null);

		public Task<int> ReadAsync(byte[] buffer, int offset, int count) =>
			(Task<int>)ReadAsyncMethod.Invoke(Stream, new object[] { buffer, offset, count });

		public ValueTask WriteAsync(
			ReadOnlyMemory<byte> buffer,
			CancellationToken cancellationToken = default(CancellationToken)
		) =>
			(ValueTask)WriteAsyncMethod.Invoke(Stream, new object[] { buffer, cancellationToken });

		public Task FlushAsync() =>
			(Task)FlushAsyncMethod.Invoke(Stream, new object[] { CancellationToken.None });

		public void Dispose()
		{
			(Stream as IDisposable)?.Dispose();
		}
	}
}