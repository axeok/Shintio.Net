using System;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Shintio.Json.Nodes;
using Shintio.Json.System.Nodes;

namespace Shintio.Json.System.Converters;

public class JsonValueInterfaceConverter : JsonConverter<IJsonValue>
{
	public override IJsonValue? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		return JsonNode.Parse(ref reader) is not JsonValue value ? null : new SystemJsonValue(value);
	}

	public override void Write(Utf8JsonWriter writer, IJsonValue value, JsonSerializerOptions options)
	{
		(value as SystemJsonValue)?.Node.WriteTo(writer, options);
	}
}

public class JsonValueConverter : JsonConverter<SystemJsonValue>
{
	public override SystemJsonValue? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		return JsonNode.Parse(ref reader) is not JsonValue value ? null : new SystemJsonValue(value);
	}

	public override void Write(Utf8JsonWriter writer, SystemJsonValue value, JsonSerializerOptions options)
	{
		(value as SystemJsonValue)?.Node.WriteTo(writer, options);
	}
}