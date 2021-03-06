using CoreSharp.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoreSharp.Json.JsonNet.ContractResolvers;

public class PrimitiveOnlyResolver : WritableOnlyPropertiesResolver
{
    //Fields
    private static PrimitiveOnlyResolver _instance;

    //Properties
    public static new PrimitiveOnlyResolver Instance
        => _instance ??= new PrimitiveOnlyResolver();

    //Methods
    protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        => base.CreateProperties(type, memberSerialization)
               .Where(j => j.PropertyType.IsPrimitiveExtended())
               .ToList();
}
