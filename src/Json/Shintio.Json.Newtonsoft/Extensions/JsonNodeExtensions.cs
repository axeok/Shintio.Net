using global::Newtonsoft.Json.Linq;
using Shintio.Json.Newtonsoft.Nodes;
using Shintio.Json.Nodes;

namespace Shintio.Json.Newtonsoft.Extensions
{
	public static class JsonNodeExtensions
	{
#if NETCOREAPP3_0_OR_GREATER
		public static TNode? GetNode<TNode>(this IJsonNode node) where TNode : JToken
#else
		public static TNode GetNode<TNode>(this IJsonNode node) where TNode : JToken
#endif
		{
			return node is NewtonsoftJsonNode<TNode> systemNode ? systemNode.Node : null;
		}
	}
}