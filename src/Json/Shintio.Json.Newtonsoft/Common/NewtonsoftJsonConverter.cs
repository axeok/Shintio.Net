using System;
using global::Newtonsoft.Json;

namespace Shintio.Json.Newtonsoft.Common
{
	public class NewtonsoftJsonConverter<TType> : JsonConverter<TType> where TType : class
	{
		private readonly Json.Common.JsonConverter<TType> _converter;

		public override bool CanRead => _converter.CanRead;
		public override bool CanWrite => _converter.CanWrite;

		public NewtonsoftJsonConverter(Json.Common.JsonConverter<TType> converter)
		{
			_converter = converter;
		}

#if NETCOREAPP3_0_OR_GREATER
		public override void WriteJson(JsonWriter writer, TType? value, JsonSerializer serializer)
#else
		public override void WriteJson(JsonWriter writer, TType value, JsonSerializer serializer)
#endif
		{
			_converter.Write(new NewtonsoftJsonWriter(writer), value);
		}

#if NETCOREAPP3_0_OR_GREATER
		public override TType? ReadJson(
			JsonReader reader,
			Type objectType,
			TType? existingValue,
			bool hasExistingValue,
			JsonSerializer serializer
		)
#else
		public override TType ReadJson(
			JsonReader reader,
			Type objectType,
			TType existingValue,
			bool hasExistingValue,
			JsonSerializer serializer
		)
#endif
		{
			return _converter.Read(new NewtonsoftJsonReader(reader), objectType);
		}
	}
}