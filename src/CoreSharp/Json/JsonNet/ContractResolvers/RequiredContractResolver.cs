using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CoreSharp.Json.JsonNet.ContractResolvers;

public class RequiredContractResolver : DefaultContractResolver
{
    // Fields
    private static RequiredContractResolver _instance;

    // Properties
    public static RequiredContractResolver Instance
        => _instance ??= new RequiredContractResolver();

    // Methods
    protected override JsonObjectContract CreateObjectContract(Type objectType)
    {
        var contract = base.CreateObjectContract(objectType);
        HandleRequiredProperties(contract);
        return contract;
    }

    public static void HandleRequiredProperties(JsonObjectContract contract)
    {
        foreach (var jsonProperty in contract.Properties.Where(RequiredPropertyFilter))
            jsonProperty.Required = Required.Always;
    }

    private static bool RequiredPropertyFilter(JsonProperty jsonProperty)
        => jsonProperty.AttributeProvider.GetAttributes(typeof(RequiredAttribute), inherit: true).Any();
}
