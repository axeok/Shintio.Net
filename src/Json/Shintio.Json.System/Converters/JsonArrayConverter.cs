using System;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Shintio.Json.Nodes;
using Shintio.Json.System.Nodes;

namespace Shintio.Json.System.Converters;

public class JsonArrayInterfaceConverter : JsonConverter<IJsonArray>
{
	public override IJsonArray? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		return JsonNode.Parse(ref reader) is not JsonArray value ? null : new SystemJsonArray(value);
	}

	public override void Write(Utf8JsonWriter writer, IJsonArray value, JsonSerializerOptions options)
	{
		(value as SystemJsonArray)?.Node.WriteTo(writer, options);
	}
}

public class JsonArrayConverter : JsonConverter<SystemJsonArray>
{
	public override SystemJsonArray? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		return JsonNode.Parse(ref reader) is not JsonArray value ? null : new SystemJsonArray(value);
	}

	public override void Write(Utf8JsonWriter writer, SystemJsonArray value, JsonSerializerOptions options)
	{
		(value as SystemJsonArray)?.Node.WriteTo(writer, options);
	}
}