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
            return byteSize.ToComputerSize("G");
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
            return byteSize.ToComputerSize("G", formatProvider);
        }

        /// <summary>
        /// Downsizes bytes and adds appropriate prefix. 
        /// </summary> 
        public static string ToComputerSize(this ulong byteSize, string format, IFormatProvider formatProvider)
        {
            format = format ?? throw new ArgumentNullException(nameof(format));
            formatProvider = formatProvider ?? throw new ArgumentNullException(nameof(formatProvider));

            //Scale down bytes  
            const int thousand = 1024;
            int thousandCounter = 0;

            //Integral division 
            var integralLimit = (ulong)Math.Pow(thousand, 2);
            while (byteSize >= integralLimit)
            {
                thousandCounter++;
                byteSize /= thousand;
            }

            //Double division 
            double scaledValue = byteSize;
            while (scaledValue >= thousand)
            {
                thousandCounter++;
                scaledValue /= thousand;
            }

            //Get prefix
            var prefices = new[] { string.Empty, "K", "M", "G", "T", "P", "E", "Z", "Y" };
            var prefix = prefices[thousandCounter];

            return scaledValue.ToString(format, formatProvider) + prefix + "B";
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
