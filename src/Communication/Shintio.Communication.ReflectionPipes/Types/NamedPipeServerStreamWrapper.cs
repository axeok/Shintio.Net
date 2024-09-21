using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Shintio.ReflectionBomb.Types;

namespace Shintio.Communication.ReflectionPipes.Types
{
	public class NamedPipeServerStreamWrapper : PipeStreamWrapper
	{
		public static readonly Type NamedPipeServerStreamType =
			AppDomainWrapper.GetOrLoadAssembly("System." + "IO" + ".Pipes")!
				.GetNativeType("System." + "IO" + ".Pipes" + ".NamedPipeServerStream")!;

		public static readonly ConstructorInfo ConstructorInfo = NamedPipeServerStreamType.GetConstructor(new[]
		{
			typeof(string),
			PipeDirectionWrapper.PipeDirectionType,
			typeof(int),
			PipeTransmissionModeWrapper.PipeTransmissionModeType,
			PipeOptionsWrapper.PipeOptionsType,
		})!;

		public static readonly MethodInfo WaitForConnectionAsyncMethod =
			NamedPipeServerStreamType.GetMethod("WaitForConnectionAsync", new[] { typeof(CancellationToken) })!;

		public NamedPipeServerStreamWrapper(
			string pipeName,
			object direction,
			int maxNumberOfServerInstances,
			object transmissionMode,
			object options
		) : base(ConstructorInfo.Invoke(new object[]
			{ pipeName, direction, maxNumberOfServerInstances, transmissionMode, options }))
		{
		}

		public Task WaitForConnectionAsync() =>
			(Task)WaitForConnectionAsyncMethod.Invoke(Stream, new object[] { CancellationToken.None });
	}
}