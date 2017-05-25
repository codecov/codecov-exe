using System.Text.RegularExpressions;

namespace Codecov.Utilities
{
    internal static class Extensions
    {
        internal static string RemoveAllWhiteSpace(this string str) => string.IsNullOrWhiteSpace(str) ? string.Empty : Regex.Replace(str.Trim(), @"\s+", string.Empty);
    }
}
