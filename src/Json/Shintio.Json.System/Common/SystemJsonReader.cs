using System.Text.Json;
using System.Text.Json.Nodes;
using Shintio.Json.Interfaces;
using Shintio.Json.Nodes;
using Shintio.Json.System.Nodes;

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

	public string? GetString()
	{
		return _value?.Trim('\"');
	}

	public IJsonObject? GetObject()
	{
		var jsonNode = JsonNode.Parse(_value ?? "");

		return jsonNode == null ? null : SystemJsonNode.Create(jsonNode) as IJsonObject;
	}
}