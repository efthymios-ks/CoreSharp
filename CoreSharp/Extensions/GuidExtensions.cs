using System;

namespace CoreSharp.Extensions
{
    /// <summary>
    /// Guid extensions. 
    /// </summary>
    public static partial class GuidExtensions
    {
        /// <summary>
        /// Compare given value with Guid.Empty (all zeros). 
        /// </summary> 
        public static bool IsNullOrEmpty(this Guid guid)
        {
            return guid == Guid.Empty;
        }

        /// <summary>
        /// Compare given value with Guid.Empty (all zeros). 
        /// </summary> 
        public static bool IsNullOrEmpty(this Guid? guid)
        {
            guid ??= Guid.Empty;
            return guid.Value.IsNullOrEmpty();
        }
    }
}
