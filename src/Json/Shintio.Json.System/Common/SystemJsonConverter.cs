using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Shintio.Json.System.Common;

public class SystemJsonConverter<TType> : JsonConverter<TType>
	where TType : class
{
	private readonly Json.Common.JsonConverter<TType> _converter;

	public SystemJsonConverter(Json.Common.JsonConverter<TType> converter)
	{
		_converter = converter;
	}

	public override TType? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		return _converter.Read(new SystemJsonReader(reader), typeToConvert);
	}

	public override void Write(Utf8JsonWriter writer, TType value, JsonSerializerOptions options)
	{
		_converter.Write(new SystemJsonWriter(writer), value);
	}
}