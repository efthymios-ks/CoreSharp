﻿using CoreSharp.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoreSharp.Models.Newtonsoft.Resolvers
{
    public class PrimitiveOnlyResolver : WritableOnlyPropertiesResolver
    {
        //Fields
        private static PrimitiveOnlyResolver _instance;

        //Properties
        new public static PrimitiveOnlyResolver Instance => _instance ??= new PrimitiveOnlyResolver();

        //Methods
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            //Get writable only properties
            var properties = base.CreateProperties(type, memberSerialization);

            return properties.Where(j => j.PropertyType.IsExtendedPrimitive()).ToList();
        }
    }
}