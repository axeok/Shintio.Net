using System;
using Shintio.ReflectionBomb.Types;

namespace Shintio.Communication.ReflectionPipes.Types
{
	public class PipeDirectionWrapper
	{
		public static readonly Type PipeDirectionType =
			AppDomainWrapper.GetOrLoadAssembly("System." + "IO" + ".Pipes")!
				.GetNativeType("System." + "IO" + ".Pipes" + ".PipeDirection")!;

		public static readonly object In = Enum.ToObject(PipeDirectionType, 1);
		public static readonly object Out = Enum.ToObject(PipeDirectionType, 2);
		public static readonly object InOut = Enum.ToObject(PipeDirectionType, 3);
	}
}