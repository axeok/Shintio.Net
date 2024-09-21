using System.Text.Json.Nodes;
using Shintio.Json.Nodes;

namespace Shintio.Json.System.Nodes
{
	public class SystemJsonValue : SystemJsonNode<JsonValue>, IJsonValue
	{
		public SystemJsonValue(JsonValue node) : base(node)
		{
		}

		public static explicit operator SystemJsonValue(JsonValue node)
		{
			return new SystemJsonValue(node);
		}

		public static explicit operator JsonValue(SystemJsonValue node)
		{
			return node.Node;
		}
	}
}