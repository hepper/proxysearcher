using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ProxySearch.Console.Code.Extensions
{
    public static class RegexExtensions
    {
        public static string ReplaceGroup(this Regex regex, string input, string groupName, string replacement)
        {
            return regex.Replace(input, m =>
            {
                string capture = m.Value.Remove(m.Groups[groupName].Index - m.Index, m.Groups[groupName].Length);
                return capture.Insert(m.Groups[groupName].Index - m.Index, replacement);
            });
        }
    }
}
