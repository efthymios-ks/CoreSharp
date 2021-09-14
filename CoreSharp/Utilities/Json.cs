using System.Linq;
using System.Text.RegularExpressions;

namespace CoreSharp.Utilities
{
    public static class Json
    {
        /// <summary>
        /// Check if string is an empty json.
        /// </summary>
        public static bool IsEmpty(string json)
        {
            json ??= string.Empty;

            //Remove spaces, line-breaks and whitespace
            json = Regex.Replace(json, @"\s+", string.Empty);

            //Empty formats
            var emptyFormats = new[] { "", "{}", "[]", "[{}]" };

            return emptyFormats.Any(f => f == json);
        }
    }
}
