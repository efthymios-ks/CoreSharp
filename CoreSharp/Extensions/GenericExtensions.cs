using CoreSharp.Models.EqualityComparers;
using CoreSharp.Models.Newtonsoft.Settings;
using CoreSharp.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml.Linq;
using System.Xml.Serialization;
using JsonNet = Newtonsoft.Json;
using TextJson = System.Text.Json;

namespace CoreSharp.Extensions
{
    /// <summary>
    /// Generic extensions.
    /// </summary>
    public static class GenericExtensions
    {
        /// <inheritdoc cref="IsIn{T}(T, T[])"/>
        public static bool IsIn<T>(this T item, IEnumerable<T> source)
            => item.IsIn(source?.ToArray());

        /// <inheritdoc cref="IsIn{TEntity, TKey}(TEntity, IEnumerable{TEntity}, Func{TEntity, TKey})"/>
        public static bool IsIn<T>(this T item, params T[] source)
        {
            _ = item ?? throw new ArgumentNullException(nameof(item));
            _ = source ?? throw new ArgumentNullException(nameof(source));

            return item.IsIn(source, i => i);
        }

        /// <summary>
        /// Determines whether a sequence contains the specified element.
        /// </summary>
        public static bool IsIn<TEntity, TKey>(this TEntity item, IEnumerable<TEntity> source, Func<TEntity, TKey> keySelector)
        {
            _ = keySelector ?? throw new ArgumentNullException(nameof(keySelector));

            var equalityComparer = new KeyEqualityComparer<TEntity, TKey>(keySelector);
            return source.Contains(item, equalityComparer);
        }

        /// <inheritdoc cref="ToJson{TEntity}(TEntity, JsonNet.JsonSerializerSettings)"/>
        public static string ToJson<TEntity>(this TEntity entity)
            where TEntity : class
            => entity.ToJson(DefaultJsonSettings.Instance);

        /// <inheritdoc cref="ToJson{TEntity}(TEntity, JsonNet.JsonSerializerSettings)"/>
        public static string ToJson<TEntity>(this TEntity entity, TextJson.JsonSerializerOptions options)
            where TEntity : class
        {
            _ = entity ?? throw new ArgumentNullException(nameof(entity));
            _ = options ?? throw new ArgumentNullException(nameof(options));

            return TextJson.JsonSerializer.Serialize(entity, options);
        }

        /// <summary>
        /// Serialize the specified object to JSON.
        /// </summary>
        public static string ToJson<TEntity>(this TEntity entity, JsonNet.JsonSerializerSettings settings)
            where TEntity : class
        {
            _ = entity ?? throw new ArgumentNullException(nameof(entity));
            _ = settings ?? throw new ArgumentNullException(nameof(settings));

            return JsonNet.JsonConvert.SerializeObject(entity, settings);
        }

        /// <inheritdoc cref="JsonClone{TEntity}(TEntity, JsonNet.JsonSerializerSettings)"/>
        public static TEntity JsonClone<TEntity>(this TEntity item)
            where TEntity : class
            => item.JsonClone(DefaultJsonSettings.Instance);

        /// <inheritdoc cref="JsonClone{TEntity}(TEntity, JsonNet.JsonSerializerSettings)"/>
        public static TEntity JsonClone<TEntity>(this TEntity item, TextJson.JsonSerializerOptions options)
            where TEntity : class
        {
            _ = item ?? throw new ArgumentNullException(nameof(item));
            _ = options ?? throw new ArgumentNullException(nameof(options));

            var json = TextJson.JsonSerializer.Serialize(item, options);
            return TextJson.JsonSerializer.Deserialize<TEntity>(json, options);
        }

        /// <summary>
        /// Perform a deep copy using json serialization.
        /// </summary>
        public static TEntity JsonClone<TEntity>(this TEntity item, JsonNet.JsonSerializerSettings settings)
            where TEntity : class
        {
            _ = item ?? throw new ArgumentNullException(nameof(item));
            _ = settings ?? throw new ArgumentNullException(nameof(settings));

            var json = JsonNet.JsonConvert.SerializeObject(item, settings);
            return JsonNet.JsonConvert.DeserializeObject<TEntity>(json, settings);
        }

        /// <inheritdoc cref="JsonEquals{TEntity}(TEntity, TEntity, JsonNet.JsonSerializerSettings)"/>
        public static bool JsonEquals<TEntity>(this TEntity left, TEntity right)
            where TEntity : class
            => left.JsonEquals(right, DefaultJsonSettings.Instance);

        /// <inheritdoc cref="JsonEquals{TEntity}(TEntity, TEntity, JsonNet.JsonSerializerSettings)"/>
        public static bool JsonEquals<TEntity>(this TEntity left, TEntity right, TextJson.JsonSerializerOptions options)
            where TEntity : class
        {
            _ = options ?? throw new ArgumentNullException(nameof(options));

            var equalityComparer = new JsonEqualityComparer<TEntity>(options);
            return equalityComparer.Equals(left, right);
        }

