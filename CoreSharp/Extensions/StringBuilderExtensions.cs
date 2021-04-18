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
        public static StringBuilder AppendFormatLine(this StringBuilder builder, string format, params object[] parameters)
        {
            return builder.AppendFormatLine(CultureInfo.CurrentCulture, format, parameters);
        }

        /// <summary>
        /// Append InvariantCulture String.Format + NewLine.  
        /// </summary>
        public static StringBuilder AppendFormatLineCI(this StringBuilder builder, string format, params object[] parameters)
        {
            return builder.AppendFormatLine(CultureInfo.InvariantCulture, format, parameters);
        }

        /// <summary>
        /// Append StringFormat with custom formatProvider + NewLine. 
        /// </summary>
        public static StringBuilder AppendFormatLine(this StringBuilder builder, IFormatProvider formatProvider, string format, params object[] parameters)
        {
            builder = builder ?? throw new ArgumentNullException(nameof(builder));
            formatProvider = formatProvider ?? throw new ArgumentNullException(nameof(formatProvider));
            parameters = parameters ?? throw new ArgumentNullException(nameof(parameters));

            builder.AppendFormat(formatProvider, format, parameters);
            builder.AppendLine();
            return builder;
        }
    }
}
