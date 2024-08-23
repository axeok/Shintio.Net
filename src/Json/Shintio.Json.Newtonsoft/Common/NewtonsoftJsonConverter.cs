using System;
using Newtonsoft.Json;

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

		public override void WriteJson(JsonWriter writer, TType? value, JsonSerializer serializer)
		{
			_converter.Write(new NewtonsoftJsonWriter(writer), value);
		}

		public override TType? ReadJson(
			JsonReader reader,
			Type objectType,
			TType? existingValue,
			bool hasExistingValue,
			JsonSerializer serializer
		)
		{
			return _converter.Read(new NewtonsoftJsonReader(reader), objectType);
		}
	}
}