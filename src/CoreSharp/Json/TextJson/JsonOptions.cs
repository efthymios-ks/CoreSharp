using CoreSharp.Json.TextJson.JsonConverterFactories;
using CoreSharp.Json.TextJson.JsonConverters;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CoreSharp.Json.TextJson;

public static class JsonOptions
{
    // Fields 
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private static JsonSerializerOptions _default;

    // Properties
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private static IEnumerable<JsonConverter> DefaultJsonConverters
        => new JsonConverter[]
        {
            new TimeSpanJsonConverter(),
            new CultureInfoJsonConveter(),
            new EnumJsonConverterFactory()
        };

    public static JsonSerializerOptions Default
        => _default ??= CreateDefault();

    // Methods
    private static JsonSerializerOptions CreateDefault()
    {
        var options = new JsonSerializerOptions
        {
            AllowTrailingCommas = true,
            //DefaultBufferSize = 16 * 1024,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            IgnoreReadOnlyFields = true,
            IgnoreReadOnlyProperties = false,
            IncludeFields = false,
            MaxDepth = 8,
            NumberHandling = JsonNumberHandling.AllowReadingFromString,
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            ReadCommentHandling = JsonCommentHandling.Skip,
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            //TypeInfoResolver = null,
            UnknownTypeHandling = JsonUnknownTypeHandling.JsonElement,
            WriteIndented = true
        };

        foreach (var jsonConverter in DefaultJsonConverters)
        {
            options.Converters.Add(jsonConverter);
        }

        return options;
    }
}
