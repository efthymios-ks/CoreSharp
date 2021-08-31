using System;
using CoreSharp.Models.Newtonsoft;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;

namespace CoreSharp.Extensions
{
    /// <summary>
    /// PropertyBuilder extensions. 
    /// </summary>
    public static partial class PropertyBuilderExtensions
    {
        /// <inheritdoc cref="HasJsonConversion{TEntity}(PropertyBuilder{TEntity}, JsonSerializerSettings)"/>
        public static PropertyBuilder<TEntity> HasJsonConversion<TEntity>(this PropertyBuilder<TEntity> builder) where TEntity : class
        {
            var settings = new JsonSerializerDefaultSettings();
            return builder.HasJsonConversion(settings);
        }

        /// <summary>
        /// Convert a property from and to json for database storage. 
        /// </summary>
        public static PropertyBuilder<TEntity> HasJsonConversion<TEntity>(this PropertyBuilder<TEntity> builder, JsonSerializerSettings settings) where TEntity : class
        {
            _ = builder ?? throw new ArgumentNullException(nameof(builder));
            _ = settings ?? throw new ArgumentNullException(nameof(settings));

            var converter = new ValueConverter<TEntity, string>(
                v => v.ToJson(settings),
                v => v.ToEntity<TEntity>(settings));

            var comparer = new ValueComparer<TEntity>(
                (l, r) => l.ToJson(settings) == r.ToJson(settings),
                v => v == null ? 0 : v.ToJson(settings).GetHashCode(),
                v => v.JsonClone(settings));

            builder.HasConversion(converter);
            builder.Metadata.SetValueConverter(converter);
            builder.Metadata.SetValueComparer(comparer);

            return builder;
        }
    }
}
