using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Shintio.Json.Interfaces;

namespace Shintio.Json.Newtonsoft.Common
{
	public class NewtonsoftJsonReader : IJsonReader
	{
		private readonly JsonReader _reader;

		public NewtonsoftJsonReader(JsonReader reader)
		{
			_reader = reader;
		}

		public string? GetFullJson()
		{
			return JToken.ReadFrom(_reader).ToString();
		}
	}
}