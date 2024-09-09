using System.Diagnostics;
using Shintio.ReflectionBomb.Types;

namespace Shintio.Communication.SubProcess.Utils
{
	public class ClientRunner
	{
		public ClientRunner(string name)
		{
			Name = name;
		}

		public string Name { get; }

		public void StartClient(byte[] executable)
		{
			var path = $"{PathWrapper.GetTempPath()}/{Name}.exe";

			FileWrapper.WriteAllBytes(path, executable);

			StartClient(path);
		}

		public void StartClient(string path)
		{
			var process = new Process
			{
				StartInfo = new ProcessStartInfo
				{
					FileName = path,
					ArgumentList = { Name },
					UseShellExecute = true,
					CreateNoWindow = false,
				}
			};

			process.Start();
		}
	}
}