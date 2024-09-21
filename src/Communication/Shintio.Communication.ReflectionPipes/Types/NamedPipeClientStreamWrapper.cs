using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Shintio.ReflectionBomb.Types;

namespace Shintio.Communication.ReflectionPipes.Types
{
	public class NamedPipeClientStreamWrapper : PipeStreamWrapper
	{
		public static readonly Type NamedPipeClientStreamType =
			AppDomainWrapper.GetOrLoadAssembly("System." + "IO" + ".Pipes")!
				.GetNativeType("System." + "IO" + ".Pipes" + ".NamedPipeClientStream")!;

		public static readonly ConstructorInfo ConstructorInfo = NamedPipeClientStreamType.GetConstructor(new[]
		{
			typeof(string), typeof(string), PipeDirectionWrapper.PipeDirectionType, PipeOptionsWrapper.PipeOptionsType
		})!;

		public static readonly MethodInfo ConnectAsyncMethod =
			NamedPipeClientStreamType.GetMethod("ConnectAsync", new[] { typeof(int), typeof(CancellationToken) })!;

		public NamedPipeClientStreamWrapper(
			string serverName,
			string pipeName,
			object direction,
			object options
		) : base(ConstructorInfo.Invoke(new object[] { serverName, pipeName, direction, options }))
		{
		}

		public Task ConnectAsync() =>
			(Task)ConnectAsyncMethod.Invoke(Stream, new object[] { Timeout.Infinite, CancellationToken.None });
	}
}