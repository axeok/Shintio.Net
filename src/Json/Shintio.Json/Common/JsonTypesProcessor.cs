using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using Shintio.Json.Attributes;
using Shintio.Json.Interfaces;

// We need cache and lock for each serializator type, not one global (multithreading issue when multiple serializators try to process types)
// ReSharper disable StaticMemberInGenericType

namespace Shintio.Json.Common
{
	public static class JsonTypesProcessor<TLibraryConverter>
	{
		private static readonly Type ConverterAttributeType = typeof(JsonConverterAttribute);
		private static readonly Type ShintioConverterClassType = typeof(JsonConverter<>);

		// TODO: axe json - ConcurrentDictionary not available in RAGE client
		private static readonly Dictionary<Type, object?> TypesCache =
			new Dictionary<Type, object?>();

		private static readonly ReaderWriterLockSlim Lock = new ReaderWriterLockSlim();

		public static object? TryProcessType(IJson json, Type type, Type genericConverterType)
		{
			if (TypesCache.TryGetValue(type, out var converter))
			{
				return converter;
			}

			Lock.EnterUpgradeableReadLock();

			try
			{
				converter = ProcessType(json, type, genericConverterType);
				TypesCache[type] = converter;

				return converter;
			}
			finally
			{
				Lock.ExitUpgradeableReadLock();
			}
		}

		private static object? ProcessType(IJson json, Type type, Type genericConverterType)
		{
			Lock.EnterWriteLock();

			try
			{
				if (TypesCache.TryGetValue(type, out var existingConverter))
				{
					return existingConverter;
				}

				if (type.GetCustomAttribute(ConverterAttributeType) is JsonConverterAttribute attribute)
				{
					var shintioConverterGenericArguments =
						FindBaseType(attribute.ConverterType, ShintioConverterClassType)?.GetGenericArguments();
					if (shintioConverterGenericArguments == null || shintioConverterGenericArguments.Length <= 0)
					{
						return null;
					}

					if (!attribute.Inheritance && type != shintioConverterGenericArguments[0])
					{
						return null;
					}

					var baseConverter = Activator.CreateInstance(attribute.ConverterType) as IJsonConverter;
					if (baseConverter == null)
					{
						return null;
					}

					baseConverter.Converter = json;

					var converterType =
						genericConverterType.MakeGenericType(shintioConverterGenericArguments[0]);
					if (Activator.CreateInstance(converterType, baseConverter) is TLibraryConverter converter)
					{
						return converter;
					}
				}
			}
			finally
			{
				Lock.ExitWriteLock();
			}

			return null;
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

				targetType = targetType.GetType().GetProperty("BaseType").GetValue(targetType) as Type;
			}

			return null;
		}
	}
}