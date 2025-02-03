using System;
using Shintio.Json.Enums;
using Shintio.Json.Nodes;

namespace Shintio.Json.Interfaces
{
	public interface IJson
	{
		public Type SerializerType { get; }
		
		public string Serialize(object? value, JsonFormatting formatting = JsonFormatting.None);

#if NETCOREAPP3_0_OR_GREATER
        public T? Deserialize<T>(string json);
#else
		public T Deserialize<T>(string json);
#endif
		
		public object? Deserialize(string json, Type type);

		public IJsonNode? ParseNode(string json);
		
		public IJsonArray CreateArray();
		public IJsonObject CreateObject();

		public IJsonArray CreateArray(object value);
		public IJsonObject CreateObject(object value);
		public IJsonNode CreateNode(object value);
	}
}