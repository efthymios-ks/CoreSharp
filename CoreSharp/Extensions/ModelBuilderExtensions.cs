
using System;
using CoreSharp.Implementations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreSharp.Extensions
{
    /// <summary>
    /// ModelBuilder extensions. 
    /// </summary>
    public static partial class ModelBuilderExtensions
    {
        /// <summary>
        /// Configure and seed enum to database table. 
        /// </summary> 
        public static ModelBuilder HasEnum<TEnum>(this ModelBuilder builder, string tableName) where TEnum : struct, IConvertible
        {
            builder = builder ?? throw new ArgumentNullException(nameof(builder));
            if (string.IsNullOrWhiteSpace(tableName))
                throw new ArgumentNullException(nameof(tableName));
            if (!typeof(TEnum).IsEnum)
                throw new ArgumentException($"{typeof(TEnum).FullName} is not an enum.", nameof(TEnum));

            builder.ConfigureEnum<TEnum>(tableName);
            builder.SeedEnum<TEnum>();

            return builder;
        }

        private static EntityTypeBuilder ConfigureEnum<TEnum>(this ModelBuilder builder, string tableName) where TEnum : struct, IConvertible
        {
            builder = builder ?? throw new ArgumentNullException(nameof(builder));
            if (string.IsNullOrWhiteSpace(tableName))
                throw new ArgumentNullException(nameof(tableName));
            if (!typeof(TEnum).IsEnum)
                throw new ArgumentException($"{typeof(TEnum).FullName} is not an enum.", nameof(TEnum));

            var entityBuilder = builder.Entity<EnumShadowEntity<TEnum>>();
            entityBuilder.ToTable(tableName);
            entityBuilder.HasKey(e => e.Value);
            entityBuilder.Property(e => e.Value);
            entityBuilder.Property(e => e.Name).IsRequired();
            return entityBuilder;
        }

        private static void SeedEnum<TEnum>(this ModelBuilder builder) where TEnum : struct, IConvertible
        {
            builder = builder ?? throw new ArgumentNullException(nameof(builder));
            if (!typeof(TEnum).IsEnum)
                throw new ArgumentException($"{typeof(TEnum).FullName} is not an enum.", nameof(TEnum));

            foreach (TEnum value in Enum.GetValues(typeof(TEnum)))
            {
                var entry = new EnumShadowEntity<TEnum>
                {
                    Value = value,
                    Name = $"{value}"
                };

                builder
                    .Entity<EnumShadowEntity<TEnum>>()
                    .HasData(entry);
            }
        }
    }
}
