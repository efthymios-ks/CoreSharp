using System;
using System.Drawing;

namespace CoreSharp.Extensions
{
    /// <summary>
    /// SizeF extensions. 
    /// </summary>
    public static class SizeFExtensions
    {
        /// <summary>
        /// Convert SizeF to Size. 
        /// </summary>
        public static Size ToSize(this SizeF source) => Size.Round(source);

        /// <summary>
        /// Scale SizeF proportionally. 
        /// </summary>
        public static SizeF Scale(this SizeF source, SizeF target)
        {
            var ratioX = target.Width / source.Width;
            var ratioY = target.Height / source.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = source.Width * ratio;
            var newHeight = source.Height * ratio;

            return new SizeF(newWidth, newHeight);
        }
    }
}
