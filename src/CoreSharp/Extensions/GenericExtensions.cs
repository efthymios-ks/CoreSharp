using CoreSharp.EqualityComparers;
using CoreSharp.Json.JsonNet;
using CoreSharp.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
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
        /// <inheritdoc cref="EqualsAny{TElement}(TElement, TElement[])"/>
        public static bool EqualsAny<TElement>(this TElement element, IEnumerable<TElement> source)
            => element.EqualsAny(source?.ToArray());

        /// <inheritdoc cref="EqualsAny{TElement, TKey}(TElement, IEnumerable{TElement}, Func{TElement, TKey})"/>
        public static bool EqualsAny<TElement>(this TElement element, params TElement[] source)
            => element.EqualsAny(source, i => i);

        /// <summary>
        /// Determines whether a sequence contains the specified element.
        /// </summary>
        public static bool EqualsAny<TElement, TKey>(this TElement element, IEnumerable<TElement> source, Func<TElement, TKey> keySelector)
        {
            _ = element ?? throw new ArgumentNullException(nameof(element));
            _ = source ?? throw new ArgumentNullException(nameof(source));
            _ = keySelector ?? throw new ArgumentNullException(nameof(keySelector));

            var equalityComparer = new KeyEqualityComparer<TElement, TKey>(keySelector);
            return source.Any(i => equalityComparer.Equals(i, element));
        }

        /// <inheritdoc cref="ToJson{TEntity}(TEntity, JsonNet.JsonSerializerSettings)"/>
        public static string ToJson<TEntity>(this TEntity entity)
            where TEntity : class
            => entity.ToJson(JsonSettings.Default);

        /// <inheritdoc cref="ToJson{TEntity}(TEntity, JsonNet.JsonSerializerSettings)"/>
        public static string ToJson<TEntity>(this TEntity entity, TextJson.JsonSerializerOptions options)
            where TEntity : class
        {
            _ = entity ?? throw new ArgumentNullException(nameof(entity));
            _ = options ?? throw new ArgumentNullException(nameof(options));

            return TextJson.JsonSerializer.Serialize(entity, options);
        }

        /// <inheritdoc cref="ToJsonStreamAsync{TEntity}(TEntity, JsonNet.JsonSerializerSettings, CancellationToken)"/>
        public static string ToJson<TEntity>(this TEntity entity, JsonNet.JsonSerializerSettings settings)
            where TEntity : class
        {
            _ = entity ?? throw new ArgumentNullException(nameof(entity));
            _ = settings ?? throw new ArgumentNullException(nameof(settings));

            return JsonNet.JsonConvert.SerializeObject(entity, settings);
        }

        /// <inheritdoc cref="ToJsonStreamAsync{TEntity}(TEntity, JsonNet.JsonSerializerSettings, CancellationToken)"/>
        public static async Task<Stream> ToJsonStreamAsync<TEntity>(this TEntity entity)
            where TEntity : class
            => await entity.ToJsonStreamAsync(JsonSettings.Default);

        /// <inheritdoc cref="ToJsonStreamAsync{TEntity}(TEntity, JsonNet.JsonSerializerSettings, CancellationToken)"/>
        public static async Task<Stream> ToJsonStreamAsync<TEntity>(
            this TEntity entity,
            TextJson.JsonSerializerOptions options,
            CancellationToken cancellationToken = default)
            where TEntity : class
        {
            _ = entity ?? throw new ArgumentNullException(nameof(entity));
            _ = options ?? throw new ArgumentNullException(nameof(options));

            var stream = new MemoryStream();
            await TextJson.JsonSerializer.SerializeAsync(stream, entity, options, cancellationToken);
            stream.Position = 0;
            return stream;
        }

        /// <summary>
        /// Serialize the specified object to JSON.
        /// </summary>
        public static async Task<Stream> ToJsonStreamAsync<TEntity>(
            this TEntity entity,
            JsonNet.JsonSerializerSettings settings,
            CancellationToken cancellationToken = default)
            where TEntity : class
        {
            _ = entity ?? throw new ArgumentNullException(nameof(entity));
            _ = settings ?? throw new ArgumentNullException(nameof(settings));

            var stream = new MemoryStream();
            var serializer = JsonNet.JsonSerializer.Create(settings);

            using var streamWriter = new StreamWriter(stream, leaveOpen: true);
            using var jsonWriter = new JsonNet.JsonTextWriter(streamWriter);

            serializer.Serialize(jsonWriter, entity);
            await jsonWriter.FlushAsync(cancellationToken);
            stream.Position = 0;

            return stream;
        }

        /// <inheritdoc cref="JsonClone{TEntity}(TEntity, JsonNet.JsonSerializerSettings)"/>
        public static TEntity JsonClone<TEntity>(this TEntity item)
            where TEntity : class
            => item.JsonClone(JsonSettings.Default);

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
            => left.JsonEquals(right, JsonSettings.Default);

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

            var properties = item.GetType()
                                 .GetProperties(BindingFlags.Public | BindingFlags.Instance)
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

        /// <inheritdoc cref="IsNull{TElement}(TElement?)"/>
        public static bool IsNull<TElement>(this TElement element)
            where TElement : class
            => element is null;

        /// <inheritdoc cref="Nullable{TElement}.HasValue"/>
        public static bool IsNull<TElement>(this TElement? element)
            where TElement : struct
            => !element.HasValue;

        /// <summary>
        /// Check if struct has default value.
        /// </summary>
        public static bool IsDefault<TElement>(this TElement element)
            where TElement : struct
            => element.Equals(default(TElement));

        /// <summary>
        /// Serializes an object into an XML document.
        /// </summary>
        public static XDocument ToXDocument<TEntity>(this TEntity entity)
            where TEntity : class
        {
            _ = entity ?? throw new ArgumentNullException(nameof(entity));

            var serializer = new XmlSerializer(entity.GetType());
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

        /// <inheritdoc cref="TypeExtensions.GetAttributes{TAttribute}(Type)"/>
        public static IEnumerable<TAttribute> GetAttributes<TElement, TMember, TAttribute>(this TElement _, Expression<Func<TElement, TMember>> memberSelector)
            where TAttribute : Attribute
            => ExpressionX.GetMemberInfo(memberSelector).GetAttributes<TAttribute>();

        /// <inheritdoc cref="TypeExtensions.GetAttribute{TAttribute}(Type)"/>
        public static TAttribute GetAttribute<TElement, TMember, TAttribute>(this TElement element, Expression<Func<TElement, TMember>> memberSelector)
            where TAttribute : Attribute
            => element.GetAttributes<TElement, TMember, TAttribute>(memberSelector)?.FirstOrDefault();

        /// <summary>
        /// Yield return item in an <see cref="IEnumerable{T}"/>.
        /// </summary>
        public static IEnumerable<TElement> Yield<TElement>(this TElement element)
        {
            if (element is null)
                yield break;
            else
                yield return element;
        }

        /// <summary>
        /// Get innermost field using recursion.
        /// Stops when inner field is null.
        /// </summary>
        public static TEntity GetInnermostField<TEntity>(this TEntity entity, Func<TEntity, TEntity> fieldSelector)
            where TEntity : class
        {
            _ = entity ?? throw new ArgumentNullException(nameof(entity));
            _ = fieldSelector ?? throw new ArgumentNullException(nameof(fieldSelector));

            var innerEnttity = fieldSelector(entity);
            if (innerEnttity is null)
                return entity;
            else
                return innerEnttity.GetInnermostField(fieldSelector);
        }
    }
}
