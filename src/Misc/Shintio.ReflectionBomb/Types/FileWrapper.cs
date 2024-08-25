using System;
using System.Reflection;
using System.Text;
using Shintio.ReflectionBomb.Utils;

namespace Shintio.ReflectionBomb.Types
{
	public static class FileWrapper
	{
		public static readonly Type FileType = GetFileType();

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

		private static Type GetFileType()
		{
			var possibleNames = new[]
			{
				new[] { "System", "IO", "File" },
				new[] { "Internal", "IO", "File" },
			};

			foreach (var name in possibleNames)
			{
				var type = TypesHelper.GetType(TypesHelper.TypeFromSystem, name);
				if (type != null)
				{
					return type;
				}
			}

			throw new Exception("File type not found");
		}
	}
}