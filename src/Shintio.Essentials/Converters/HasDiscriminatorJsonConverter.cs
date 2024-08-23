using System;
using System.Collections.Generic;
using System.Linq;
using Shintio.Essentials.Interfaces;
using Shintio.Json.Common;
using Shintio.Json.Interfaces;

namespace Shintio.Essentials.Converters
{
	public static class HasDiscriminatorJsonConverter
	{
		public static Dictionary<Type, Dictionary<string, Type>> TypesMap =
			new Dictionary<Type, Dictionary<string, Type>>();
	}

	public class HasDiscriminatorJsonConverter<T> : JsonConverter<T>
	{
		public override void Write(IJsonWriter writer, T value)
		{
			// writer.WriteValue(value);
		}

#if NETCOREAPP3_0_OR_GREATER
		public override T? Read(IJsonReader reader, Type type)
#else
		public override T Read(IJsonReader reader, Type type)
#endif
		{
			var json = reader.GetFullJson();
			if (json == null)
			{
				return default;
			}

			var jsonObject = Converter.ParseNode(json);
			if (jsonObject == null)
			{
				return default;
			}

			var discriminator = jsonObject[nameof(IHasDiscriminator.Discriminator)]?.ToString();
			if (discriminator == null)
			{
				return default;
			}

			var concreteType = GetConcreteType(type, discriminator);
			if (concreteType == null)
			{
				return default;
			}

#if NETCOREAPP3_0_OR_GREATER
			return (T?)jsonObject.ToObject(concreteType);
#else
			return (T)jsonObject.ToObject(concreteType);
#endif
		}

		protected static Type? GetConcreteType(Type parent, string discriminator)
		{
			if (!HasDiscriminatorJsonConverter.TypesMap.ContainsKey(parent))
			{
				HasDiscriminatorJsonConverter.TypesMap.Add(parent, new Dictionary<string, Type>());
			}

			// TODO: axe json
			// На клиенте RAGE MP нет доступа к сборкам(Assembly), поэтому типы нельзя получить автоматически и нужно задавать вручную
			// Для этого есть генерато HasDiscriminatorTypesGenerator
#if !DEBUG
			if (!HasDiscriminatorJsonConverter.TypesMap[parent].ContainsKey(discriminator))
			{
				HasDiscriminatorJsonConverter.TypesMap[parent].Add(
					discriminator,
					parent.Assembly.GetTypes()
						.FirstOrDefault(t => parent.IsAssignableFrom(t) && t.Name == discriminator)
				);
			}
#endif

			HasDiscriminatorJsonConverter.TypesMap[parent].TryGetValue(discriminator, out var type);

			return type;
		}
	}
}