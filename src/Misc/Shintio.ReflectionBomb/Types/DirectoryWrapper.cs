using System;
using System.Reflection;
using Shintio.ReflectionBomb.Utils;

namespace Shintio.ReflectionBomb.Types
{
	public static class DirectoryWrapper
	{
		public static readonly Type DirectoryType =
			TypesHelper.GetType(TypesHelper.TypeFromSystem, "System", "IO", "Directory")!;

		private static readonly MethodInfo GetCurrentDirectoryMethod = DirectoryType.GetMethod("GetCurrentDirectory")!;

		public static string GetCurrentDirectory()
		{
			return (string)GetCurrentDirectoryMethod.Invoke(null, new object[] { });
		}
	}
}