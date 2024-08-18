using Newtonsoft.Json.Linq;
using Shintio.Json.Newtonsoft.Nodes;
using Shintio.Json.Nodes;

namespace Shintio.Json.Newtonsoft.Extensions
{
	public static class JsonNodeExtensions
	{
		public static TNode? GetNode<TNode>(this IJsonNode node) where TNode : JToken
		{
			return node is NewtonsoftJsonNode<TNode> systemNode ? systemNode.Node : null;
		}
	}
}