using System;
using System.Text;
using System.Timers;

namespace Shintio.Communication.SubProcess.Client.Helpers
{
	public static class RequestHelper
	{
		public static event Action<string>? MessageReceived;
		public static event Action? Closed;

		private static Timer? _timer;

		public static void StartListen()
		{
			UpdateTimer();
			
			var started = false;
			var content = new StringBuilder();

			while (true)
			{
				var line = Console.ReadLine();
				if (line == null)
				{
					continue;
				}

				if (line == SubProcessConstants.BeginRequestString)
				{
					started = true;
					content = new StringBuilder();
				}
				else if (line == SubProcessConstants.EndRequestString)
				{
					started = false;
					
					var result = content.ToString();
					if (string.IsNullOrWhiteSpace(result))
					{
						continue;
					}
					
					MessageReceived?.Invoke(result);
				}
				else if (line == SubProcessConstants.PingString)
				{
					UpdateTimer();
				}
				else if (started)
				{
					content.AppendLine(line);
				}
			}
		}

		private static void UpdateTimer()
		{
			_timer?.Stop();

			_timer = new Timer()
			{
				Interval = SubProcessConstants.PingTimeout.TotalMilliseconds,
				AutoReset = false,
			};

			_timer.Elapsed += TimerOnElapsed;

			_timer.Start();
		}

		private static void TimerOnElapsed(object? sender, ElapsedEventArgs e)
		{
			Closed?.Invoke();
			Environment.Exit(0);
		}
	}
}