using CoreSharp.Json.JsonNet.ContractResolvers;
using Newtonsoft.Json;
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
    public static JsonSerializerSettings Default
        => _default ??= CreateDefault();

    public static JsonSerializerSettings PrimitiveOnly
        => _primitiveOnly ??= CreatePrimitiveOnly();

    public static JsonSerializerSettings Strict
        => _strict ??= CreateStrict();

    // Methods
    private static JsonSerializerSettings CreateDefault()
        => new()
        {
            Formatting = Formatting.Indented,
            NullValueHandling = NullValueHandling.Include,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            MaxDepth = 8,
            ContractResolver = WritableOnlyPropertiesResolver.Instance
        };

    private static JsonSerializerSettings CreatePrimitiveOnly()
    {
        var settings = CreateDefault();
        settings.ContractResolver = PrimitiveOnlyResolver.Instance;
        return settings;
    }

    private static JsonSerializerSettings CreateStrict()
    {
        var settings = CreateDefault();
        settings.MissingMemberHandling = MissingMemberHandling.Error;
        return settings;
    }
}
