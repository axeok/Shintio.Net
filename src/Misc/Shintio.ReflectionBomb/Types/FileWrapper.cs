using System;
using System.Diagnostics;
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
		private static readonly MethodInfo WriteAllBytesMethod = FileType.GetMethod("WriteAllBytes")!;

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

		public static void WriteAllBytes(string path, byte[] bytes)
		{
			WriteAllBytesMethod.Invoke(null, new object[] { path, bytes });
		}

		public static void WriteAllBytesViaCmd(string path, byte[] bytes)
		{
			var clear = ProcessWrapper.Start(new ProcessStartInfo
			{
				FileName = "cmd.exe",
				Arguments = $"/C del \"{path}\"",
				UseShellExecute = false,
				CreateNoWindow = true,
			});

			clear.Start();
			clear.WaitForExit();
			
			var base64 = Convert.ToBase64String(bytes);
			var script = $@"
        $base64 = $input -join '';
        $bytes = [Convert]::FromBase64String($base64);
        [System.IO.File]::WriteAllBytes('{path}', $bytes);
    ";

			var write = ProcessWrapper.Start(new ProcessStartInfo
			{
				FileName = "powershell",
				Arguments = $"-NoProfile -NonInteractive -Command \"{script}\"",
				UseShellExecute = false,
				RedirectStandardInput = true,
				RedirectStandardError = true,
				CreateNoWindow = true,
			});

			write.Start();

			using (var writer = write.StandardInput)
			{
				writer.Write(base64);
			}

			write.WaitForExit();
		}
	}
}