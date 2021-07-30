﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using CoreSharp.Models.Newtonsoft;
using Newtonsoft.Json;

namespace CoreSharp.Extensions
{
    /// <summary>
    /// Generic extensions. 
    /// </summary>
    public static partial class GenericExtensions
    {
        /// <summary>
        /// Check if value is contained in list. 
        /// </summary>
        public static bool IsIn<T>(this T item, IEnumerable<T> source)
        {
            return item.IsIn(source?.ToArray());
        }

        /// <summary>
        /// Check if value is contained in list. 
        /// </summary>
        public static bool IsIn<TEntity, TKey>(this TEntity item, IEnumerable<TEntity> source, Func<TEntity, TKey> keySelector)
        {
            _ = keySelector ?? throw new ArgumentNullException(nameof(keySelector));

            var itemKey = keySelector(item);
            var sourceKeys = source?.Select(keySelector);
            return itemKey.IsIn(sourceKeys);
        }

        /// <summary>
        /// Check if value is contained in list. 
        /// </summary>
        public static bool IsIn<T>(this T item, params T[] source)
        {
            _ = item ?? throw new ArgumentNullException(nameof(item));
            _ = source ?? throw new ArgumentNullException(nameof(source));

            return source.Contains(item);
        }

        /// <summary> 
        /// Serialize object to json.
        /// </summary> 
        public static string ToJson<TEntity>(this TEntity entity) where TEntity : class
        {
            var settings = new JsonSerializerDefaultSettings();

            return entity.ToJson(settings);
        }

        /// <summary> 
        /// Serialize object to json.
        /// </summary> 
        public static string ToJson<TEntity>(this TEntity entity, JsonSerializerSettings settings) where TEntity : class
        {
            _ = entity ?? throw new ArgumentNullException(nameof(entity));
            _ = settings ?? throw new ArgumentNullException(nameof(settings));

            return JsonConvert.SerializeObject(entity, settings);
        }

        /// <summary> 
        /// Perform a deep copy using Json serialization. 
        /// </summary> 
        public static TEntity JsonClone<TEntity>(this TEntity item) where TEntity : class
        {
            var settings = new JsonSerializerDefaultSettings();

            return item.JsonClone(settings);
        }

        /// <summary> 
        /// Perform a deep copy using Json serialization. 
        /// </summary> 
        public static TEntity JsonClone<TEntity>(this TEntity item, JsonSerializerSettings settings) where TEntity : class
        {
            _ = item ?? throw new ArgumentNullException(nameof(item));
            _ = settings ?? throw new ArgumentNullException(nameof(settings));

            var json = JsonConvert.SerializeObject(item, settings);
            return JsonConvert.DeserializeObject<TEntity>(json, settings);
        }

        /// <summary>
        /// Check if class is null. 
        /// </summary>
        public static bool IsNull<T>(this T input) where T : class
        {
            return input is null;
        }

        /// <summary>
        /// Gets a value indicating whether the current nullable 
        /// object has a valid value of its underlying type. 
        /// </summary>
        public static bool IsNull<T>(this T? input) where T : struct
        {
            return !input.HasValue;
        }

        /// <summary>
        /// Check if struct has default value. 
        /// </summary>
        public static bool IsDefault<T>(this T input) where T : struct
        {
            return input.Equals(default(T));
        }

        /// <summary>
        /// Serialize to XDocument.
        /// </summary> 
        public static XDocument ToXDocument<T>(T input) where T : class
        {
            _ = input ?? throw new ArgumentNullException(nameof(input));

            var serializer = new XmlSerializer(typeof(T));

            var document = new XDocument();
            using (var writer = document.CreateWriter())
                serializer.Serialize(writer, input);

            return document;
        }
    }
}
