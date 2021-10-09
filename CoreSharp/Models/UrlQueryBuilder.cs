using CoreSharp.Extensions;
using Microsoft.AspNetCore.Http.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace CoreSharp.Models
{
    public class UrlQueryBuilder : QueryBuilder
    {
        //Methods
        private void InternalAdd(string key, object value)
        {
            if (value is not null)
                base.Add(key, Convert.ToString(value, CultureInfo.InvariantCulture));
        }

        //Basic 
        public void Add<TValue>(string key, TValue value)
        {
            var type = typeof(TValue);

            if (value is null)
                return;
            else if (value is DateTime dateTimeValue)
                Add(key, dateTimeValue);
            else if (value is DateTimeOffset dateTimeOffsetValue)
                Add(key, dateTimeOffsetValue);
            else if (value is TimeSpan timeSpanValue)
                Add(key, timeSpanValue);
            else if (value is string stringValue && !string.IsNullOrWhiteSpace(stringValue))
                InternalAdd(key, stringValue.Trim());
            else if (value is IEnumerable enumerable)
                Add(key, enumerable);
            else if (value is IEnumerable<object> enumerableObject)
                Add(key, enumerableObject);
            else if (type.IsClass && value is not object)
                Add(((object)value).GetPropertiesDictionary());
            else
                InternalAdd(key, value);
        }

        /// <summary>
        /// Add <see cref="IDictionary{TKey, TValue}"/>.
        /// </summary>
        public void Add<TValue>(IDictionary<string, TValue> dictionary)
        {
            _ = dictionary ?? throw new ArgumentNullException(nameof(dictionary));

            foreach (var pair in dictionary)
                Add(pair.Key, pair.Value);
        }

        //Type-specific 
        /// <summary>
        /// Add <see cref="DateTime"/> and format with "s" specifier.
        /// </summary>
        private void Add(string key, DateTime value)
            => InternalAdd(key, value.ToString("s"));

        /// <summary>
        /// Add <see cref="DateTimeOffset"/> and format with "s" specifier.
        /// </summary>
        private void Add(string key, DateTimeOffset value)
            => InternalAdd(key, value.ToString("s"));

        /// <summary>
        /// Add <see cref="TimeSpan"/> and format with "s" specifier.
        /// </summary>
        private void Add(string key, TimeSpan value)
            => InternalAdd(key, value.ToString("c"));

        /// <summary>
        /// Add <see cref="IEnumerable{T}"/>.
        /// </summary>
        private void Add<TValue>(string key, IEnumerable<TValue> items)
            => Add(key, (IEnumerable)items);

        /// <summary>
        /// Add <see cref="IEnumerable"/>.
        /// </summary>
        private void Add(string key, IEnumerable items)
        {
            _ = items ?? throw new ArgumentNullException(nameof(items));

            foreach (var item in items)
                Add(key, item);
        }
    }
}
