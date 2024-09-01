using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json.Nodes;
using Shintio.Json.Nodes;

namespace Shintio.Json.System.Nodes
{
	public class SystemJsonObject : SystemJsonNode<JsonObject>, IJsonObject
	{
		public SystemJsonObject(JsonObject node) : base(node)
		{
		}

		public static explicit operator SystemJsonObject(JsonObject node)
		{
			return new SystemJsonObject(node);
		}

		public static explicit operator JsonObject(SystemJsonObject node)
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

		public bool ContainsKey(string key)
		{
			return Node.ContainsKey(key);
		}

		public bool Remove(string key)
		{
			return Node.Remove(key);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}