        /// <summary>
        /// Compares two entities using
        /// <see cref="JsonEqualityComparer{TEntity}"/>.
        /// </summary>
        public static bool JsonEquals<TEntity>(this TEntity left, TEntity right, JsonNet.JsonSerializerSettings settings)
            where TEntity : class
        {
            _ = settings ?? throw new ArgumentNullException(nameof(settings));

            var equalityComparer = new JsonEqualityComparer<TEntity>(settings);
            return equalityComparer.Equals(left, right);
        }

        /// <summary>
        /// Perform a deep copy using reflection
        /// and public, primitive properties.
        /// Uses <see cref="TypeExtensions.IsPrimitiveExtended(Type)"/>.
        /// </summary>
        public static TEntity PrimitiveClone<TEntity>(this TEntity item)
            where TEntity : class, new()
        {
            _ = item ?? throw new ArgumentNullException(nameof(item));

            static bool PrimititeTypePredicate(PropertyInfo properyInfo)
            {
                _ = properyInfo ?? throw new ArgumentNullException(nameof(properyInfo));

                if (!properyInfo.CanWrite)
                    return false;
                else if (!properyInfo.CanRead)
                    return false;
                else if (!properyInfo.PropertyType.IsPrimitiveExtended())
                    return false;
                else
                    return true;
            }

            var properties = typeof(TEntity).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                            .Where(PrimititeTypePredicate);

            var result = new TEntity();
            foreach (var property in properties)
            {
                var value = property.GetValue(item);
                property.SetValue(result, value);
            }

            return result;
        }

        /// <summary>
        /// Compares two entities using
        /// <see cref="PrimitiveEqualityComparer{TEntity}"/>.
        /// </summary>
        public static bool PrimitiveEquals<TEntity>(this TEntity left, TEntity right)
            where TEntity : class
        {
            var equalityComparer = new PrimitiveEqualityComparer<TEntity>();
            return equalityComparer.Equals(left, right);
        }

        /// <inheritdoc cref="IsNull{T}(T?)"/>
        public static bool IsNull<T>(this T input)
            where T : class
            => input is null;

        /// <inheritdoc cref="Nullable{T}.HasValue"/>
        public static bool IsNull<T>(this T? input)
            where T : struct
            => !input.HasValue;

        /// <summary>
        /// Check if struct has default value.
        /// </summary>
        public static bool IsDefault<T>(this T input)
            where T : struct
            => input.Equals(default(T));

        /// <summary>
        /// Serializes an object into an XML document.
        /// </summary>
        public static XDocument ToXDocument<TEntity>(this TEntity entity)
            where TEntity : class
        {
            _ = entity ?? throw new ArgumentNullException(nameof(entity));

            var serializer = new XmlSerializer(typeof(TEntity));
            var document = new XDocument();
            using var writer = document.CreateWriter();
            serializer.Serialize(writer, entity);

            return document;
        }

        /// <inheritdoc cref="ToXDocument{TEntity}(TEntity)" />
        public static string ToXml<TEntity>(this TEntity entity)
            where TEntity : class
            => entity.ToXDocument().ToString();

        /// <inheritdoc cref="GetPropertiesDictionary{TEntity}(TEntity, BindingFlags)"/>
        public static IDictionary<string, object> GetPropertiesDictionary<TEntity>(this TEntity entity)
            where TEntity : class
            => entity.GetPropertiesDictionary(BindingFlags.Public | BindingFlags.Instance);

        /// <summary>
        /// Convert item properties to <see cref="IDictionary{TKey, TValue}"/>.
        /// </summary>
        public static IDictionary<string, object> GetPropertiesDictionary<TEntity>(this TEntity entity, BindingFlags flags)
            where TEntity : class
        {
            _ = entity ?? throw new ArgumentNullException(nameof(entity));

            return entity
                    .GetType()
                    .GetProperties(flags)
                    .ToDictionary(p => p.Name, p => p.GetValue(entity));
        }

#pragma warning disable IDE0060 // Remove unused parameter
#pragma warning disable RCS1175 // Unused this parameter.
        /// <inheritdoc cref="TypeExtensions.GetAttributes{TAttribute}(Type)"/>
        public static IEnumerable<TAttribute> GetAttributes<TItem, TMember, TAttribute>(this TItem item, Expression<Func<TItem, TMember>> memberSelector)
            where TAttribute : Attribute
#pragma warning restore RCS1175 // Unused this parameter.
#pragma warning restore IDE0060 // Remove unused parameter
            => ExpressionX.GetMemberInfo(memberSelector).GetAttributes<TAttribute>();

        /// <inheritdoc cref="TypeExtensions.GetAttribute{TAttribute}(Type)"/>
        public static TAttribute GetAttribute<TItem, TMember, TAttribute>(this TItem item, Expression<Func<TItem, TMember>> memberSelector)
            where TAttribute : Attribute
            => item.GetAttributes<TItem, TMember, TAttribute>(memberSelector)?.FirstOrDefault();
    }
}
