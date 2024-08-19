using System;

namespace Shintio.Json.Nodes
{
	public interface IJsonNode
	{
		public IJsonNode? this[string propertyName] { get; set; }

		public object GetRealNode();
		
		public T ToObject<T>();
		public object? ToObject(Type type);
	}
}