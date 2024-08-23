using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Shintio.Json.System.Converters;

public class ReadOnlyCollectionJsonConverter : JsonConverterFactory
{
	public override bool CanConvert(Type typeToConvert)
	{
		if (typeToConvert.IsArray)
			return false;

		if (!typeToConvert.GetInterfaces()
			    .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IReadOnlyCollection<>)))
			return false;

		if (
			(typeToConvert.IsGenericType && typeToConvert.GetGenericTypeDefinition() != typeof(ReadOnlyCollection<>)) &&
			FindBaseType(typeToConvert, typeof(ReadOnlyCollection<>)) == null
		)
		{
			return false;
		}

		return true;
	}

	public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
	{
		Type keyType = typeToConvert.GetGenericArguments()[0];

		JsonConverter converter = (JsonConverter)Activator.CreateInstance(
			typeof(IReadOnlyCollectionConverterInner<>).MakeGenericType(keyType),
			BindingFlags.Instance | BindingFlags.Public,
			binder: null, args: null, culture: null);

		return converter;
	}

	private class IReadOnlyCollectionConverterInner<TValue> :
		JsonConverter<IReadOnlyCollection<TValue>>
		where TValue : notnull
	{
		public override IReadOnlyCollection<TValue> Read(
			ref Utf8JsonReader reader,
			Type typeToConvert,
			JsonSerializerOptions options
		)
		{
			var array = JsonSerializer.Deserialize<TValue[]>(ref reader, options: options);

			return (IReadOnlyCollection<TValue>)Activator.CreateInstance(
				typeToConvert,
				BindingFlags.Instance | BindingFlags.Public,
				binder: null,
#pragma warning disable CS8601 // Possible null reference assignment.
				args: new object[] { array },
#pragma warning restore CS8601 // Possible null reference assignment.
				culture: null);
		}

		public override void Write(
			Utf8JsonWriter writer,
			IReadOnlyCollection<TValue> dictionary,
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