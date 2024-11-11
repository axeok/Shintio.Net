using System;
using System.Collections;
using System.Reflection;
using System.Text.Json;

namespace Shintio.Json.System.Extensions;

public static class JsonWriterExtensions
{
	// TODO: протестить
	public static void WriteObject(this Utf8JsonWriter writer, object? obj)
	{
		if (obj == null)
		{
			writer.WriteNullValue();
			return;
		}

		Type type = obj.GetType();
		writer.WriteStartObject();

		foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
		{
			var propertyValue = property.GetValue(obj);
			if (propertyValue == null)
			{
				writer.WriteNull(property.Name);
				continue;
			}

			if (propertyValue is string strValue)
			{
				writer.WriteString(property.Name, strValue);
			}
			else if (propertyValue is int intValue)
			{
				writer.WriteNumber(property.Name, intValue);
			}
			else if (propertyValue is bool boolValue)
			{
				writer.WriteBoolean(property.Name, boolValue);
			}
			else if (propertyValue is IEnumerable enumerable)
			{
				writer.WriteStartArray(property.Name);
				foreach (var item in enumerable)
				{
					WriteObject(writer, item);
				}

				writer.WriteEndArray();
			}
			else if (propertyValue.GetType().IsClass)
			{
				writer.WritePropertyName(property.Name);
				WriteObject(writer, propertyValue);
			}
			else
			{
				// Fallback for other types, e.g., DateTime, double, etc.
				writer.WritePropertyName(property.Name);
				JsonSerializer.Serialize(writer, propertyValue);
			}
		}

		writer.WriteEndObject();
	}
}