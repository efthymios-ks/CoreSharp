using System;
using System.Linq.Expressions;
using CoreSharp.Implementations.EntityFramework;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreSharp.Extensions
{
    /// <summary>
    /// EntityTypeBuilder extensions.
    /// </summary>
    public static class EntityTypeBuilderExtensions
    {
        /// <summary>
        /// One-to-many relation with an enum.
        /// </summary>
        public static ReferenceCollectionBuilder<EnumShadowEntity<TEnum>, TEntity> HasOneEnum<TEntity, TEnum>(this EntityTypeBuilder<TEntity> builder, Expression<Func<TEntity, TEnum>> propertySelector)
            where TEntity : class
            where TEnum : Enum
        {
            _ = builder ?? throw new ArgumentNullException(nameof(builder));
            _ = propertySelector ?? throw new ArgumentNullException(nameof(propertySelector));
            if (!typeof(TEnum).IsEnum)
                throw new ArgumentException($"{typeof(TEnum).FullName} is not an enum.", nameof(TEnum));

            var enumPropertyName = propertySelector.GetMemberName();
            return builder
                     .HasOne<EnumShadowEntity<TEnum>>()
                     .WithMany()
                     .HasForeignKey(enumPropertyName);
        }
    }
}
