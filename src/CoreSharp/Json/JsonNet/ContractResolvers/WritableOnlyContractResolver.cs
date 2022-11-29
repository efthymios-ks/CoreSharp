using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoreSharp.Json.JsonNet.ContractResolvers;

public class WritableOnlyContractResolver : DefaultContractResolver
{
    // Fields
    private static WritableOnlyContractResolver _instance;

    // Properties
    public static WritableOnlyContractResolver Instance
        => _instance ??= new WritableOnlyContractResolver();

    // Methods
    protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        => base.CreateProperties(type, memberSerialization)
               .Where(WritablePropertyFilter)
               .ToList();

    private static bool WritablePropertyFilter(JsonProperty jsonProperty)
        => jsonProperty.Writable;
}
