using System.Reflection;
using System.Text.Json;
using Shintio.CodeGenerator.Extensions;
using Shintio.Essentials.Extensions.ReflectionExtensions;

namespace Shintio.CodeGenerator.Utils;

public static class GeneratorHelper
{
	public static string FormatPropertyValue(PropertyInfo property, object? value)
	{
		return FormatPropertyValue(property.PropertyType, value, property.IsNullable());
	}

	public static string FormatPropertyValue(Type property, object? value, bool isNullable)
	{
		if (value == null || string.IsNullOrEmpty(value.ToString()))
		{
			return "null";
		}

		if (property == typeof(string))
		{
			return $"\"{value}\"";
		}

		if (property == typeof(float))
		{
			return Formatter.AsFloat(value.ToString()!);
		}

		if (property == typeof(bool))
		{
			return Convert.ToBoolean(value).ToString().ToLower();
		}

		if (property == typeof(TimeSpan))
		{
			return $"{typeof(TimeSpan).FullName}.{nameof(TimeSpan.FromSeconds)}({value})";
		}

		if (property.IsPrimitive)
		{
			return value.ToString()!;
		}

		if (property.IsEnum)
		{
			value = $"\\\"{value}\\\"";
		}

		var deserializeMethod = $"Newtonsoft.Json.JsonConvert.DeserializeObject";

		return $"{deserializeMethod}<{property.GetTypeString()}>(\"{value}\"){(isNullable ? "" : "!")}";
	}
}