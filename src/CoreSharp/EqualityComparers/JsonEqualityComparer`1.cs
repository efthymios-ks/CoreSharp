using CoreSharp.Json.JsonNet;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using JsonNet = Newtonsoft.Json;
using TextJson = System.Text.Json;

namespace CoreSharp.EqualityComparers;

/// <summary>
/// Defines methods to support the comparison of objects for equality.
/// Uses <see cref="JsonNet.JsonConvert.SerializeObject(object)"/>
/// for default conversions.
/// </summary>
public class JsonEqualityComparer<TEntity> : IEqualityComparer<TEntity>
{
    //Fields
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly Func<TEntity, string> _serializeFunction;

    //Constructors
    public JsonEqualityComparer()
        : this(JsonSettings.Default)
    {
    }

    public JsonEqualityComparer(JsonNet.JsonSerializerSettings settings)
        : this(entity => JsonNet.JsonConvert.SerializeObject(entity, settings))
        => _ = settings ?? throw new ArgumentNullException(nameof(settings));

    public JsonEqualityComparer(TextJson.JsonSerializerOptions options)
        : this(entity => TextJson.JsonSerializer.Serialize(entity, options))
        => _ = options ?? throw new ArgumentNullException(nameof(options));

    public JsonEqualityComparer(Func<TEntity, string> serializeFunction)
        => _serializeFunction = serializeFunction ?? throw new ArgumentNullException(nameof(serializeFunction));

    //Methods
    public bool Equals(TEntity x, TEntity y)
    {
        var leftJson = _serializeFunction(x);
        var rightJson = _serializeFunction(y);
        return leftJson == rightJson;
    }

    public int GetHashCode(TEntity obj)
        => _serializeFunction(obj).GetHashCode();
}
