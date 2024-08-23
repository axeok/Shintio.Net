using Shintio.Json.Enums;
using Shintio.Json.Interfaces;

namespace Shintio.Json.Utils
{
	public static class JsonConverter
	{
		public static IJson Instance = null!;

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
			return Instance.Deserialize<T>(json);
		}
	}
}