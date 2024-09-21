using System.Reflection;
using Shintio.CodeGenerator.Utils;
using Shintio.Essentials.Extensions.ReflectionExtensions;

namespace Shintio.CodeGenerator.Extensions
{
	public static class ReflectionExtensions
	{
		public static string GetTypeString(this Type type, bool withoutVariable = false)
		{
			if (type == typeof(void))
			{
				if (withoutVariable)
				{
					return "typeof(void)";
				}

				return "void";
			}

			if (type.IsGenericType)
			{
				var returnType = ReflectionHelper.GetGenericTypeString(type, true);

				if (withoutVariable)
				{
					return $"typeof({returnType})";
				}

				return returnType;
			}

			if (type.IsByRef)
			{
				if (withoutVariable)
				{
					return $"typeof({type.Namespace}.{type.GetElementType()!.Name}).MakeByRefType()";
				}

				return $"ref {type.Namespace}.{type.GetElementType()!.Name}";
			}

			if (type.IsPointer)
			{
				if (withoutVariable)
				{
					return $"typeof({type.Namespace}.{type.GetElementType()!.Name}).MakeByPointerType()";
				}

				return $"{type.Namespace}.{type.GetElementType()!.Name}*";
			}

			if (withoutVariable)
			{
				return $"typeof({type.Namespace}.{type.Name})";
			}

			return $"{type.Namespace}.{type.Name}";
		}

		public static string GetParameterString(this ParameterInfo parameter, Type? type = null, string? name = null)
		{
			type ??= parameter.ParameterType;
			name ??= parameter.Name;

			return
				$"{type.GetTypeString()}{(parameter.IsNullable() ? "?" : "")} {name}{(parameter.HasDefaultValue ? $" = {ReflectionHelper.GetValueString(parameter.DefaultValue)}" : "")}";
		}

		public static string GetMethodString(this MethodInfo method, Type? type = null)
		{
			type ??= method.ReturnType;

			return
				$"{type.GetTypeString()} {method.Name}({string.Join(", ", method.GetParameters().Select(p => p.GetParameterString()))});";
		}
	}
}