using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Shintio.Json.Enums;
using Shintio.Json.Extensions;
using Shintio.Json.Interfaces;

namespace Shintio.Database.Converters;

public abstract class ValueConverterFactory
{
	public abstract ValueConverter Create();
}

public class ObjectToJsonConverterFactory<TObject> : ValueConverterFactory
{
	private readonly IJson _json;
	private readonly Func<TObject> _getDefaultValue;

	public ObjectToJsonConverterFactory(IJson json, Func<TObject> getDefaultValue)
	{
		_json = json;
		_getDefaultValue = getDefaultValue;
	}

	public override ValueConverter<TObject, string> Create()
	{
		return new ValueConverter<TObject, string>(
			v => _json.Serialize(v, JsonFormatting.None),
			v => _json.DeserializeOrDefault<TObject>(v, _getDefaultValue)
		);
	}
}