using System;
using global::Newtonsoft.Json;
using global::Newtonsoft.Json.Linq;
using Shintio.Json.Newtonsoft.Nodes;
using Shintio.Json.Nodes;

namespace Shintio.Json.Newtonsoft.Converters
{
	public class JsonObjectConverter : JsonConverter<IJsonObject>
	{
		public override void WriteJson(JsonWriter writer, IJsonObject? value, JsonSerializer serializer)
		{
			(value as NewtonsoftJsonObject)?.Node.WriteTo(writer);
		}

		public override IJsonObject? ReadJson(
			JsonReader reader,
			Type objectType,
			IJsonObject? existingValue,
			bool hasExistingValue,
			JsonSerializer serializer
		)
		{
			return JToken.ReadFrom(reader) is JObject value ? new NewtonsoftJsonObject(value) : null;
		}
	}
}