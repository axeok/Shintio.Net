using System;
using Shintio.ReflectionBomb.Utils;

namespace Shintio.ReflectionBomb.Types
{
	public class ProcessWrapper
	{
		public static readonly Type ProcessType =
			TypesHelper.GetType(TypesHelper.TypeFromSystem, "System", "Diagnostics", "Process")!;
	}
}