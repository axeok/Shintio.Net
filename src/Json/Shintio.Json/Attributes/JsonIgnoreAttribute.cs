using System;

namespace Shintio.Json.Attributes
{
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
	public class JsonIgnoreAttribute : Attribute
	{
	}
}