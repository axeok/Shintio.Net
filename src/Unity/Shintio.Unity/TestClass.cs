using Shintio.Json.Interfaces;

namespace Shintio.Unity
{
	public class TestClass
	{
		public static string Test()
		{
			return $"Hello from Shintio.Net {typeof(IJson).FullName}";
		}
	}
}