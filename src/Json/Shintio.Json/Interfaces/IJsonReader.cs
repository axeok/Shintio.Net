using Shintio.Json.Nodes;

namespace Shintio.Json.Interfaces
{
	public interface IJsonReader
	{
		public string? GetFullJson();
		public string? GetString();
		
		public IJsonObject? GetObject();
	}
}