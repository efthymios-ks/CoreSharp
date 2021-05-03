using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoreSharp.Extensions
{
    /// <summary>
    /// IDictionary extensions. 
    /// </summary>
    public static class IDictionaryExtensions
    {
        /// <summary>
        /// Attempts to get the item with the specifed key. 
        /// </summary> 
        public static bool TryGet<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, out TValue value)
        {
            source = source ?? throw new ArgumentNullException(nameof(source));

            return source.TryGetValue(key, out value);
        }

        /// <summary>
        /// Attempts to add the item with the specifed key. 
        /// </summary> 
        public static bool TryAdd<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, TValue value)
        {
            source = source ?? throw new ArgumentNullException(nameof(source));

            if (!source.ContainsKey(key))
            {
                source.Add(key, value);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Attempts to remove the item the specified key in dictionary. 
        /// </summary> 
        public static bool TryRemove<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key)
        {
            return source.TryRemove(key, out _);
        }

        /// <summary>
        /// Attempts to remove the item the specified key in dictionary and return the value removed. 
        /// </summary> 
        public static bool TryRemove<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, out TValue value)
        {
            source = source ?? throw new ArgumentNullException(nameof(source));

            value = default;
            if (source.ContainsKey(key))
            {
                value = source[key];
                source.Remove(key);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Attempts to update the specifed key in dictionary, if exists. 
        /// </summary> 
        public static bool TryUpdate<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, TValue value)
        {
            return source.TryUpdate(key, v => value);
        }

        /// <summary>
        /// Attempts to update the specifed key in dictionary, if exists. 
        /// </summary> 
        /// <param name="updateAction">(value) => ...</param> 
        public static bool TryUpdate<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, Func<TValue, TValue> updateAction)
        {
            updateAction = updateAction ?? throw new ArgumentNullException(nameof(updateAction));

            return source.TryUpdate(key, (k, v) => updateAction(v));
        }

        /// <summary> 
        /// Attempts to update the specifed key in dictionary, if exists.
        /// </summary> 
        /// <param name="updateAction">(key, value) => ...</param> 
        public static bool TryUpdate<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, Func<TKey, TValue, TValue> updateAction)
        {
            source = source ?? throw new ArgumentNullException(nameof(source));
            updateAction = updateAction ?? throw new ArgumentNullException(nameof(updateAction));

            if (source.ContainsKey(key))
            {
                var newValue = updateAction(key, source[key]);
                source[key] = newValue;
                return true;
            }
            else
                return false;
        }

        /// <summary> 
        /// Attempts to add or update an item with the specified key. 
        /// </summary> 
        public static TValue AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, TValue value)
        {
            return source.AddOrUpdate(key, value, (k, v) => value);
        }

        /// <summary> 
        /// Attempts to add or update an item with the specified key. 
        /// </summary> 
        public static TValue AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, TValue addValue, TValue updateValue)
        {
            return source.AddOrUpdate(key, addValue, (k, v) => updateValue);
        }

        /// <summary> 
        /// Attempts to add or update an item with the specified key. 
        /// </summary> 
        /// <param name="updateAction">(value) => ...</param> 
        public static TValue AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, TValue addValue, Func<TValue, TValue> updateAction)
        {
            updateAction = updateAction ?? throw new ArgumentNullException(nameof(updateAction));

            return source.AddOrUpdate(key, addValue, (k, v) => updateAction(v));
        }

        /// <summary> 
        /// Attempts to add or update an item with the specified key. 
        /// </summary> 
        /// <param name="updateAction">(key, value) => ...</param> 
        public static TValue AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, TValue addValue, Func<TKey, TValue, TValue> updateAction)
        {
            source = source ?? throw new ArgumentNullException(nameof(source));
            updateAction = updateAction ?? throw new ArgumentNullException(nameof(updateAction));

            if (source.ContainsKey(key))
                source.TryUpdate(key, updateAction);
            else
                source.TryAdd(key, addValue);

            return source[key];
        }

        /// <summary> 
        /// If value exists, get, else add default and get.  
        /// </summary> 
        /// <returns>Value found or added.</returns>
        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key)
        {
            return source.GetOrAdd(key, default);
        }

        /// <summary> 
        /// If value exists, get, else add and get.  
        /// </summary> 
        /// <returns>Value found or added.</returns>
        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, TValue addValue)
        {
            source = source ?? throw new ArgumentNullException(nameof(source));
            addValue = addValue ?? throw new ArgumentNullException(nameof(addValue));

            source.TryAdd(key, addValue);
            return source[key];
        }

        /// <summary>
        /// Converts dictionary to KeyValuePair enumerable. 
        /// </summary> 
        public static IEnumerable<KeyValuePair<TKey, TValue>> ToEnumerable<TKey, TValue>(this IDictionary<TKey, TValue> source)
        {
            source = source ?? throw new ArgumentNullException(nameof(source));

            return source;
        }

        /// <summary>
        /// Returns given value occurences in dictionary.
        /// </summary>
        public static bool ContainsValue<TKey, TValue>(this IDictionary<TKey, TValue> source, TValue value)
        {
            source = source ?? throw new ArgumentNullException(nameof(source));

            return source.Values.Any(v => v.Equals(value));
        }

        /// <summary>
        /// Build url query string from parameters dictionary. 
        /// Converts both key and value to string with default converter. 
        /// </summary> 
        public static string ToUrlQueryString<TKey, TValue>(this IDictionary<TKey, TValue> parameters, bool encodeParameters = true)
        {
            parameters = parameters ?? throw new ArgumentNullException(nameof(parameters));

            var pairs = parameters.Select(p =>
            {
                string key = $"{p.Key}";
                string value = $"{p.Value}";

                if (encodeParameters)
                {
                    key = HttpUtility.UrlEncode(key);
                    value = HttpUtility.UrlEncode(value);
                }

                key = key.Trim();

                return $"{key}={value}";
            });

            string query = string.Join("&", pairs);
            return query;
        }
    }
}
