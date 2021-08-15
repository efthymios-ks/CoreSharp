using System;
using System.ComponentModel.DataAnnotations;

namespace CoreSharp.Implementations.EntityFramework
{
    /// <summary>
    /// Enum shadow entity used for one-to-many relationship.
    /// </summary> 
    public class EnumShadowEntity<TEnum> where TEnum : Enum
    {
        //Properties
        [Key]
        public TEnum Value { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
