using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FromAncientGreeksToModernGeeks
{
    public static class ExtensionMethods
    {
        public static string ReplaceRegex(this string input, string pattern, string replacement) => 
            Regex.Replace(input, pattern, replacement);
    }
}
