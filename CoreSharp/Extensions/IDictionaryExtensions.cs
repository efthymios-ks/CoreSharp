using System;
using System.Collections.Generic;
using System.Linq;

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
        public static bool TryGet<T>(this IDictionary<string, T> source, string key, out T value)
        {
            source = source ?? throw new ArgumentNullException(nameof(source));

            return source.TryGetValue(key, out value);
        }

        /// <summary>
        /// Attempts to add the item with the specifed key. 
        /// </summary> 
        public static bool TryAdd<T>(this IDictionary<string, T> source, string key, T value)
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
        public static bool TryRemove<T>(this IDictionary<string, T> source, string key)
        {
            return source.TryRemove(key, out _);
        }

        /// <summary>
        /// Attempts to remove the item the specified key in dictionary and return the value removed. 
        /// </summary> 
        public static bool TryRemove<T>(this IDictionary<string, T> source, string key, out T value)
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
        public static bool TryUpdate<T>(this IDictionary<string, T> source, string key, T value)
        {
            return source.TryUpdate(key, v => value);
        }

        /// <summary>
        /// Attempts to update the specifed key in dictionary, if exists. 
        /// </summary> 
        public static bool TryUpdate<T>(this IDictionary<string, T> source, string key, Func<T, T> updateAction)
        {
            updateAction = updateAction ?? throw new ArgumentNullException(nameof(updateAction));

            return source.TryUpdate(key, (k, v) => updateAction(v));
        }

        /// <summary>
        /// Attempts to update the specifed key in dictionary, if exists. 
        /// </summary> 
        public static bool TryUpdate<T>(this IDictionary<string, T> source, string key, Func<string, T, T> updateAction)
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
        /// <returns>True if added, false if updated</returns>
        public static T AddOrUpdate<T>(this IDictionary<string, T> source, string key, T value)
        {
            return source.AddOrUpdate(key, value, (k, v) => value);
        }

        /// <summary> 
        /// Attempts to add or update an item with the specified key. 
        /// </summary> 
        /// <returns>True if added, false if updated</returns>
        public static T AddOrUpdate<T>(this IDictionary<string, T> source, string key, T addValue, T updateValue)
        {
            return source.AddOrUpdate(key, addValue, (k, v) => updateValue);
        }

        /// <summary> 
        /// Attempts to add or update an item with the specified key. 
        /// </summary> 
        public static T AddOrUpdate<T>(this IDictionary<string, T> source, string key, T addValue, Func<T, T> updateAction)
        {
            updateAction = updateAction ?? throw new ArgumentNullException(nameof(updateAction));

            return source.AddOrUpdate(key, addValue, (k, v) => updateAction(v));
        }

        /// <summary> 
        /// Attempts to add or update an item with the specified key. 
        /// </summary> 
        public static T AddOrUpdate<T>(this IDictionary<string, T> source, string key, T addValue, Func<string, T, T> updateAction)
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
        /// If value exists, get, else add and get.  
        /// </summary> 
        /// <returns>Value found or added.</returns>
        public static T GetOrAdd<T>(this IDictionary<string, T> source, string key, T addValue)
        {
            source = source ?? throw new ArgumentNullException(nameof(source));

            source.TryAdd(key, addValue);
            return source[key];
        }

        /// <summary>
        /// Converts dictionary to KeyValuePair enumerable. 
        /// </summary> 
        public static IEnumerable<KeyValuePair<string, T>> ToEnumerable<T>(this IDictionary<string, T> source)
        {
            source = source ?? throw new ArgumentNullException(nameof(source));

            return source;
        }

        /// <summary>
        /// Returns given value occurences in dictionary.
        /// </summary>
        public static int ContainsValue<T>(this IDictionary<string, T> source, T value)
        {
            source = source ?? throw new ArgumentNullException(nameof(source));

            return source.Values.Count(v => v.Equals(value));
        }
    }
}
