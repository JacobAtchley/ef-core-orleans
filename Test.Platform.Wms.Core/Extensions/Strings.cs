using System;
using System.Linq;

namespace Test.Platform.Wms.Core.Extensions
{
    public static class Strings
    {
        public static bool In(this string source, params string[] list)
        {
            return list.Any(x => x.EqualsIgnoreCaseAndWhitespace(source));
        }

        public static bool EqualsIgnoreCaseAndWhitespace(this string source, string compare)
        {
            if (source == null && compare == null)
            {
                return true;
            }

            if (source == null || compare == null)
            {
                return false;
            }

            return source.SafeTrim().Equals(compare.SafeTrim(), StringComparison.OrdinalIgnoreCase);
        }

        public static string SafeTrim(this string source)
        {
            return string.IsNullOrEmpty(source) ? source : source.Trim();
        }
    }
}