using System;
using global::Newtonsoft.Json;
using global::Newtonsoft.Json.Linq;
using Shintio.Json.Interfaces;
using Shintio.Json.Newtonsoft.Nodes;
using Shintio.Json.Nodes;

namespace Shintio.Json.Newtonsoft.Common
{
	public class NewtonsoftJsonReader : IJsonReader
	{
		private readonly JsonReader _reader;

		public NewtonsoftJsonReader(JsonReader reader)
		{
			_reader = reader;
		}

		public string? GetFullJson()
		{
			return JToken.ReadFrom(_reader).ToString();
		}

		public string? GetString()
		{
			return _reader.Value?.ToString();
		}

		public IJsonObject? GetObject()
		{
			if (_reader.TokenType == JsonToken.Null)
			{
				return null;
			}

			var jObject = JObject.Load(_reader);

			return jObject == null ? null : NewtonsoftJsonNode.Create(jObject) as IJsonObject;
		}
	}
}