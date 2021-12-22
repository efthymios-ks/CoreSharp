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
            => (guid ?? Guid.Empty).IsNullOrEmpty();

        /// <summary>
        /// Indicates whether the specified <see cref="Guid"/> is
        /// null or an empty <see cref="Guid.Empty"/>.
        /// </summary>
        public static bool IsNullOrEmpty(this Guid guid)
            => guid == Guid.Empty;
    }
}
