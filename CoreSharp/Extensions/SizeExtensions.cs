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
        {
            var sizeF = new SizeF(source);
            return sizeF;
        }

        /// <summary>
        /// Scale Size proportionally. 
        /// </summary>
        public static Size Scale(this Size source, Size target)
        {
            var sourceF = source.ToSizeF();
            var targetF = target.ToSizeF();
            var result = sourceF.Scale(targetF).ToSize();
            return result;
        }
    }
}
