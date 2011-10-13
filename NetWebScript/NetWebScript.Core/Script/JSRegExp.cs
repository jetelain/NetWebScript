using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace NetWebScript.Script
{
    /// <summary>
    /// JavaScript regular expression
    /// </summary>
    [Imported(Name="RegExp"), IgnoreNamespace]
    public sealed class JSRegExp
    {
        private readonly Regex regex;
        private readonly bool isGlobal;

        public JSRegExp(string pattern)
        {
            regex = new Regex(pattern, RegexOptions.ECMAScript);
        }

        public JSRegExp(string pattern, string flags)
        {
            RegexOptions options = RegexOptions.ECMAScript;
            if (flags.IndexOf('g') != -1)
            {
                isGlobal = true;
            }
            if (flags.IndexOf('i') != -1)
            {
                options |= RegexOptions.IgnoreCase;
            }
            if (flags.IndexOf('m') != -1)
            {
                options |= RegexOptions.Multiline;
            }
            regex = new Regex(pattern, options);
        }

        public string[] Exec(string s)
        {
            return regex.Matches(s).Cast<Match>().Select(m => m.Value).ToArray();
        }

        public bool Test(string s)
        {
            return regex.IsMatch(s);
        }

        [IntrinsicProperty]
        public bool Global
        {
            get
            {
                return isGlobal;
            }
        }

        [IntrinsicProperty]
        public bool IgnoreCase
        {
            get
            {
                return (regex.Options & RegexOptions.IgnoreCase) == RegexOptions.IgnoreCase;
            }
        }

        [IntrinsicProperty]
        public bool Multiline
        {
            get
            {
                return (regex.Options & RegexOptions.Multiline) == RegexOptions.Multiline;
            }
        }

        [IntrinsicProperty]
        public string Pattern
        {
            get
            {
                return regex.ToString();
            }
        }

        internal Regex Expression
        {
            get { return regex; }
        }
    }
}
