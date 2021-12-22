﻿using CoreSharp.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoreSharp.Extensions
{
    /// <summary>
    /// <see cref="IDictionary{TKey, TValue}"/> extensions.
    /// </summary>
    public static class IDictionaryExtensions
    {
        /// <inheritdoc cref="IDictionary{TKey, TValue}.TryGetValue(TKey, out TValue)" />
        public static bool TryGet<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, out TValue value)
        {
            _ = source ?? throw new ArgumentNullException(nameof(source));

            return source.TryGetValue(key, out value);
        }

        /// <summary>
        /// Chain calls <see cref="IDictionary{TKey, TValue}.ContainsKey(TKey)"/> and <see cref="IDictionary{TKey, TValue}.Add(TKey, TValue)"/>.
        /// </summary>
        public static bool TryAdd<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, TValue value)
        {
            _ = source ?? throw new ArgumentNullException(nameof(source));

            if (!source.ContainsKey(key))
            {
                source.Add(key, value);
                return true;
            }

            return false;
        }

        /// <inheritdoc cref="TryRemove{TKey, TValue}(IDictionary{TKey, TValue}, TKey, out TValue)"/>
        public static bool TryRemove<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key)
            => source.TryRemove(key, out _);

        /// <summary>
        /// Chain calls <see cref="IDictionary{TKey, TValue}.ContainsKey(TKey)"/> and <see cref="IDictionary{TKey, TValue}.Remove(TKey)"/>.
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

            return false;
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
        /// Attempts to update the specified key in dictionary, if exists.
        /// <code>
        /// dictionary.TryUpdate("key1", (key, value) => value + 5);
        /// </code>
        /// </summary>
        public static bool TryUpdate<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, Func<TKey, TValue, TValue> updateAction)
        {
            _ = source ?? throw new ArgumentNullException(nameof(source));
            _ = updateAction ?? throw new ArgumentNullException(nameof(updateAction));

            if (source.ContainsKey(key))
            {
                source[key] = updateAction(key, source[key]);
                return true;
            }

            return false;
        }

        /// <inheritdoc cref="AddOrUpdate{TKey, TValue}(IDictionary{TKey, TValue}, TKey, TValue, Func{TKey, TValue, TValue})"/>
        public static TValue AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, TValue value)
            => source.AddOrUpdate(key, value, (_, _) => value);

        /// <inheritdoc cref="AddOrUpdate{TKey, TValue}(IDictionary{TKey, TValue}, TKey, TValue, Func{TKey, TValue, TValue})"/>
        public static TValue AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, TValue addValue, TValue updateValue)
            => source.AddOrUpdate(key, addValue, (_, _) => updateValue);

        /// <inheritdoc cref="AddOrUpdate{TKey, TValue}(IDictionary{TKey, TValue}, TKey, TValue, Func{TKey, TValue, TValue})"/>
        public static TValue AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, TValue addValue, Func<TValue, TValue> updateAction)
        {
            _ = updateAction ?? throw new ArgumentNullException(nameof(updateAction));

            return source.AddOrUpdate(key, addValue, (_, v) => updateAction(v));
        }

        /// <summary>
        /// Attempts to add or update an item with the specified key.
        /// <code>
        /// dictionary.AddOrUpdate("key1", (key, value) => value + 5);
        /// </code>
        /// </summary>
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
        /// Converts <see cref="IDictionary{TKey, TValue}"/> to
        /// <see cref="IEnumerable{T}"/> of <see cref="KeyValuePair{TKey, TValue}"/>.
        /// </summary>
        public static IEnumerable<KeyValuePair<TKey, TValue>> ToEnumerable<TKey, TValue>(this IDictionary<TKey, TValue> source)
        {
            _ = source ?? throw new ArgumentNullException(nameof(source));

            return source;
        }

        /// <summary>
        /// Returns given value occurrences in <see cref="IDictionary{TKey, TValue}"/>.
        /// </summary>
        public static bool ContainsValue<TKey, TValue>(this IDictionary<TKey, TValue> source, TValue value)
        {
            _ = source ?? throw new ArgumentNullException(nameof(source));

            return source.Values.Any(v => v.Equals(value));
        }

        /// <summary>
        /// Build url query string from parameters dictionary.
        /// Converts both key and value to <see cref="string"/> with default converter.
        /// </summary>
        public static string ToUrlQueryString<TValue>(this IDictionary<string, TValue> parameters)
        {
            _ = parameters ?? throw new ArgumentNullException(nameof(parameters));

            var builder = new UrlQueryBuilder
            {
                parameters
            };
            return builder.ToString();
        }
    }
}
