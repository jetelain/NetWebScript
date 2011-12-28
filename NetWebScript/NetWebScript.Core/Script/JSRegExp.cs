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
    [Imported(Name = "RegExp", IgnoreNamespace = true)]
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

        public ExecResult Exec(string s)
        {
            Match match = regex.Match(s, LastIndex);
            if (match.Success)
            {
                if (isGlobal)
                {
                    LastIndex = match.Index+match.Length;
                }
                return new ExecResult(match);
            }
            return null;
        }

        [Imported]
        public sealed class ExecResult
        {
            private readonly Match match;

            internal ExecResult(Match match)
            {
                this.match = match;
            }

            [IntrinsicProperty]
            public int Index
            {
                get
                {
                    return match.Index;
                }
            }

            [IntrinsicProperty]
            public int Length
            {
                get
                {
                    return match.Groups.Count;
                }
            }

            [IntrinsicProperty]
            public string this[int index]
            {
                get
                {
                    return match.Groups[index].Value;
                }
            }

        }

        public bool Test(string s)
        {
            Match match = regex.Match(s, LastIndex);
            if (match.Success)
            {
                if (isGlobal)
                {
                    LastIndex = match.Index + match.Length;
                }
                return true;
            }
            return false;
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


        [IntrinsicProperty]
        public int LastIndex
        {
            get;
            set;
        }
    }
}
