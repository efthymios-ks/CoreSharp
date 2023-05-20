using Microsoft.AspNetCore.Http.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace CoreSharp.Models;

public class UrlQueryBuilder : QueryBuilder
{
    // Methods
    private void AddInternal(string key, object value)
    {
        if (value is not null)
        {
            Add(key, Convert.ToString(value, CultureInfo.InvariantCulture));
        }
    }

    private void SwitchAdd<TValue>(string key, TValue value)
    {
        if (value is null)
        {
            return;
        }

        var type = typeof(TValue);
        switch (value)
        {
            case DateTime dateTimeValue:
                Add(key, dateTimeValue);
                break;
            case DateTimeOffset dateTimeOffsetValue:
                Add(key, dateTimeOffsetValue);
                break;
            case TimeSpan timeSpanValue:
                Add(key, timeSpanValue);
                break;
            case string stringValue when !string.IsNullOrWhiteSpace(stringValue):
                AddInternal(key, stringValue.Trim());
                break;
            case IEnumerable enumerable:
                AddMany(key, enumerable);
                break;
            default:
                if (value is IEnumerable<object> enumerableObject)
                {
                    AddMany(key, enumerableObject);
                }
                else if (type.IsClass && value is not object)
                {
                    var properties = value.GetType()
                                          .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                          .ToDictionary(p => p.Name, p => p.GetValue(value));
                    Add(properties);
                }
                else
                {
                    AddInternal(key, value);
                }

                break;
        }
    }

    /// <summary>
    /// Split item into properties and add one-by-one.
    /// </summary>
    public void Parse<TEntity>(TEntity item)
        where TEntity : class
    {
        _ = item ?? throw new ArgumentNullException(nameof(item));

        var properties = item.GetType()
                             .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                             .ToDictionary(p => p.Name, p => p.GetValue(item));
        Add(properties);
    }

    /// <summary>
    /// Add <see cref="IDictionary{TKey, TValue}"/>.
    /// </summary>
    public void Add<TValue>(IDictionary<string, TValue> dictionary)
    {
        _ = dictionary ?? throw new ArgumentNullException(nameof(dictionary));

        foreach (var pair in dictionary)
        {
            SwitchAdd(pair.Key, pair.Value);
        }
    }

    /// <summary>
    /// Add <see cref="DateTime"/> and format with "s" specifier.
    /// </summary>
    public void Add(string key, DateTime value)
        => AddInternal(key, value.ToString("s"));

    /// <summary>
    /// Add <see cref="DateTimeOffset"/> and format with "s" specifier.
    /// </summary>
    public void Add(string key, DateTimeOffset value)
        => AddInternal(key, value.ToString("s"));

    /// <summary>
    /// Add <see cref="TimeSpan"/> and format with "c" specifier.
    /// </summary>
    public void Add(string key, TimeSpan value)
        => AddInternal(key, value.ToString("c"));

    /// <inheritdoc cref="AddMany(string, IEnumerable)"/>
    public void AddMany<TElement>(string key, IEnumerable<TElement> items)
        => AddMany(key, (IEnumerable)items);

    /// <summary>
    /// Add <see cref="IEnumerable"/>.
    /// </summary>
    public void AddMany(string key, IEnumerable items)
    {
        _ = items ?? throw new ArgumentNullException(nameof(items));

        foreach (var item in items)
        {
            AddInternal(key, item);
        }
    }
}
