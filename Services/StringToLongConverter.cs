using System.Text.Json;
using System.Text.Json.Serialization;

namespace BitrateCalculation.Services;

public class StringToLongConverter : JsonConverter<long>
{
    public override long Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            return long.Parse(reader.GetString()!);
        }
        else
        {
            return reader.GetInt64();
        }
    }

    public override void Write(Utf8JsonWriter writer, long value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(value);
    }
}
