using System;

namespace CoreSharp.Extensions
{
    /// <summary>
    /// Guid extensions.
    /// </summary>
    public static class GuidExtensions
    {
        /// <inheritdoc cref="IsNullOrEmpty(Guid)"/>
        public static bool IsNullOrEmpty(this Guid? guid)
        {
            guid ??= Guid.Empty;
            return guid.Value.IsNullOrEmpty();
        }

        /// <summary>
        /// Compare given value with Guid.Empty (all zeros).
        /// </summary>
        public static bool IsNullOrEmpty(this Guid guid) 
            => guid == Guid.Empty;
    }
}
