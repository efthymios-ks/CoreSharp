using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CoreSharp.Json.TextJson.JsonConverters;

public sealed class CultureInfoJsonConveter : JsonConverter<CultureInfo>
{
    // Methods
    public override CultureInfo Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => reader.TokenType switch
        {
            JsonTokenType.Number => CultureInfo.GetCultureInfo(reader.GetInt32()),
            JsonTokenType.String => CultureInfo.GetCultureInfo(reader.GetString()),
            _ => null
        };

    public override void Write(Utf8JsonWriter writer, CultureInfo value, JsonSerializerOptions options)
        => writer.WriteStringValue(value?.Name);
}