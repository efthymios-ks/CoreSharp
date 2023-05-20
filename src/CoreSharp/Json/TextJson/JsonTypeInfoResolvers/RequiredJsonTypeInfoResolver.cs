using Newtonsoft.Json;
using System.Linq;
using System.Text.Json.Serialization.Metadata;

namespace CoreSharp.Json.TextJson.JsonTypeInfoResolvers;

public sealed class RequiredJsonTypeInfoResolver : DefaultJsonTypeInfoResolver
{
    // Constructors
    public RequiredJsonTypeInfoResolver()
        => Modifiers.Add(HandleRequiredProperties);

    // Methods
    private static void HandleRequiredProperties(JsonTypeInfo jsonTypeInfo)
    {
        if (jsonTypeInfo.Kind != JsonTypeInfoKind.Object)
        {
            return;
        }

        foreach (var jsonPropertyInfo in jsonTypeInfo.Properties.Where(IsPropertyRequired))
        {
            jsonPropertyInfo.IsRequired = true;
        }
    }

    private static bool IsPropertyRequired(JsonPropertyInfo jsonPropertyInfo)
        => jsonPropertyInfo.AttributeProvider.IsDefined(typeof(Required), inherit: true);
}
