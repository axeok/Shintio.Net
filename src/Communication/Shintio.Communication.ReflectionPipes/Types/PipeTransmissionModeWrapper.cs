using System;
using Shintio.ReflectionBomb.Types;

namespace Shintio.Communication.ReflectionPipes.Types
{
	public class PipeTransmissionModeWrapper
	{
		public static readonly Type PipeTransmissionModeType =
			AppDomainWrapper.GetOrLoadAssembly("System." + "IO" + ".Pipes")!
				.GetNativeType("System." + "IO" + ".Pipes" + ".PipeTransmissionMode")!;

		public static readonly object Byte = Enum.ToObject(PipeTransmissionModeType, 0);
		public static readonly object Message = Enum.ToObject(PipeTransmissionModeType, 1);
	}
}