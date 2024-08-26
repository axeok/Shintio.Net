using System;
using System.Reflection;
using System.Text;
using Shintio.ReflectionBomb.Utils;

namespace Shintio.ReflectionBomb.Types
{
	public static class FileWrapper
	{
		public static readonly Type FileType =
#if NETCOREAPP3_0_OR_GREATER
			TypesHelper.GetType(TypesHelper.TypeFromSystem, "System", "IO", "File")!;
#else
			AppDomainWrapper.GetAssembly("System." + "IO" + ".FileSystem")!.GetNativeType("System." + "IO" +
				".File")!;
#endif

		private static readonly MethodInfo ExistsMethod = FileType.GetMethod("Exists")!;
		private static readonly MethodInfo ReadAllBytesMethod = FileType.GetMethod("ReadAllBytes")!;

		public static bool Exists(string path)
		{
			return (bool)ExistsMethod.Invoke(null, new object[] { path });
		}

		public static byte[] ReadAllBytes(string path)
		{
			return (byte[])ReadAllBytesMethod.Invoke(null, new object[] { path });
		}

		public static string ReadAllText(string path)
		{
			var bytes = ReadAllBytes(path);

			return Encoding.UTF8.GetString(bytes);
		}
	}
}