using System;
using System.Text;
using System.Text.RegularExpressions;

namespace CoreSharp.Utilities
{
    public static class Url
    {
        /// <summary>
        /// Combines url segments.
        /// "/sec1", "/sec2/", "sec3" results to "/sec1/sec2/sec3/". 
        /// </summary> 
        public static string Combine(params object[] segments)
        {
            _ = segments ?? throw new ArgumentNullException(nameof(segments));

            var builder = new StringBuilder();

            //Connect 
            foreach (var segment in segments)
            {
                var trimmed = $"/{segment}".Trim();
                builder.Append(trimmed);
            }
            builder.Append("/");

            //Build url 
            var url = builder.ToString();

            //Multiple forward-slashes to single one 
            url = Regex.Replace(url, @"\/+", @"/");

            return url;
        }
    }
}
