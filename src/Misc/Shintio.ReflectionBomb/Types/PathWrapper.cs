using System;
using System.Reflection;
using Shintio.ReflectionBomb.Enums;
using Shintio.ReflectionBomb.Utils;

namespace Shintio.ReflectionBomb.Types
{
	public static class PathWrapper
	{
		public static readonly Type PathType = TypesHelper.GetType(TypesHelper.TypeFromSystem, "System", "IO", "Path")!;

		public static readonly char DirectorySeparatorChar =
			(char)PathType.GetField("DirectorySeparatorChar").GetValue(null);

		private static readonly MethodInfo CombineMethod = PathType.GetMethod("Combine", new[] { typeof(string[]) })!;
		private static readonly MethodInfo GetTempPathMethod = PathType.GetMethod("GetTempPath")!;

		public static string Combine(params string[] path)
		{
			return (string)CombineMethod.Invoke(null, new object[] { path });
		}

		public static string GetTempPath()
		{
			return (string)GetTempPathMethod.Invoke(null, new object[] { });
		}

		public static string GetTempPathViaCmd()
		{
			return CliHelper.GetOutput("[System.IO.Path]::GetTempPath()", CliInterpreter.Powershell)
				.GetAwaiter()
				.GetResult();
		}
	}
}