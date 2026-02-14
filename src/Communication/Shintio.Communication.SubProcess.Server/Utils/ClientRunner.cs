using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Shintio.Communication.SubProcess.Server.Common;
using Shintio.ReflectionBomb.Types;

namespace Shintio.Communication.SubProcess.Server.Utils
{
	public class ClientRunner : IDisposable
	{
		public delegate void MessageReceivedDelegate(string message);

		public event MessageReceivedDelegate? MessageReceived;

		public ClientRunner(string name, string tempPath = "")
		{
			Name = name;
			TempPath = tempPath;
		}

		~ClientRunner()
		{
			KillProcess();
		}

		public string Name { get; }
		public string TempPath { get; }
		public ProcessWrapper? Process { get; private set; }

		public void StartClient(byte[] executable)
		{
			var path = $"{TempPath}/{Name}.exe";

			// FileWrapper.WriteAllBytesViaCmd(path, executable);

			StartClient(path);
		}

		public void StartClient(string path)
		{
			// Process = ProcessWrapper.Start(new ProcessStartInfo
			// {
			// 	FileName = path,
			// 	ArgumentList = { Name },
			// 	UseShellExecute = false,
			// 	CreateNoWindow = true,
			// 	RedirectStandardOutput = true,
			// 	RedirectStandardInput = true,
			// });

			InitWorkers();
		}

		public ProcessRequest BeginRequest()
		{
			return new ProcessRequest(Process!);
		}

		public ProcessRequest BeginRequest(string text)
		{
			var response = BeginRequest();

			response.AppendLine(text);

			return response;
		}

		public void Send(string text)
		{
			var response = BeginRequest(text);
			response.Dispose();
		}

		private void InitWorkers()
		{
			if (Process == null)
			{
				return;
			}

			_ = Task.Run(() =>
			{
				var started = false;
				var content = new StringBuilder();

				while (Process?.HasExited == false)
				{
					var line = Process.StandardOutput.ReadLine();
					if (line == null)
					{
						continue;
					}

					if (line == SubProcessConstants.BeginResponseString)
					{
						started = true;
						content = new StringBuilder();
					}
					else if (line == SubProcessConstants.EndResponseString)
					{
						started = false;

						var result = content.ToString();
						if (string.IsNullOrWhiteSpace(result))
						{
							continue;
						}

						MessageReceived?.Invoke(result);
					}
					else if (started)
					{
						content.AppendLine(line);
					}
				}
			});

			_ = Task.Run(() =>
			{
				var timer = new Timer
				{
					Interval = SubProcessConstants.PingDelay.TotalMilliseconds,
					AutoReset = true,
				};

				timer.Elapsed += (s, e) =>
				{
					if (Process?.HasExited != false)
					{
						timer.Stop();
						return;
					}

					Send(SubProcessConstants.PingString);
				};

				timer.Start();
			});
		}

		public void Dispose()
		{
			KillProcess();
		}

		private void KillProcess()
		{
			try
			{
				var process = Process;
				Process = null;

				process?.Kill();
			}
			catch
			{
				// ignored
			}
		}
	}
}