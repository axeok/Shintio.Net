using System.Diagnostics;
using Shintio.ReflectionBomb.Types;

namespace Shintio.Communication.SubProcess.Utils
{
	public class ClientRunner
	{
		public ClientRunner(string name, string tempPath = "")
		{
			Name = name;
			TempPath = tempPath;
		}

		public string Name { get; }
		public string TempPath { get; }

		public void StartClient(byte[] executable)
		{
			var path = $"{TempPath}/{Name}.exe";

			FileWrapper.WriteAllBytesViaCmd(path, executable);

			StartClient(path);
		}

		public void StartClient(string path)
		{
			ProcessWrapper.Start(new ProcessStartInfo
			{
				FileName = path,
				ArgumentList = { Name },
				UseShellExecute = true,
				CreateNoWindow = false,
			});
		}
	}
}