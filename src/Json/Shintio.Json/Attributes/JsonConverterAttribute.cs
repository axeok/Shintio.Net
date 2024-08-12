using System;

namespace Shintio.Json.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class JsonConverterAttribute : Attribute
    {
        public JsonConverterAttribute(Type converterType)
        {
            ConverterType = converterType;
        }

        public Type ConverterType { get; }
    }
}