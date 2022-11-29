using CoreSharp.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoreSharp.Json.JsonNet.ContractResolvers;

public class PrimitiveOnlyContractResolver : WritableOnlyContractResolver
{
    // Fields
    private static PrimitiveOnlyContractResolver _instance;

    // Properties
    public static new PrimitiveOnlyContractResolver Instance
        => _instance ??= new PrimitiveOnlyContractResolver();

    // Methods
    protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        => base.CreateProperties(type, memberSerialization)
               .Where(PrimitivePropertyFilter)
               .ToList();

    private static bool PrimitivePropertyFilter(JsonProperty jsonProperty)
        => jsonProperty.PropertyType.IsPrimitiveExtended();
}
