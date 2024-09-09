using Shintio.Communication.ReflectionPipes.Common;

namespace Shintio.Net.SecondClient
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var client = PipeClient.Create("Shintio.Net");

			client.Started += () => Console.WriteLine("Client started");
			client.Connected += () => Console.WriteLine("Connected to server");
			client.Stopped += () => Console.WriteLine("Client stopped");

			client.AddEventHandler("TestEvent", TestEventHandler);
			client.AddEventHandler("Plus", PlusEventHandler);

			await client.Connect();

			await client.Start();

			Console.ReadLine();
		}

		private static void TestEventHandler(object?[] obj)
		{
			Console.WriteLine("Test called");
		}

		private static object PlusEventHandler(object?[] obj)
		{
			Console.WriteLine("plus called");

			return (int)obj[0] + (int)obj[1];
		}
	}
}