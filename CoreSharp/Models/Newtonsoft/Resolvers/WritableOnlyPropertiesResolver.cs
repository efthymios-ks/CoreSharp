using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoreSharp.Models.Newtonsoft.Resolvers
{
    public class WritableOnlyPropertiesResolver : DefaultContractResolver
    {
        //Fields
        private static WritableOnlyPropertiesResolver _instance;

        //Properties
        public static WritableOnlyPropertiesResolver Instance
            => _instance ??= new WritableOnlyPropertiesResolver();

        //Methods
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
            => base.CreateProperties(type, memberSerialization)
                   .Where(p => p.Writable)
                   .ToList();
    }
}
