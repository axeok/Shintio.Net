using System.Text.Json;
using Shintio.Json.Interfaces;

namespace Shintio.Json.System.Common;

public class SystemJsonReader : IJsonReader
{
	private readonly string? _value;

	public SystemJsonReader(Utf8JsonReader reader)
	{
		_value = reader.GetString();
	}

	public string? Read()
	{
		return _value;
	}
}