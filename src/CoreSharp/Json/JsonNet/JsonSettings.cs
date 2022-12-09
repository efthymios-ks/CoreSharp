using CoreSharp.Json.JsonNet.ContractResolvers;
using CoreSharp.Json.JsonNet.JsonConverters;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;

namespace CoreSharp.Json.JsonNet;

public static class JsonSettings
{
    // Fields
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private static JsonSerializerSettings _default;
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private static JsonSerializerSettings _primitiveOnly;
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private static JsonSerializerSettings _strict;

    // Properties
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private static IEnumerable<JsonConverter> DefaultJsonConverters
        => new JsonConverter[]
        {
            new CultureInfoJsonConveter(),
            new UtcDateTimeJsonConverter()
        };

    public static JsonSerializerSettings Default
        => _default ??= CreateDefault();

    public static JsonSerializerSettings PrimitiveOnly
        => _primitiveOnly ??= CreatePrimitiveOnly();

    public static JsonSerializerSettings Strict
        => _strict ??= CreateStrict();

    // Methods
    private static JsonSerializerSettings CreateDefault()
    {
        var settings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            NullValueHandling = NullValueHandling.Include,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            MaxDepth = 8,
            ContractResolver = WritableOnlyContractResolver.Instance
        };

        foreach (var jsonConverter in DefaultJsonConverters)
            settings.Converters.Add(jsonConverter);

        return settings;
    }
    private static JsonSerializerSettings CreatePrimitiveOnly()
    {
        var settings = CreateDefault();
        settings.ContractResolver = PrimitiveOnlyContractResolver.Instance;
        return settings;
    }

    private static JsonSerializerSettings CreateStrict()
    {
        var settings = CreateDefault();
        settings.MissingMemberHandling = MissingMemberHandling.Error;
        return settings;
    }
}
