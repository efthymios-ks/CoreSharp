using CoreSharp.Json.TextJson.JsonConverters;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CoreSharp.Json.TextJson.JsonConverterFactories;

public sealed class EnumJsonConverterFactory : JsonConverterFactory
{
    // Methods
    public override bool CanConvert(Type typeToConvert)
        => typeToConvert.IsEnum;

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var enumJsonConverterType = typeof(EnumJsonConverter<>).MakeGenericType(typeToConvert);
        return (JsonConverter)Activator.CreateInstance(enumJsonConverterType, options);
    }
}