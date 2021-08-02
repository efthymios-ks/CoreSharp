using System;
using System.Collections.Generic;
using System.Dynamic;

namespace CoreSharp.Extensions
{
    /// <summary>
    /// ExpandoObject extensions. 
    /// </summary>
    public static class ExpandoObjectExtensions
    {
        public static IDictionary<string, object> ToDictionary(this ExpandoObject source)
        {
            _ = source ?? throw new ArgumentNullException(nameof(source));

            return source;
        }

        public static TValue GetValue<TValue>(this ExpandoObject source, string key)
        {
            _ = source ?? throw new ArgumentNullException(nameof(source));

            var dictionary = source.ToDictionary();
            return (TValue)dictionary[key];
        }
    }
}
