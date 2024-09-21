using System.Collections.Generic;

namespace Shintio.Json.Nodes
{
	public interface IJsonArray : IJsonNode, IList<IJsonNode?>
	{
		public void Add(object? value);
	}
}