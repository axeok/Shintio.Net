using System.Collections;
using System.Reflection;
using System.Text.Json;

namespace Shintio.CodeGenerator.Utils.DefaultValueProviders;

public class JavaScriptDefaultValueProvider : BaseDefaultValueProvider
{
	public override string Get(Type type, bool isNullable)
	{
		if (isNullable)
		{
			return "null";
		}

		if (type == typeof(bool))
		{
			return "false";
		}

		if (type.IsPrimitive || type.IsEnum)
		{
			return "0";
		}

		if (
			type != typeof(string) && type.Name != "JObject" && type.GetInterface(nameof(IDictionary)) == null &&
			type.GetInterface(nameof(IEnumerable)) != null)
		{
			return "[]";
		}

		// if (type.IsSubclassOf(typeof(DataCollection)))
		if (type.BaseType?.Name == "DataCollection")
		{
			return "\"\"";
		}

		const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
		if (!type.IsValueType && type.GetConstructor(flags, Type.EmptyTypes) != null)
		{
			return JsonSerializer.Serialize(Activator.CreateInstance(type, true));
		}

		return "\"\"";
	}
}