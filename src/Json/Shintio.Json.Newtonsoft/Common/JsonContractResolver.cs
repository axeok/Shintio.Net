using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using global::Newtonsoft.Json;
using global::Newtonsoft.Json.Serialization;
using Shintio.Json.Common;
using Shintio.Json.Interfaces;
using JsonConstructorAttribute = Shintio.Json.Attributes.JsonConstructorAttribute;
using JsonIgnoreAttribute = Shintio.Json.Attributes.JsonIgnoreAttribute;
using JsonObjectAttribute = Shintio.Json.Attributes.JsonObjectAttribute;
using MemberSerialization = Shintio.Json.Enums.MemberSerialization;

namespace Shintio.Json.Newtonsoft.Common
{
	public class JsonContractResolver : DefaultContractResolver
	{
		private readonly object _lock = 0;
		private readonly IJson _json;

		public JsonContractResolver(IJson json)
		{
			_json = json;
		}

		protected override List<MemberInfo> GetSerializableMembers(Type objectType)
		{
			// TODO: учесть все наши атрибуты
			return base.GetSerializableMembers(objectType);
		}

		protected override JsonObjectContract CreateObjectContract(Type objectType)
		{
			var contract = base.CreateObjectContract(objectType);

			ApplyIgnore(contract, objectType);

			if (JsonTypesProcessor<JsonConverter>.TryProcessType(_json, objectType, typeof(NewtonsoftJsonConverter<>))
			    is JsonConverter converter)
			{
				contract.Converter = converter;
			}
			else
			{
				var constructorInfo = objectType.GetConstructors()
					.FirstOrDefault(c => c.GetCustomAttribute<JsonConstructorAttribute>() != null);
				if (constructorInfo != null)
				{
					contract.OverrideCreator = CreateParameterizedConstructor(constructorInfo);
					
					// fix for RAGE MP
					// contract.CreatorParameters.Clear();
					var creatorParametersProperty = contract.GetType().GetProperty("CreatorParameters")!;
					creatorParametersProperty.PropertyType.GetMethod("Clear")!
						.Invoke(creatorParametersProperty.GetValue(contract), new object[] { });

					foreach (var property in CreateConstructorParameters(constructorInfo, contract.Properties))
					{
						// fix for RAGE MP
						// contract.CreatorParameters.Add(property);
						creatorParametersProperty.PropertyType.GetMethod("Add")!
							.Invoke(creatorParametersProperty.GetValue(contract), new object[] { property });
					}
				}
			}

			return contract;
		}

		private void ApplyIgnore(JsonObjectContract contract, Type type)
		{
			var jsonObjectAttribute = type.GetCustomAttribute<JsonObjectAttribute>();
			var memberSerialization = jsonObjectAttribute?.MemberSerialization ?? MemberSerialization.OptOut;
			
			foreach (var propertyInfo in type.GetProperties())
			{
				// fix for RAGE MP
				// var property = contract.Properties.FirstOrDefault(p => p.UnderlyingName == propertyInfo.Name);
				var properties =
					(IEnumerable<JsonProperty>)contract.GetType().GetProperty("Properties")!.GetValue(contract);
				var property = properties.FirstOrDefault(p => p.UnderlyingName == propertyInfo.Name);
				if (property == null)
				{
					continue;
				}
				
				property.Ignored = IsIgnored(propertyInfo, memberSerialization);
			}
		}

		private bool IsIgnored(MemberInfo memberInfo, MemberSerialization memberSerialization)
		{
			switch (memberSerialization)
			{
				case MemberSerialization.OptOut:
					return memberInfo.GetCustomAttribute<JsonIgnoreAttribute>() != null;
				case MemberSerialization.OptIn:
					return memberInfo.GetCustomAttribute<JsonPropertyAttribute>() == null;
				case MemberSerialization.Fields:
					// TODO
					break;
				default:
					return false;
			}

			return false;
		}

		private ObjectConstructor<object> CreateParameterizedConstructor(MethodBase method)
		{
			if (method is ConstructorInfo c)
			{
				return a => c.Invoke(a);
			}

			return a => method.Invoke(null, a)!;
		}
	}
}