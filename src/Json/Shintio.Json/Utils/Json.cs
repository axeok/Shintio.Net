using System;
using System.Reflection;
using System.Text;
using Shintio.Json.Enums;
using Shintio.Json.Interfaces;

namespace Shintio.Json.Utils
{
	public static class JsonConverter
	{
		public static IJson Instance { get; set; } = null!;

		public static string Serialize(object? value, JsonFormatting formatting)
		{
			return Instance.Serialize(value, formatting);
		}

		public static string Serialize(object? value)
		{
			return Serialize(value, JsonFormatting.None);
		}

#if DEBUG
		public static T? Deserialize<T>(string json)
#else
		public static T Deserialize<T>(string json)
#endif
		{
// fix for xunit
#if DEBUG
			// ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
			if (Instance == null)
			{
				Assembly.Load("Shintio.Json.Newtonsoft");
				foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
				{
					foreach (var type in assembly.GetTypes())
					{
						if (type.FullName == "Shintio.Json.Newtonsoft.Common.NewtonsoftJson")
						{
							Instance = (Activator.CreateInstance(type) as IJson)!;
						}
					}
				}
			}
#endif

			return Instance!.Deserialize<T>(json);
		}
	}
}