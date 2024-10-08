﻿#if NET7_0_OR_GREATER

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using Shintio.Essentials.Utils;
using Shintio.Json.Common;
using Shintio.Json.Interfaces;
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

	private const string LazyLoaderType =
		"Microsoft.EntityFrameworkCore.Infrastructure.ILazyLoader";

	private static readonly Type ObjectType = typeof(object);

	private static readonly Type SystemJsonIgnoreAttribute =
		typeof(global::System.Text.Json.Serialization.JsonIgnoreAttribute);

	private static readonly Assembly Assembly =
		Assembly.GetAssembly(typeof(global::System.Text.Json.Serialization.JsonConstructorAttribute))!;

	private readonly IJson _json;

	public JsonTypeInfoResolver(IJson json)
	{
		_json = json;
	}

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

		foreach (var propertyInfo in type.GetProperties(ReflectionHelper.PrivateFlags)
			         .Where(p =>
				         p.GetCustomAttribute<JsonIgnoreAttribute>() != null ||
				         p.GetCustomAttribute(SystemJsonIgnoreAttribute) != null ||
				         p.PropertyType.FullName == LazyLoaderType
			         ).ToArray())
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
		if (JsonTypesProcessor<JsonConverter>.TryProcessType(_json, type, typeof(SystemJsonConverter<>)) is not
		    JsonConverter converter)
		{
			var constructorInfo = type.GetConstructors(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
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
						// if (typeToConvert.FullName == "Shintio.Math.Common.Vector3")
						// {
						// 	var result = new StringBuilder();
						//
						// 	var type = typeToConvert.GetProperties()
						// 		.FirstOrDefault(p =>
						// 			p.Name.ToLowerInvariant().Contains(parameters[i].Name.ToLowerInvariant()))?
						// 		.PropertyType ?? typeToConvert.GetFields()
						// 		.FirstOrDefault(p =>
						// 			p.Name.ToLowerInvariant().Contains(parameters[i].Name.ToLowerInvariant()))?
						// 		.FieldType ?? parameters[i].ParameterType;
						//
						// 	result.AppendLine(constructor.ToString());
						// 	result.AppendLine(type.FullName);
						// 	result.AppendLine(parameters[i].ParameterType.FullName);
						// 	
						// 	throw new Exception(result.ToString());
						// }
						typeArguments[i + 1] = typeToConvert.GetProperties()
							.FirstOrDefault(p =>
								p.Name.ToLowerInvariant().Contains(parameters[i].Name.ToLowerInvariant()))?
							.PropertyType ?? typeToConvert.GetFields()
							.FirstOrDefault(p =>
								p.Name.ToLowerInvariant().Contains(parameters[i].Name.ToLowerInvariant()))?
							.FieldType ?? parameters[i].ParameterType;
						// typeArguments[i + 1] = parameters[i].ParameterType;
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