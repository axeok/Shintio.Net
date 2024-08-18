using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Shintio.Json.Nodes;

namespace Shintio.Json.Newtonsoft.Nodes
{
	public class NewtonsoftJsonObject : NewtonsoftJsonNode<JObject>, IJsonObject
	{
		public NewtonsoftJsonObject(JObject node) : base(node)
		{
		}

		public static explicit operator NewtonsoftJsonObject(JObject node)
		{
			return new NewtonsoftJsonObject(node);
		}

		public static explicit operator JObject(NewtonsoftJsonObject node)
		{
			return node.Node;
		}

		public IEnumerator<KeyValuePair<string, IJsonNode?>> GetEnumerator()
		{
			foreach (var (key, value) in Node)
			{
				yield return new KeyValuePair<string, IJsonNode?>(key, Create(value));
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}