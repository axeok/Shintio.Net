using System;
using System.Reflection;
using Shintio.ReflectionBomb.Utils;

namespace Shintio.ReflectionBomb.Types
{
	public static class DirectoryWrapper
	{
		public static readonly Type DirectoryType =
#if NETCOREAPP3_0_OR_GREATER
			TypesHelper.GetType(TypesHelper.TypeFromSystem, "System", "IO", "Directory")!;
#else
			AppDomainWrapper.GetAssembly("System." + "IO" + ".FileSystem")!.GetNativeType("System." + "IO" +
				".Directory")!;
#endif

		private static readonly MethodInfo GetCurrentDirectoryMethod = DirectoryType.GetMethod("GetCurrentDirectory")!;

		private static readonly MethodInfo GetDirectoriesMethod =
			DirectoryType.GetMethod("GetDirectories", new[] { typeof(string) })!;

		private static readonly MethodInfo GetFilesMethod =
			DirectoryType.GetMethod("GetFiles", new[] { typeof(string) })!;

		public static string GetCurrentDirectory()
		{
			return (string)GetCurrentDirectoryMethod.Invoke(null, new object[] { });
		}

		public static string[] GetDirectories(string path)
		{
			return (string[])GetDirectoriesMethod.Invoke(null, new object[] { path });
		}

		public static string[] GetFiles(string path)
		{
			return (string[])GetFilesMethod.Invoke(null, new object[] { path });
		}
	}
}