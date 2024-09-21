using System;
using Shintio.Essentials.Common;
using Shintio.Json.Common;
using Shintio.Json.Interfaces;

namespace Shintio.Essentials.Converters
{
	public class DataCollectionJsonConverter<T> : JsonConverter<T> where T : DataCollection
	{
		public override void Write(IJsonWriter writer, T value)
		{
			// TODO: axe
			writer.WriteValue(value?.Key);
		}

		public override T Read(IJsonReader reader, Type type)
		{
			var key = reader.GetString();
			// TODO: axe
			return key == null ? null : DataCollection.TryParseOrDefault<T>(key);
		}
	}
}