using System.Drawing;

namespace CoreSharp.Extensions
{
    /// <summary>
    /// Size extensions.
    /// </summary>
    public static class SizeExtensions
    {
        /// <summary>
        /// Convert Size to SizeF.
        /// </summary>
        public static SizeF ToSizeF(this Size source)
            => new(source);

        /// <inheritdoc cref="SizeFExtensions.Scale(SizeF, SizeF)"/>
        public static Size Scale(this Size source, Size target)
        {
            var sourceF = source.ToSizeF();
            var targetF = target.ToSizeF();
            return sourceF.Scale(targetF).ToSize();
        }
    }
}
