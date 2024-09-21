using System.Text.Json;
using Shintio.Json.Interfaces;

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
}