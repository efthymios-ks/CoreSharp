using Microsoft.AspNetCore.Http.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

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

        /// <summary>
        /// Add <see cref="IDictionary{TKey, TValue}"/>.
        /// </summary>
        public void Add<TValue>(IDictionary<string, TValue> dictionary)
        {
            _ = dictionary ?? throw new ArgumentNullException(nameof(dictionary));

            foreach (var pair in dictionary)
                InternalAdd(pair.Key, pair.Value);
        }

        /// <summary>
        /// Split item into properties and add one-by-one.
        /// </summary>
        public void Parse<TItem>(TItem item)
            where TItem : class
        {
            _ = item ?? throw new ArgumentNullException(nameof(item));

            var properties = item.GetType()
                                 .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                 .ToDictionary(p => p.Name, p => p.GetValue(item));
            Add(properties);
        }

        /// <summary>
        /// Add <see cref="DateTime"/> and format with "s" specifier.
        /// </summary>
        public void Add(string key, DateTime value)
            => InternalAdd(key, value.ToString("s"));

        /// <summary>
        /// Add <see cref="DateTimeOffset"/> and format with "s" specifier.
        /// </summary>
        public void Add(string key, DateTimeOffset value)
            => InternalAdd(key, value.ToString("s"));

        /// <summary>
        /// Add <see cref="TimeSpan"/> and format with "s" specifier.
        /// </summary>
        public void Add(string key, TimeSpan value)
            => InternalAdd(key, value.ToString("c"));

        /// <summary>
        /// Add <see cref="IEnumerable"/>.
        /// </summary>
        public void Add(string key, IEnumerable items)
        {
            _ = items ?? throw new ArgumentNullException(nameof(items));

            foreach (var item in items)
                InternalAdd(key, item);
        }
    }
}
