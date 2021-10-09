using CoreSharp.Implementations.EntityFramework;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Linq.Expressions;

namespace CoreSharp.Extensions
{
    /// <summary>
    /// <see cref="EntityTypeBuilder{TEntity}"/> extensions.
    /// </summary>
    public static class EntityTypeBuilderExtensions
    {
        /// <summary>
        /// One-to-many relation with an <see cref="Enum"/>.
        /// </summary>
        public static ReferenceCollectionBuilder<EnumShadowEntity<TEnum>, TEntity> HasOneEnum<TEntity, TEnum>(this EntityTypeBuilder<TEntity> builder, Expression<Func<TEntity, TEnum>> propertySelector)
            where TEntity : class
            where TEnum : Enum
        {
            _ = builder ?? throw new ArgumentNullException(nameof(builder));
            _ = propertySelector ?? throw new ArgumentNullException(nameof(propertySelector));

            var enumPropertyName = propertySelector.GetMemberName();
            return builder
                     .HasOne<EnumShadowEntity<TEnum>>()
                     .WithMany()
                     .HasForeignKey(enumPropertyName);
        }
    }
}
