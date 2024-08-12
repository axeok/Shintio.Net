using System;
using Newtonsoft.Json;

namespace Shintio.Json.Newtonsoft
{
    public class Test
    {
    }

    public class TestConverter : JsonConverter<Test>
    {
        public override void WriteJson(JsonWriter writer, Test? value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override Test? ReadJson(
            JsonReader reader,
            Type objectType,
            Test? existingValue,
            bool hasExistingValue,
            JsonSerializer serializer
        )
        {
            throw new NotImplementedException();
        }
    }
}