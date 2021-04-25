using System;
using System.Collections.Generic;

namespace CoreSharp.Implementations
{
    /// <summary>
    /// Enum shadow entity used for one-to-many relationship.
    /// </summary> 
    public class EnumShadowEntity<TEnum> where TEnum : struct, IConvertible
    {
        public TEnum Value { get; set; }
        public string Name { get; set; }
    }

    /// <summary>
    /// Enum shadow entity used for many-to-many relationship.
    /// </summary> 
    public class EnumShadowEntity<TEnum, TEntity> : EnumShadowEntity<TEnum>
        where TEnum : struct, IConvertible
        where TEntity : class
    {
        public ICollection<TEntity> ParentEntities { get; set; }
    }
}
