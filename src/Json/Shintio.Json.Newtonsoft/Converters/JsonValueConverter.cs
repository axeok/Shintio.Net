using System;
using global::Newtonsoft.Json;
using global::Newtonsoft.Json.Linq;
using Shintio.Json.Newtonsoft.Nodes;
using Shintio.Json.Nodes;

namespace Shintio.Json.Newtonsoft.Converters
{
	public class JsonValueConverter : JsonConverter<IJsonValue>
	{
		public override void WriteJson(JsonWriter writer, IJsonValue? value, JsonSerializer serializer)
		{
			(value as NewtonsoftJsonValue)?.Node.WriteTo(writer);
		}

		public override IJsonValue? ReadJson(
			JsonReader reader,
			Type objectType,
			IJsonValue? existingValue,
			bool hasExistingValue,
			JsonSerializer serializer
		)
		{
			return JToken.ReadFrom(reader) is JValue value ? new NewtonsoftJsonValue(value) : null;
		}
	}
}