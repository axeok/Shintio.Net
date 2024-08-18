using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Shintio.Json.Newtonsoft.Nodes;
using Shintio.Json.Nodes;

namespace Shintio.Json.Newtonsoft.Converters
{
	public class JsonArrayConverter : JsonConverter<IJsonArray>
	{
		public override void WriteJson(JsonWriter writer, IJsonArray? value, JsonSerializer serializer)
		{
			(value as NewtonsoftJsonArray)?.Node.WriteTo(writer);
		}

		public override IJsonArray? ReadJson(
			JsonReader reader,
			Type objectType,
			IJsonArray? existingValue,
			bool hasExistingValue,
			JsonSerializer serializer
		)
		{
			return JToken.ReadFrom(reader) is JArray value ? new NewtonsoftJsonArray(value) : null;
		}
	}
}