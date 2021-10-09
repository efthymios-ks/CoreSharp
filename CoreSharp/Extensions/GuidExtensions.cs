using System;

namespace CoreSharp.Extensions
{
    /// <summary>
    /// <see cref="Guid"/> extensions.
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
        /// Compare given value with <see cref="Guid.Empty"/>.
        /// </summary>
        public static bool IsNullOrEmpty(this Guid guid)
            => guid == Guid.Empty;
    }
}
