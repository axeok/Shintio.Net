using System;
using System.Text;

namespace Shintio.Communication.SubProcess.Client.Common
{
	public class ProcessResponse : IDisposable
	{
		private StringBuilder _builder = new StringBuilder();

		public static Action<string> WriteTo = Console.Write;

		public ProcessResponse()
		{
			_builder.AppendLine(SubProcessConstants.BeginResponseString);
		}

		public void Append(string text)
		{
			_builder.Append(text);
		}

		public void AppendLine(string line)
		{
			_builder.AppendLine(line);
		}

		public void Dispose()
		{
			_builder.AppendLine(SubProcessConstants.EndResponseString);

			WriteTo(_builder.ToString());
		}
	}
}