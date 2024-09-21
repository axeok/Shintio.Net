using System;
using System.Text.Json;
using System.Text.Json.Nodes;
using Shintio.Json.Nodes;
using BaseNode = global::System.Text.Json.Nodes.JsonNode;

namespace Shintio.Json.System.Nodes
{
	public abstract class SystemJsonNode<TNode> : SystemJsonNode, IJsonNode where TNode : BaseNode
	{
		public SystemJsonNode(TNode node)
		{
			Node = node;
		}

		public TNode Node { get; }

		// public abstract IJsonNode? this[string propertyName] { get; set; }

		public IJsonNode? this[string propertyName]
		{
			get => Create(Node[propertyName]);
			set => Node[propertyName] = value?.GetRealNode() as BaseNode;
		}

		public string Path => Node.GetPath();

		public object GetRealNode()
		{
			return Node;
		}

		public T ToObject<T>()
		{
			return Node.Deserialize<T>();
		}

		public object? ToObject(Type type)
		{
			return Node.Deserialize(type);
		}

		public bool DeepEquals(IJsonNode? other)
		{
			if (other is not SystemJsonNode<TNode> casted)
			{
				return false;
			}

			// TODO: axe json
			return false;
		}

		public override string ToString()
		{
			return Node.ToString();
		}
	}

	public abstract class SystemJsonNode
	{
		public static IJsonNode? Create(BaseNode? node) => node switch
		{
			JsonArray n => new SystemJsonArray(n),
			JsonObject n => new SystemJsonObject(n),
			JsonValue n => new SystemJsonValue(n),
			_ => null,
		};
	}
}