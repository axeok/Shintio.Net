using System.Reflection;
using System.Reflection.Emit;
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

	public static IEnumerable<Type> GetChildrenTypes(Type type, bool allowAbstract = false)
	{
		return type.Assembly
			.GetTypes()
			.Where(child =>
				child.IsClass &&
				(allowAbstract || !child.IsAbstract) &&
				child.IsSubclassOf(type)
			) ?? Array.Empty<Type>();
	}

	public static IEnumerable<Type> GetChildrenInterfaces(Type type)
	{
		return type.Assembly
			.GetTypes()
			.Where(t => type.IsAssignableFrom(t) && t != type) ?? Array.Empty<Type>();
	}

	public static object? GetPropertyValue(PropertyInfo property)
	{
		try
		{
			var assemblyBuilder =
				AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("DynamicAssembly"),
					AssemblyBuilderAccess.Run);
			var moduleBuilder = assemblyBuilder.DefineDynamicModule("DynamicModule");
			var typeBuilder = moduleBuilder.DefineType("DynamicType", TypeAttributes.Public);

			typeBuilder.AddInterfaceImplementation(property.DeclaringType);

			foreach (var propertyInfo in new[] { property.DeclaringType }
				         .Concat(property.DeclaringType.GetInterfaces())
				         .SelectMany(i => i.GetProperties()))
			{
				var propertyBuilder = typeBuilder.DefineProperty(propertyInfo.Name,
					PropertyAttributes.None,
					propertyInfo.PropertyType,
					null);

				var setMethod = propertyInfo.GetSetMethod();
				if (setMethod != null)
				{
					var propertyMethod = typeBuilder.DefineMethod($"set_{propertyInfo.Name}",
						MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.SpecialName |
						MethodAttributes.Final | MethodAttributes.HideBySig | MethodAttributes.NewSlot,
						null,
						new[] { propertyInfo.PropertyType });

					var il = propertyMethod.GetILGenerator();
					il.Emit(OpCodes.Ldarg_0);
					il.Emit(OpCodes.Ldarg_1);
					il.Emit(OpCodes.Call, setMethod);
					il.Emit(OpCodes.Ret);
					propertyBuilder.SetSetMethod(propertyMethod);
				}
				else
				{
					var propertyMethod = typeBuilder.DefineMethod($"set_{propertyInfo.Name}",
						MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.SpecialName |
						MethodAttributes.Final | MethodAttributes.HideBySig | MethodAttributes.NewSlot,
						null,
						new[] { propertyInfo.PropertyType });

					var il = propertyMethod.GetILGenerator();
					il.Emit(OpCodes.Ret);
					propertyBuilder.SetSetMethod(propertyMethod);
				}

				var getMethod = propertyInfo.GetGetMethod();
				if (getMethod != null)
				{
					var propertyMethod = typeBuilder.DefineMethod($"get_{propertyInfo.Name}",
						MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.SpecialName |
						MethodAttributes.Final | MethodAttributes.HideBySig | MethodAttributes.NewSlot,
						propertyInfo.PropertyType,
						Type.EmptyTypes);

					var il = propertyMethod.GetILGenerator();
					il.Emit(OpCodes.Ldarg_0);
					il.Emit(OpCodes.Call, getMethod);
					il.Emit(OpCodes.Ret);
					propertyBuilder.SetGetMethod(propertyMethod);
				}
				else
				{
					var propertyMethod = typeBuilder.DefineMethod($"get_{propertyInfo.Name}",
						MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.SpecialName |
						MethodAttributes.Final | MethodAttributes.HideBySig | MethodAttributes.NewSlot,
						propertyInfo.PropertyType,
						Type.EmptyTypes);

					var il = propertyMethod.GetILGenerator();
					il.Emit(OpCodes.Ldnull);
					il.Emit(OpCodes.Ret);
					propertyBuilder.SetGetMethod(propertyMethod);
				}
			}

			var dynamicType = typeBuilder.CreateTypeInfo();

			var instance = Activator.CreateInstance(dynamicType);

			// Console.WriteLine(property.Name);
			// foreach (var propertyInfo in instance.GetType().GetProperties())
			// {
			// 	Console.WriteLine($"p {propertyInfo.Name}: {propertyInfo.GetValue(instance)}");
			// }

			return dynamicType.GetProperty(property.Name).GetValue(instance);
		}
		catch
		{
			return null;
		}
	}
}