using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Shintio.Essentials.Common;

namespace Shintio.Json.System.Converters;

public class DataCollectionDictionaryJsonConverter : JsonConverterFactory
{
	public override bool CanConvert(Type typeToConvert)
	{
		if (
			(typeToConvert.IsGenericType && typeToConvert.GetGenericTypeDefinition() != typeof(Dictionary<,>)) &&
			FindBaseType(typeToConvert, typeof(Dictionary<,>)) == null)
		{
			return false;
		}

		var key = typeToConvert.GetGenericArguments().FirstOrDefault();
		if (
			key == null ||
			(key != typeof(DataCollection) && !key.IsSubclassOf(typeof(DataCollection)))
		)
		{
			return false;
		}

		return true;
	}

	public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
	{
		Type keyType = typeToConvert.GetGenericArguments()[0];
		Type valueType = typeToConvert.GetGenericArguments()[1];

		JsonConverter converter = (JsonConverter)Activator.CreateInstance(
			typeof(KeyedDictionaryJsonConverterConverterInner<,>).MakeGenericType(keyType, valueType),
			BindingFlags.Instance | BindingFlags.Public,
			binder: null, args: null, culture: null);

		return converter;
	}

	private class KeyedDictionaryJsonConverterConverterInner<TKey, TValue> :
		JsonConverter<Dictionary<TKey, TValue>>
		where TKey : DataCollection
	{
		public override Dictionary<TKey, TValue> Read(
			ref Utf8JsonReader reader,
			Type typeToConvert,
			JsonSerializerOptions options
		)
		{
			var dictionary = JsonSerializer.Deserialize<Dictionary<string, TValue>>(ref reader, options: options);
			return dictionary.ToDictionary(p => DataCollection.TryParse<TKey>(p.Key), p => p.Value);
		}

		public override void Write(
			Utf8JsonWriter writer,
			Dictionary<TKey, TValue> dictionary,
			JsonSerializerOptions options
		)
		{
			JsonSerializer.Serialize(writer, dictionary.ToDictionary(p => p.Key.Key, p => p.Value), options);
		}
	}

	private static Type? FindBaseType(Type? targetType, Type baseType)
	{
		while (targetType != null && targetType != typeof(object))
		{
			if (
				targetType == baseType ||
				(targetType.IsGenericType && targetType.GetGenericTypeDefinition() == baseType)
			)
			{
				return targetType;
			}

			targetType = targetType.BaseType;
		}

		return null;
	}
}