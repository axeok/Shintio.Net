using System.Text.Json;
using Shintio.Json.Interfaces;

namespace Shintio.Json.System.Common;

public class SystemJsonReader : IJsonReader
{
	private readonly string? _value;

	public SystemJsonReader(ref Utf8JsonReader reader)
	{
		using var document = JsonDocument.ParseValue(ref reader);

		_value = document.RootElement.GetRawText();
		// _value = reader.GetString();
	}

	public string? GetFullJson()
	{
		return _value;
	}
}