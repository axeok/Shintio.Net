using System;
using System.Reflection;
using TypeInfo = Shintio.CodeBuilder.CSharp.Components.TypeInfo;

namespace Shintio.CodeBuilder.CSharp.Extensions;

public static class ReflectionExtensions
{
	public static TypeInfo AsTypeInfo(this Type type)
	{
		return type;
	}

	public static TypeInfo AsTypeInfo(this FieldInfo fieldInfo)
	{
		return fieldInfo;
	}

	public static TypeInfo AsTypeInfo(this PropertyInfo propertyInfo)
	{
		return propertyInfo;
	}

	public static TypeInfo AsTypeInfo(this ParameterInfo parameterInfo)
	{
		return parameterInfo;
	}
}