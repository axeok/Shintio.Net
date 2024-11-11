using System.Text.Json;
using Shintio.Json.Interfaces;
using Shintio.Json.System.Extensions;

namespace Shintio.Json.System.Common;

public class SystemJsonWriter : IJsonWriter
{
	private readonly Utf8JsonWriter _writer;

	public SystemJsonWriter(Utf8JsonWriter writer)
	{
		_writer = writer;
	}

	public void WriteValue(string value)
	{
		_writer.WriteStringValue(value);
	}

	public void WriteRawValue(string json)
	{
		_writer.WriteRawValue(json);
	}

	public void WriteObject(object? value)
	{
		_writer.WriteObject(value);
	}
}