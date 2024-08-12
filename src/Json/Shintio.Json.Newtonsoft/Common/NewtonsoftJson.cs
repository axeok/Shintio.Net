using Newtonsoft.Json;
using Shintio.Json.Common;
using Shintio.Json.Enums;
using Shintio.Json.Interfaces;

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
			_typesProcessor =
				new JsonTypesProcessor<JsonConverter>(typeof(NewtonsoftJsonConverter<>), AddConverter);
		}

		public string Serialize(object value, JsonFormatting formatting = JsonFormatting.None)
		{
			_typesProcessor.TryProcessType(value.GetType());

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