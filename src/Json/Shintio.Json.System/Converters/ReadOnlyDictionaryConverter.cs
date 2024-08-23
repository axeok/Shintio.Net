using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Shintio.Json.System.Converters;

public class ReadOnlyDictionaryJsonConverter : JsonConverterFactory
{
	public override bool CanConvert(Type typeToConvert)
	{
		if (!typeToConvert.GetInterfaces().Any(i =>
			    i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IReadOnlyDictionary<,>)))
			return false;

		if (
			(typeToConvert.IsGenericType &&
			 typeToConvert.GetGenericTypeDefinition() != typeof(ReadOnlyDictionary<,>)) &&
			FindBaseType(typeToConvert, typeof(ReadOnlyDictionary<,>)) == null)
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
			typeof(IReadOnlyDictionaryConverterInner<,>).MakeGenericType(keyType, valueType),
			BindingFlags.Instance | BindingFlags.Public,
			binder: null, args: null, culture: null);

		return converter;
	}

	private class IReadOnlyDictionaryConverterInner<TKey, TValue> :
		JsonConverter<IReadOnlyDictionary<TKey, TValue>>
		where TKey : notnull
	{
		public override IReadOnlyDictionary<TKey, TValue> Read(
			ref Utf8JsonReader reader,
			Type typeToConvert,
			JsonSerializerOptions options
		)
		{
			var dictionary = JsonSerializer.Deserialize<Dictionary<TKey, TValue>>(ref reader, options: options);
			return (IReadOnlyDictionary<TKey, TValue>)Activator.CreateInstance(
				typeToConvert,
				BindingFlags.Instance | BindingFlags.Public,
				binder: null,
#pragma warning disable CS8601 // Possible null reference assignment.
				args: new object[] { dictionary },
#pragma warning restore CS8601 // Possible null reference assignment.
				culture: null);
		}

		public override void Write(
			Utf8JsonWriter writer,
			IReadOnlyDictionary<TKey, TValue> dictionary,
			JsonSerializerOptions options
		) =>
			JsonSerializer.Serialize(writer, dictionary, options);
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