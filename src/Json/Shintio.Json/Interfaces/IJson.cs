using Shintio.Json.Enums;

namespace Shintio.Json.Interfaces
{
	public interface IJson
	{
		public string Serialize(object? value, JsonFormatting formatting = JsonFormatting.None);

#if NETCOREAPP3_0_OR_GREATER
        public T? Deserialize<T>(string json);
#else
		public T Deserialize<T>(string json);
#endif
	}
}