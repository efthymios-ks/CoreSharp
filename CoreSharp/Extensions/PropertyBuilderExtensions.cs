using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;

namespace EFCoreDemo.Extensions
{
    /// <summary>
    /// PropertyBuilder extensions. 
    /// </summary>
    public static partial class PropertyBuilderExtensions
    {
        /// <summary>
        /// Convert a property from and to json for database storage. 
        /// </summary>
        public static PropertyBuilder<TEntity> HasJsonConversion<TEntity>(this PropertyBuilder<TEntity> builder) where TEntity : class
        {
            builder = builder ?? throw new ArgumentNullException(nameof(builder));

            var converter = new ValueConverter<TEntity, string>(
                v => JsonConvert.SerializeObject(v),
                v => JsonConvert.DeserializeObject<TEntity>(v));

            var comparer = new ValueComparer<TEntity>(
                (l, r) => JsonConvert.SerializeObject(l) == JsonConvert.SerializeObject(r),
                v => v == null ? 0 : JsonConvert.SerializeObject(v).GetHashCode(),
                v => JsonConvert.DeserializeObject<TEntity>(JsonConvert.SerializeObject(v)));

            builder.HasConversion(converter);
            builder.Metadata.SetValueConverter(converter);
            builder.Metadata.SetValueComparer(comparer);

            return builder;
        }
    }
}
