using System;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Shintio.Json.Nodes;
using Shintio.Json.System.Nodes;

namespace Shintio.Json.System.Converters;

public class JsonObjectInterfaceConverter : JsonConverter<IJsonObject>
{
	public override IJsonObject? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		return JsonNode.Parse(ref reader) is not JsonObject value ? null : new SystemJsonObject(value);
	}

	public override void Write(Utf8JsonWriter writer, IJsonObject value, JsonSerializerOptions options)
	{
		(value as SystemJsonObject)?.Node.WriteTo(writer, options);
	}
}

public class JsonObjectConverter : JsonConverter<SystemJsonObject>
{
	public override SystemJsonObject? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		return JsonNode.Parse(ref reader) is not JsonObject value ? null : new SystemJsonObject(value);
	}

	public override void Write(Utf8JsonWriter writer, SystemJsonObject value, JsonSerializerOptions options)
	{
		(value as SystemJsonObject)?.Node.WriteTo(writer, options);
	}
}