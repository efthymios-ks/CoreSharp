﻿using System;
using System.Collections.Generic;
using System.Dynamic;

namespace CoreSharp.Extensions
{
    /// <summary>
    /// ExpandoObject extensions. 
    /// </summary>
    public static class ExpandoObjectExtensions
    {
        /// <summary>
        /// Convert to dictionary. 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IDictionary<string, object> ToDictionary(this ExpandoObject source)
        {
            _ = source ?? throw new ArgumentNullException(nameof(source));

            return source;
        }

        /// <summary>
        /// Try get specific value by key. 
        /// </summary> 
        public static TValue GetValue<TValue>(this ExpandoObject source, string key)
        {
            _ = source ?? throw new ArgumentNullException(nameof(source));

            var dictionary = source.ToDictionary();
            return (TValue)dictionary[key];
        }
    }
}
