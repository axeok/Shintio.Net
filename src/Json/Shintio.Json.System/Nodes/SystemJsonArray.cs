using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Nodes;
using Shintio.Json.Nodes;

namespace Shintio.Json.System.Nodes
{
	public class SystemJsonArray : SystemJsonNode<JsonArray>, IJsonArray
	{
		public SystemJsonArray(JsonArray node) : base(node)
		{
		}

		public int Count => Node.Count;
		public bool IsReadOnly => false;

		public static explicit operator SystemJsonArray(JsonArray node)
		{
			return new SystemJsonArray(node);
		}

		public static explicit operator JsonArray(SystemJsonArray node)
		{
			return node.Node;
		}

		public IEnumerator<IJsonNode?> GetEnumerator()
		{
			return Node.Select(Create).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public void Add(IJsonNode? item)
		{
			Node.Add(item?.GetRealNode());
		}

		public void Clear()
		{
			Node.Clear();
		}

		public bool Contains(IJsonNode? item)
		{
			var node = item?.GetRealNode();

			return node is JsonNode jsonNode && Node.Contains(jsonNode);
		}

		public void CopyTo(IJsonNode?[] array, int arrayIndex)
		{
			if (Node is ICollection<JsonNode> collection)
			{
				collection.Select(Create).ToArray().CopyTo(array, arrayIndex);
			}
		}

		public bool Remove(IJsonNode? item)
		{
			var node = item?.GetRealNode();

			return node is JsonNode jsonNode && Node.Remove(jsonNode);
		}

		public int IndexOf(IJsonNode? item)
		{
			var node = item?.GetRealNode();

			return node is JsonNode jsonNode ? Node.IndexOf(jsonNode) : -1;
		}

		public void Insert(int index, IJsonNode? item)
		{
			var node = item?.GetRealNode();

			if (node is JsonNode jsonNode)
			{
				Node.Insert(index, jsonNode);
			}
		}

		public void RemoveAt(int index)
		{
			Node.RemoveAt(index);
		}

		public IJsonNode? this[int index]
		{
			get => Create(Node.ElementAtOrDefault(index));
			set => Node[index] = value?.GetRealNode() as JsonNode;
		}

		public void Add(object? value)
		{
			if (value is IJsonNode jsonNode)
			{
				Node.Add(jsonNode?.GetRealNode());
			}
			else
			{
				Node.Add(value);
			}
		}
	}
}