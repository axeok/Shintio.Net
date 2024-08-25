using System;
using System.Reflection;
using Shintio.ReflectionBomb.Utils;

namespace Shintio.ReflectionBomb.Types
{
	public static class PathWrapper
	{
		public static readonly Type PathType = TypesHelper.GetType(TypesHelper.TypeFromSystem, "System", "IO", "Path")!;

		public static readonly char DirectorySeparatorChar =
			(char)PathType.GetField("DirectorySeparatorChar").GetValue(null);

		private static readonly MethodInfo CombineMethod = PathType.GetMethod("Combine", new[] { typeof(string[]) })!;

		public static string Combine(params string[] path)
		{
			return (string)CombineMethod.Invoke(null, new object[] { path });
		}
	}
}