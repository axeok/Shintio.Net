using System;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Shintio.Json.Enums;
using Shintio.Json.Interfaces;
using Shintio.Json.Newtonsoft.Converters;
using Shintio.Json.Newtonsoft.Nodes;
using Shintio.Json.Nodes;

namespace Shintio.Json.Newtonsoft.Common
{
	public class NewtonsoftJson : IJson
	{
		private readonly JsonSerializerSettings _serializerSettings;

		public NewtonsoftJson()
		{
			_serializerSettings = new JsonSerializerSettings()
			{
				ContractResolver = new JsonContractResolver(this),
			};

			_serializerSettings.Converters.Add(new JsonArrayConverter());
			_serializerSettings.Converters.Add(new JsonObjectConverter());
			_serializerSettings.Converters.Add(new JsonValueConverter());

			// TODO: axe json
			// try
			// {
			// 	JsonConvert.DefaultSettings = GetSettings;
			// }
			// catch(TargetInvocationException ex)
			// {
				var property = typeof(JsonConvert).GetProperty(nameof(JsonConvert.DefaultSettings));
				if (property != null && property.CanWrite)
				{
					Func<JsonSerializerSettings> settingsFunc = GetSettings;

					property.SetValue(null, settingsFunc);
				}
			// }
		}

		private JsonSerializerSettings GetSettings()
		{
			return _serializerSettings;
		}

		public string Serialize(object? value, JsonFormatting formatting = JsonFormatting.None)
		{
			return JsonConvert.SerializeObject(value, GetFormatting(formatting), _serializerSettings);
		}

#if NETCOREAPP3_0_OR_GREATER
        public T? Deserialize<T>(string json)
#else
		public T Deserialize<T>(string json)
#endif
		{
			return JsonConvert.DeserializeObject<T>(json, _serializerSettings);
		}

		public object? Deserialize(string json, Type type)
		{
			return JsonConvert.DeserializeObject(json, type, _serializerSettings);
		}

		public IJsonNode? ParseNode(string json)
		{
			return NewtonsoftJsonNode.Create(JToken.Parse(json));
		}

		public IJsonArray CreateArray()
		{
			return new NewtonsoftJsonArray(new JArray());
		}

		public IJsonObject CreateObject()
		{
			return new NewtonsoftJsonObject(new JObject());
		}

		public IJsonArray CreateArray(object value)
		{
			return new NewtonsoftJsonArray(JArray.FromObject(value));
		}

		public IJsonObject CreateObject(object value)
		{
			return new NewtonsoftJsonObject(JObject.FromObject(value));
		}

		public IJsonNode CreateNode(object value)
		{
			return NewtonsoftJsonNode.Create(JToken.FromObject(value));
		}

		private Formatting GetFormatting(JsonFormatting formatting) => formatting switch
		{
			JsonFormatting.None => Formatting.None,
			JsonFormatting.Indented => Formatting.Indented,
			_ => Formatting.None,
		};
	}
}