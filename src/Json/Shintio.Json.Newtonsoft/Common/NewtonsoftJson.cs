using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Shintio.Json.Common;
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
		private readonly JsonTypesProcessor<JsonConverter> _typesProcessor;

		private readonly object _lock = 0;

		public NewtonsoftJson()
		{
			_serializerSettings = new JsonSerializerSettings()
			{
				ContractResolver = new JsonContractResolver(),
			};
			
			_serializerSettings.Converters.Add(new JsonArrayConverter());
			_serializerSettings.Converters.Add(new JsonObjectConverter());
			_serializerSettings.Converters.Add(new JsonValueConverter());
			
			_typesProcessor =
				new JsonTypesProcessor<JsonConverter>(typeof(NewtonsoftJsonConverter<>), AddConverter);
		}

		public string Serialize(object? value, JsonFormatting formatting = JsonFormatting.None)
		{
			if (value != null)
			{
				_typesProcessor.TryProcessType(value.GetType());
			}

			return JsonConvert.SerializeObject(value, GetFormatting(formatting), _serializerSettings);
		}

#if NETCOREAPP3_0_OR_GREATER
        public T? Deserialize<T>(string json)
#else
		public T Deserialize<T>(string json)
#endif
		{
			_typesProcessor.TryProcessType(typeof(T));

			return JsonConvert.DeserializeObject<T>(json, _serializerSettings);
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

		private void AddConverter(JsonConverter converter)
		{
			lock (_lock)
			{
				_serializerSettings.Converters.Add(converter);
			}
		}

		private Formatting GetFormatting(JsonFormatting formatting) => formatting switch
		{
			JsonFormatting.None => Formatting.None,
			JsonFormatting.Indented => Formatting.Indented,
			_ => Formatting.None,
		};
	}
}