using System;
using System.Globalization;

namespace CoreSharp.Extensions
{
    /// <summary>
    /// ulong extensions.
    /// </summary>
    public static partial class UlongExtensions
    {
        /// <summary>
        /// Downsizes bytes and adds appropriate prefix. 
        /// </summary> 
        public static string ToComputerSize(this ulong byteSize)
        {
            return byteSize.ToComputerSize("{0}");
        }

        /// <summary>
        /// Downsizes bytes and adds appropriate prefix. 
        /// </summary> 
        public static string ToComputerSize(this ulong byteSize, string format)
        {
            return byteSize.ToComputerSize(format, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Downsizes bytes and adds appropriate prefix. 
        /// </summary> 
        public static string ToComputerSize(this ulong byteSize, IFormatProvider formatProvider)
        {
            return byteSize.ToComputerSize("{0}", formatProvider);
        }

        /// <summary>
        /// Downsizes bytes and adds appropriate prefix. 
        /// </summary> 
        public static string ToComputerSize(this ulong byteSize, string format, IFormatProvider formatProvider)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Downsizes bytes and adds appropriate prefix. 
        /// Uses `0.###` and `CultureInfo.InvariantCulture`.
        /// </summary> 
        public static string ToComputerSizeCI(this ulong byteSize)
        {
            return byteSize.ToComputerSize("0.###", CultureInfo.InvariantCulture);
        }
    }
}
