using System;
using System.Text;

namespace Shintio.Communication.SubProcess
{
	public static class SubProcessConstants
	{
		// Client -> Server
		public static readonly string BeginResponseString = UniquizeString("BeginResponse");
		public static readonly string EndResponseString = UniquizeString("EndResponse");

		// Server -> Client
		public static readonly string BeginRequestString = UniquizeString("BeginRequest");
		public static readonly string EndRequestString = UniquizeString("EndRequest");

		public static readonly string PingString = UniquizeString("Ping");
		public static readonly TimeSpan PingDelay = TimeSpan.FromSeconds(3);
		public static readonly TimeSpan PingTimeout = TimeSpan.FromSeconds(10);

		private static string UniquizeString(string value)
		{
			return $"#!$-={Convert.ToBase64String(Encoding.UTF8.GetBytes($"Q1{value}1Q"))}=-$!#";
		}
	}
}