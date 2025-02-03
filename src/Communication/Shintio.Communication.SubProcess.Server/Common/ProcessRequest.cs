using System;
using System.Text;
using Shintio.ReflectionBomb.Types;

namespace Shintio.Communication.SubProcess.Server.Common
{
	public class ProcessRequest : IDisposable
	{
		private StringBuilder _builder = new StringBuilder();
		private ProcessWrapper _process;

		public ProcessRequest(ProcessWrapper process)
		{
			_process = process;

			_builder.AppendLine(SubProcessConstants.BeginRequestString);
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
			// TODO: что-то было null

			_builder?.AppendLine(SubProcessConstants.EndRequestString);

			_process?.StandardInput?.Write(_builder?.ToString());
		}
	}
}