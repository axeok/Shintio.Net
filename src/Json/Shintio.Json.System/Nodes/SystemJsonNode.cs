using System.Text.Json.Nodes;
using Shintio.Json.Nodes;
using BaseNode = global::System.Text.Json.Nodes.JsonNode;

namespace Shintio.Json.System.Nodes
{
	public abstract class SystemJsonNode<TNode> : IJsonNode where TNode : BaseNode
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
			set => Node[propertyName] = (value as SystemJsonNode<TNode>)?.Node;
		}

		public object GetRealNode()
		{
			return Node;
		}

		public static IJsonNode? Create(BaseNode? node) => node switch
		{
			JsonArray n => new SystemJsonArray(n),
			JsonObject n => new SystemJsonObject(n),
			JsonValue n => new SystemJsonValue(n),
			_ => null,
		};

		public override string ToString()
		{
			return Node.ToString();
		}
	}
}