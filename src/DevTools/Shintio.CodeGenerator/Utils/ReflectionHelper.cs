using Shintio.CodeGenerator.Extensions;

namespace Shintio.CodeGenerator.Utils;

public class ReflectionHelper
{
	public static string TrimGenericName(string name)
	{
		return name.Contains('`') ? name.Substring(0, name.IndexOf('`')) : name;
	}

	public static string GetGenericTypeString(Type type, bool withNamespace = false, string prefix = "")
	{
		string name;

		if (!type.IsGenericType)
		{
			name = $"{prefix}{type.Name}";

			return withNamespace ? $"{type.Namespace}.{name}" : type.Name;
		}

		name = $"{prefix}{type.GetGenericTypeDefinition().Name}";

		var genericTypeName = withNamespace
			? $"{type.GetGenericTypeDefinition().Namespace}.{name}"
			: name;

		genericTypeName = TrimGenericName(genericTypeName);

		return
			$"{genericTypeName}<{string.Join(",", type.GetGenericArguments().Select(t => GetGenericTypeString(t, withNamespace)))}>";
	}

	public static string GetValueString(object? value, bool isJs = false)
	{
		if (value == null)
		{
			return "null";
		}

		var type = value.GetType();

		if (type == typeof(string))
		{
			return $"\"{value}\"";
		}

		if (type == typeof(float))
		{
			return isJs
				? Convert.ToSingle(value).ToString().Replace(",", ".")
				: Formatter.AsFloat(Convert.ToSingle(value));
		}

		if (type == typeof(bool))
		{
			return Convert.ToBoolean(value) ? "true" : "false";
		}

		if (type.IsEnum)
		{
			return isJs ? $"\"{value}\"" : $"{type.GetTypeString()}.{value}";
		}

		if (type.IsPrimitive)
		{
			return value.ToString()!;
		}

		return "";
	}
}