using System;

namespace Shintio.Json.Attributes
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
	public class JsonConverterAttribute : Attribute
	{
		public JsonConverterAttribute(Type converterType, bool inheritance = true)
		{
			ConverterType = converterType;
			Inheritance = inheritance;
		}

		public Type ConverterType { get; }
		public bool Inheritance { get; }
	}
}