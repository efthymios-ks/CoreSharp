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
        /// Attempts to get the item with the specified key.
        /// </summary>
        public static bool TryGet<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, out TValue value)
        {
            _ = source ?? throw new ArgumentNullException(nameof(source));

            return source.TryGetValue(key, out value);
        }

        /// <summary>
        /// Attempts to add the item with the specified key.
        /// </summary>
        public static bool TryAdd<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, TValue value)
        {
            _ = source ?? throw new ArgumentNullException(nameof(source));

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

        /// <inheritdoc cref="TryRemove{TKey, TValue}(IDictionary{TKey, TValue}, TKey, out TValue)"/>
        public static bool TryRemove<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key)
            => source.TryRemove(key, out _);

        /// <summary>
        /// Attempts to remove the item the specified key in dictionary and return the value removed.
        /// </summary>
        public static bool TryRemove<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, out TValue value)
        {
            _ = source ?? throw new ArgumentNullException(nameof(source));

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

        /// <inheritdoc cref="TryUpdate{TKey, TValue}(IDictionary{TKey, TValue}, TKey, Func{TKey, TValue, TValue})"/>
        public static bool TryUpdate<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, TValue value)
            => source.TryUpdate(key, _ => value);

        /// <inheritdoc cref="TryUpdate{TKey, TValue}(IDictionary{TKey, TValue}, TKey, Func{TKey, TValue, TValue})"/>
        public static bool TryUpdate<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, Func<TValue, TValue> updateAction)
        {
            _ = updateAction ?? throw new ArgumentNullException(nameof(updateAction));

            return source.TryUpdate(key, (_, v) => updateAction(v));
        }

        /// <summary>
        /// Attempts to update the specifed key in dictionary, if exists.
        /// </summary>
        /// <param name="updateAction">(key, value) => ...</param>
        public static bool TryUpdate<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, Func<TKey, TValue, TValue> updateAction)
        {
            _ = source ?? throw new ArgumentNullException(nameof(source));
            _ = updateAction ?? throw new ArgumentNullException(nameof(updateAction));

            if (source.ContainsKey(key))
            {
                source[key] = updateAction(key, source[key]);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <inheritdoc cref="AddOrUpdate{TKey, TValue}(IDictionary{TKey, TValue}, TKey, TValue, Func{TKey, TValue, TValue})"/>
        public static TValue AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, TValue value)
            => source.AddOrUpdate(key, value, (_, __) => value);

        /// <inheritdoc cref="AddOrUpdate{TKey, TValue}(IDictionary{TKey, TValue}, TKey, TValue, Func{TKey, TValue, TValue})"/>
        public static TValue AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, TValue addValue, TValue updateValue)
            => source.AddOrUpdate(key, addValue, (_, __) => updateValue);

        /// <inheritdoc cref="AddOrUpdate{TKey, TValue}(IDictionary{TKey, TValue}, TKey, TValue, Func{TKey, TValue, TValue})"/>
        public static TValue AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, TValue addValue, Func<TValue, TValue> updateAction)
        {
            _ = updateAction ?? throw new ArgumentNullException(nameof(updateAction));

            return source.AddOrUpdate(key, addValue, (_, v) => updateAction(v));
        }

        /// <summary>
        /// Attempts to add or update an item with the specified key.
        /// </summary>
        /// <param name="updateAction">(key, value) => ...</param>
        public static TValue AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, TValue addValue, Func<TKey, TValue, TValue> updateAction)
        {
            _ = source ?? throw new ArgumentNullException(nameof(source));
            _ = updateAction ?? throw new ArgumentNullException(nameof(updateAction));

            if (source.ContainsKey(key))
                source.TryUpdate(key, updateAction);
            else
                source.TryAdd(key, addValue);

            return source[key];
        }

        /// <inheritdoc cref="GetOrAdd{TKey, TValue}(IDictionary{TKey, TValue}, TKey, TValue)"/>
        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key)
            => source.GetOrAdd(key, default);

        /// <summary>
        /// If value exists, get, else add and get.
        /// </summary>
        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, TValue addValue)
        {
            _ = source ?? throw new ArgumentNullException(nameof(source));
            _ = addValue ?? throw new ArgumentNullException(nameof(addValue));

            source.TryAdd(key, addValue);
            return source[key];
        }

        /// <summary>
        /// Converts dictionary to KeyValuePair enumerable.
        /// </summary>
        public static IEnumerable<KeyValuePair<TKey, TValue>> ToEnumerable<TKey, TValue>(this IDictionary<TKey, TValue> source)
        {
            _ = source ?? throw new ArgumentNullException(nameof(source));

            return source;
        }

        /// <summary>
        /// Returns given value occurrences in dictionary.
        /// </summary>
        public static bool ContainsValue<TKey, TValue>(this IDictionary<TKey, TValue> source, TValue value)
        {
            _ = source ?? throw new ArgumentNullException(nameof(source));

            return source.Values.Any(v => v.Equals(value));
        }

        /// <summary>
        /// Build url query string from parameters dictionary.
        /// Converts both key and value to string with default converter.
        /// </summary>
        public static string ToUrlQueryString<TKey, TValue>(this IDictionary<TKey, TValue> parameters, bool encodeParameters = true)
        {
            _ = parameters ?? throw new ArgumentNullException(nameof(parameters));

            var pairs = parameters.Select(p =>
            {
                var key = $"{p.Key}";
                var value = $"{p.Value}";

                if (encodeParameters)
                {
                    key = HttpUtility.UrlEncode(key);
                    value = HttpUtility.UrlEncode(value);
                }

                key = key.Trim();

                return $"{key}={value}";
            });

            return string.Join("&", pairs);
        }
    }
}
