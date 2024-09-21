using global::Newtonsoft.Json.Linq;
using Shintio.Json.Nodes;

namespace Shintio.Json.Newtonsoft.Nodes
{
	public class NewtonsoftJsonValue : NewtonsoftJsonNode<JValue>, IJsonValue
	{
		public NewtonsoftJsonValue(JValue node) : base(node)
		{
		}

		public static explicit operator NewtonsoftJsonValue(JValue node)
		{
			return new NewtonsoftJsonValue(node);
		}

		public static explicit operator JValue(NewtonsoftJsonValue node)
		{
			return node.Node;
		}
	}
}