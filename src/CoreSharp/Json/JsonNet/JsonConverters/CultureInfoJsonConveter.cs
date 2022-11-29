using Newtonsoft.Json;
using System;
using System.Globalization;

namespace CoreSharp.Json.JsonNet.JsonConverters;

public sealed class CultureInfoJsonConveter : JsonConverter<CultureInfo>
{
    // Methods
    public override CultureInfo ReadJson(JsonReader reader, Type objectType, CultureInfo existingValue, bool hasExistingValue, JsonSerializer serializer)
        => reader.Value switch
        {
            int cultureCode => CultureInfo.GetCultureInfo(cultureCode),
            string cultureName => CultureInfo.GetCultureInfo(cultureName),
            _ => null
        };

    public override void WriteJson(JsonWriter writer, CultureInfo value, JsonSerializer serializer)
        => writer.WriteValue(value?.Name);
}