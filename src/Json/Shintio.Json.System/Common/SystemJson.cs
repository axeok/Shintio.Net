using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Shintio.Json.Enums;
using Shintio.Json.Interfaces;
using Shintio.Json.Nodes;
using Shintio.Json.System.Converters;
using Shintio.Json.System.Nodes;

namespace Shintio.Json.System.Common
{
	public class SystemJson : IJson
	{
		private readonly JsonSerializerOptions _noneFormattingOptions;
		private readonly JsonSerializerOptions _indentedFormattingOptions;

		public SystemJson()
		{
			_noneFormattingOptions = new JsonSerializerOptions()
			{
#if NET7_0_OR_GREATER
				TypeInfoResolver = new JsonTypeInfoResolver(this),
#endif
			};
			_indentedFormattingOptions = new JsonSerializerOptions()
			{
				WriteIndented = true,
#if NET7_0_OR_GREATER
				TypeInfoResolver = new JsonTypeInfoResolver(this),
#endif
			};

			foreach (var converter in GetConverters())
			{
				_noneFormattingOptions.Converters.Add(converter);
				_indentedFormattingOptions.Converters.Add(converter);
			}
		}

		public string Serialize(object? value, JsonFormatting formatting = JsonFormatting.None)
		{
			return JsonSerializer.Serialize(value,
				formatting == JsonFormatting.Indented ? _indentedFormattingOptions : _noneFormattingOptions);
		}

#if NETCOREAPP3_0_OR_GREATER
		public T? Deserialize<T>(string json)
#else
        public T Deserialize<T>(string json)
#endif
		{
			return JsonSerializer.Deserialize<T>(json, _noneFormattingOptions);
		}

		public object? Deserialize(string json, Type type)
		{
			return JsonSerializer.Deserialize(json, type, _noneFormattingOptions);
		}

		public IJsonNode? ParseNode(string json)
		{
			return SystemJsonNode.Create(JsonNode.Parse(json));
		}

		public IJsonArray CreateArray()
		{
			return new SystemJsonArray(new JsonArray());
		}

		public IJsonObject CreateObject()
		{
			return new SystemJsonObject(new JsonObject());
		}

		public IJsonArray CreateArray(object value)
		{
			// TODO: axe json
			return Deserialize<IJsonArray>(Serialize(value));
		}

		public IJsonObject CreateObject(object value)
		{
			// TODO: axe json
			return Deserialize<IJsonObject>(Serialize(value));
		}

		private IEnumerable<JsonConverter> GetConverters()
		{
			yield return new JsonArrayInterfaceConverter();
			yield return new JsonObjectInterfaceConverter();
			yield return new JsonValueInterfaceConverter();

			yield return new JsonArrayConverter();
			yield return new JsonObjectConverter();
			yield return new JsonValueConverter();
		}
	}
}