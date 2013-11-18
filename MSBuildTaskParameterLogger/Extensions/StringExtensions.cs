using System.Collections.Generic;
using System.Linq;

namespace MSBuildTaskParameterLogger.Extensions
{
    static class StringExtensions
    {
        public static string JoinEnumerable(this IEnumerable<string> values, string separator)
        {
            return string.Join(separator, values);
        }

        public static string JoinEnumerable<T>(this IEnumerable<T> values, string separator)
        {
            return values.Select(item => item.ToString()).JoinEnumerable(separator);
        }
    }
}