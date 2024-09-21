using System;

namespace Shintio.Json.Attributes
{
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
	public class JsonPropertyAttribute : Attribute
	{
		public JsonPropertyAttribute(string? name = null)
		{
			Name = name;
		}

		public string? Name { get; set; }
	}
}