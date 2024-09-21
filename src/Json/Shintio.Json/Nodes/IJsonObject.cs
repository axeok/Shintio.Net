using System.Collections.Generic;

namespace Shintio.Json.Nodes
{
	public interface IJsonObject : IJsonNode, IEnumerable<KeyValuePair<string, IJsonNode?>>
	{
		public bool ContainsKey(string key);
		public bool Remove(string key);
	}
}