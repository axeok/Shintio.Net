using System;
using Shintio.ReflectionBomb.Types;

namespace Shintio.Communication.ReflectionPipes.Types
{
	public class PipeOptionsWrapper
	{
		public static readonly Type PipeOptionsType =
			AppDomainWrapper.GetOrLoadAssembly("System." + "IO" + ".Pipes")!
				.GetNativeType("System." + "IO" + ".Pipes" + ".PipeOptions")!;

		public static readonly object None = Enum.ToObject(PipeOptionsType, 0);
		public static readonly object WriteThrough = Enum.ToObject(PipeOptionsType, -2147483648);
		public static readonly object Asynchronous = Enum.ToObject(PipeOptionsType, 1073741824);
		public static readonly object CurrentUserOnly = Enum.ToObject(PipeOptionsType, 536870912);
		public static readonly object FirstPipeInstance = Enum.ToObject(PipeOptionsType, 524288);
	}
}