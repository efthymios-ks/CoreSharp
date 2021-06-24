using System;
using System.Globalization;
using System.Text;

namespace CoreSharp.Extensions
{
    /// <summary>
    /// StringBuilder extensions.
    /// </summary>
    public static partial class StringBuilderExtensions
    {
        /// <summary>
        /// Append String.Format + NewLine.  
        /// </summary>
        public static StringBuilder AppendFormatLine(this StringBuilder builder, string format, params object[] arguments)
        {
            return builder.AppendFormatLine(CultureInfo.CurrentCulture, format, arguments);
        }

        /// <summary>
        /// Append InvariantCulture String.Format + NewLine.  
        /// </summary>
        public static StringBuilder AppendFormatLineCI(this StringBuilder builder, string format, params object[] arguments)
        {
            return builder.AppendFormatLine(CultureInfo.InvariantCulture, format, arguments);
        }

        /// <summary>
        /// Append StringFormat with custom formatProvider + NewLine. 
        /// </summary>
        public static StringBuilder AppendFormatLine(this StringBuilder builder, IFormatProvider formatProvider, string format, params object[] arguments)
        {
            _ = builder ?? throw new ArgumentNullException(nameof(builder));

            builder.AppendFormat(formatProvider, format, arguments);
            builder.AppendLine();
            return builder;
        }
    }
}
