#if NET7_0_OR_GREATER

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using Shintio.Essentials.Utils;
using JsonConstructorAttribute = Shintio.Json.Attributes.JsonConstructorAttribute;
using JsonIgnoreAttribute = Shintio.Json.Attributes.JsonIgnoreAttribute;

namespace Shintio.Json.System.Common;

public class JsonTypeInfoResolver : DefaultJsonTypeInfoResolver
{
	public const int UnboxedParameterCountThreshold = 4;

	private const string ObjectDefaultConverterType =
		"System.Text.Json.Serialization.Converters.ObjectDefaultConverter`1";

	private const string SmallObjectWithParameterizedConstructorConverterType =
		"System.Text.Json.Serialization.Converters.SmallObjectWithParameterizedConstructorConverter`5";

	private const string LargeObjectWithParameterizedConstructorConverterWithReflectionType =
		"System.Text.Json.Serialization.Converters.LargeObjectWithParameterizedConstructorConverterWithReflection`1";

	private static readonly Type ObjectType = typeof(object);

	private static readonly Assembly Assembly =
		Assembly.GetAssembly(typeof(global::System.Text.Json.Serialization.JsonConstructorAttribute))!;

	public override JsonTypeInfo GetTypeInfo(Type type, JsonSerializerOptions options)
	{
		var typeInfo = CreateJsonTypeInfo(type, options);
		typeInfo.OriginatingResolver = this;

		typeInfo.GetType().GetProperty("IsCustomized", ReflectionHelper.PrivateFlags)?.SetValue(typeInfo, false);

		var modifiers = GetType().BaseType
			!.GetField("_modifiers", ReflectionHelper.PrivateFlags)
			?.GetValue(this) as IEnumerable<Action<JsonTypeInfo>>;
		if (modifiers != null)
		{
			foreach (Action<JsonTypeInfo> modifier in modifiers)
			{
				modifier(typeInfo);
			}
		}

		foreach (var propertyInfo in type.GetProperties()
			         .Where(p => p.GetCustomAttribute<JsonIgnoreAttribute>() != null).ToArray())
		{
			var property = typeInfo.Properties.FirstOrDefault(p => p.Name == propertyInfo.Name);
			if (property == null)
			{
				continue;
			}

			property.Get = null;
			property.Set = null;
		}

		return typeInfo;
	}

	private JsonTypeInfo CreateJsonTypeInfo(Type type, JsonSerializerOptions options)
	{
		JsonConverter converter;

		var constructorInfo = type.GetConstructors()
			.FirstOrDefault(c => c.GetCustomAttribute<JsonConstructorAttribute>() != null);
		if (constructorInfo != null)
		{
			converter = CreateConverter(type, constructorInfo, options);
		}
		else
		{
			converter = (JsonConverter)(
				GetType().BaseType!.GetMethod("GetConverterForType", ReflectionHelper.StaticFlags)
					!.Invoke(this, new object[] { type, options, true })
			)!;
		}

		return (JsonTypeInfo)(
			GetType().BaseType!.GetMethod("CreateTypeInfoCore", ReflectionHelper.StaticFlags)
				!.Invoke(this, new object[] { type, converter, options })
		)!;
	}

	private JsonConverter CreateConverter(
		Type typeToConvert,
		ConstructorInfo constructor,
		JsonSerializerOptions options
	)
	{
		JsonConverter converter;
		Type converterType;

		ParameterInfo[]? parameters = constructor?.GetParameters();

		if (constructor == null || typeToConvert.IsAbstract || parameters!.Length == 0)
		{
			converterType = Assembly.GetType(ObjectDefaultConverterType)!.MakeGenericType(typeToConvert);
		}
		else
		{
			int parameterCount = parameters.Length;

			if (parameterCount <= UnboxedParameterCountThreshold)
			{
				Type placeHolderType = ObjectType;
				Type[] typeArguments = new Type[UnboxedParameterCountThreshold + 1];

				typeArguments[0] = typeToConvert;
				for (int i = 0; i < UnboxedParameterCountThreshold; i++)
				{
					if (i < parameterCount)
					{
						typeArguments[i + 1] = parameters[i].ParameterType;
					}
					else
					{
						// Use placeholder arguments if there are less args than the threshold.
						typeArguments[i + 1] = placeHolderType;
					}
				}

				converterType = Assembly
					.GetType(SmallObjectWithParameterizedConstructorConverterType)
					!.MakeGenericType(typeArguments);
			}
			else
			{
				converterType =
					Assembly
						.GetType(LargeObjectWithParameterizedConstructorConverterWithReflectionType)
						!.MakeGenericType(typeToConvert);
			}
		}

		converter = (JsonConverter)Activator.CreateInstance(
			converterType,
			BindingFlags.Instance | BindingFlags.Public,
			binder: null,
			args: null,
			culture: null)!;

		var constructorInfo = converterType.GetProperty("ConstructorInfo", ReflectionHelper.PrivateFlags)!;
		constructorInfo.SetValue(converter, constructor);

		return converter;
	}
}

#endif