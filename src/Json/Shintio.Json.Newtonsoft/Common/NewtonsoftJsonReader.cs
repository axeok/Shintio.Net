using Newtonsoft.Json;
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

        public string? Read()
        {
            return _reader.Value?.ToString();
        }
    }
}