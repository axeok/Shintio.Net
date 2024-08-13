using Newtonsoft.Json;
using Shintio.Json.Interfaces;

namespace Shintio.Json.Newtonsoft.Common
{
	public class NewtonsoftJsonWriter : IJsonWriter
	{
		private readonly JsonWriter _writer;

		public NewtonsoftJsonWriter(JsonWriter writer)
		{
			_writer = writer;
		}

		public void WriteValue(string value)
		{
			_writer.WriteValue(value);
		}
	}
}