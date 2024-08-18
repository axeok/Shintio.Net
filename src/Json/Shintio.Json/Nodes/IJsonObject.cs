using System.Collections.Generic;

namespace Shintio.Json.Nodes
{
	public interface IJsonObject : IJsonNode, IEnumerable<KeyValuePair<string, IJsonNode?>>
	{
	}
}