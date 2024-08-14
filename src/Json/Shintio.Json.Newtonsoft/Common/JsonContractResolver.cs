﻿using System;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json.Serialization;
using Shintio.Json.Attributes;

namespace Shintio.Json.Newtonsoft.Common
{
	public class JsonContractResolver : DefaultContractResolver
	{
		protected override JsonObjectContract CreateObjectContract(Type objectType)
		{
			var contract = base.CreateObjectContract(objectType);

			foreach (var propertyInfo in objectType.GetProperties()
				         .Where(p => p.GetCustomAttribute<JsonIgnoreAttribute>() != null).ToArray())
			{
				var property = contract.Properties.FirstOrDefault(p => p.UnderlyingName == propertyInfo.Name);
				if (property == null)
				{
					continue;
				}

				property.Ignored = true;
			}

			var constructorInfo = objectType.GetConstructors()
				.FirstOrDefault(c => c.GetCustomAttribute<JsonConstructorAttribute>() != null);
			if (constructorInfo != null)
			{
				contract.OverrideCreator = CreateParameterizedConstructor(constructorInfo);
				contract.CreatorParameters.Clear();

				foreach (var property in CreateConstructorParameters(constructorInfo, contract.Properties))
				{
					contract.CreatorParameters.Add(property);
				}
			}

			return contract;
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