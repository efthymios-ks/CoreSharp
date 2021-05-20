using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CoreSharp.Models.Newtonsoft
{
    internal class WritableOnlyPropertiesResolver : DefaultContractResolver
    {
        //Methods
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            var properties = base.CreateProperties(type, memberSerialization);
            return properties.Where(p => p.Writable).ToList();
        }
    }
}
