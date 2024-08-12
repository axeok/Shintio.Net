using Shintio.Json.Enums;
using Shintio.Json.Interfaces;

namespace Shintio.Json.Utils
{
    public static class JsonConverter
    {
        public static IJson Instance = null!;

        public static string Serialize(object value, JsonFormatting formatting = JsonFormatting.None)
        {
            return Instance.Serialize(value, formatting);
        }

#if NETCOREAPP3_0_OR_GREATER
        public static T? Deserialize<T>(string json)
#else
        public static T Deserialize<T>(string json)
#endif
        {
            return Instance.Deserialize<T>(json);
        }
    }
}