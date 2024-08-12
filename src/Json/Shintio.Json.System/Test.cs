using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Shintio.Json.System;

public class Test
{
}

public class TestConverter : JsonConverter<Test>
{
    public override Test? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }

    public override void Write(Utf8JsonWriter writer, Test value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}