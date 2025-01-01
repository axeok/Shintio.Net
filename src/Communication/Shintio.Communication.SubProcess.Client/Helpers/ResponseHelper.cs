using Shintio.Communication.SubProcess.Client.Common;

namespace Shintio.Communication.SubProcess.Client.Helpers
{
	public static class ResponseHelper
	{
		public static ProcessResponse Begin()
		{
			return new ProcessResponse();
		}

		public static ProcessResponse Begin(string text)
		{
			var response = Begin();

			response.AppendLine(text);

			return response;
		}

		public static void Send(string text)
		{
			var response = Begin(text);
			response.Dispose();
		}
	}
}