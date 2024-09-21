using System;
using Shintio.Json.Interfaces;

namespace Shintio.Json.Extensions
{
	public static class JsonExtensions
	{
		public static T DeserializeOrDefault<T>(this IJson converter, string json, T defaultValue)
		{
			return converter.DeserializeOrDefault(json, () => defaultValue);
		}

		public static T DeserializeOrDefault<T>(this IJson converter, string json, Func<T> getDefaultValue)
		{
			try
			{
				return converter.Deserialize<T>(json) ?? getDefaultValue();
			}
			catch
			{
				return getDefaultValue();
			}
		}
	}
}