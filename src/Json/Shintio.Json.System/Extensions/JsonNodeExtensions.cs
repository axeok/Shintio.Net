using System.Text.Json.Nodes;
using Shintio.Json.Nodes;
using Shintio.Json.System.Nodes;

namespace Shintio.Json.System.Extensions;

public static class JsonNodeExtensions
{
	public static TNode? GetNode<TNode>(this IJsonNode node) where TNode : JsonNode
	{
		return node is SystemJsonNode<TNode> systemNode ? systemNode.Node : null;
	}
}