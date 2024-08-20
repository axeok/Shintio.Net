using Shintio.Json.Interfaces;
using UnityEngine;
using UnityEngine.Advertisements;

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