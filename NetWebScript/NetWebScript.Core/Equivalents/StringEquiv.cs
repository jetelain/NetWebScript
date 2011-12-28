
using NetWebScript.Script;
using System;
using System.Text;
using NetWebScript.Equivalents.Text;
namespace NetWebScript.Equivalents
{
    [ScriptAvailable]
    [ImportedExtender(typeof(JSString))]
    [ScriptEquivalent(typeof(System.String))]
    public sealed class StringEquiv
    {
        [ScriptBody(Inline="this")]
        private readonly JSString str;

        [ScriptBody(Inline = "this")]
        internal StringEquiv(JSString str)
        {
            this.str = str;
        }

        [ScriptBody(Inline = "a==b")]
        public static bool op_Equality(string a, string b)
        {
            return a == b;
        }

        [ScriptBody(Inline = "a!=b")]
        public static bool op_Inequality(string a, string b)
        {
            return a != b;
        }

        public string Substring(int index)
        {
            return str.Substr(index);
        }

        public string Substring(int index, int length)
        {
            return str.Substr(index,length);
        }

        public char get_Chars(int index)
        {
            return (char)str.CharCodeAt(index);
        }

        public int get_Length()
        {
            return str.Length;
        }

        public static bool IsNullOrEmpty(string str)
        {
            return str == null || str.Length == 0;
        }

        public static bool IsNullOrWhitespace(string str)
        {
            return str == null || str.Trim().Length == 0;
        }

        [ScriptBody(Inline = "''")]
        public static readonly string Empty; // Initialize to null to avoid static constructor

        public object Clone()
        {
            return str;
        }

        //public static int Compare(string strA, string strB);

        //public static int Compare(string strA, string strB, bool ignoreCase);

        //public static int Compare(string strA, string strB, StringComparison comparisonType);

        //public static int Compare(string strA, string strB, bool ignoreCase, CultureInfo culture);

        //public static int Compare(string strA, string strB, CultureInfo culture, CompareOptions options);

        //public static int Compare(string strA, int indexA, string strB, int indexB, int length);

        //public static int Compare(string strA, int indexA, string strB, int indexB, int length, bool ignoreCase);

        //public static int Compare(string strA, int indexA, string strB, int indexB, int length, StringComparison comparisonType);

        //public static int Compare(string strA, int indexA, string strB, int indexB, int length, bool ignoreCase, CultureInfo culture);

        //public static int Compare(string strA, int indexA, string strB, int indexB, int length, CultureInfo culture, CompareOptions options);

        public static int CompareOrdinal(string strA, string strB)
        {
            return ((strA == strB) ? 0 : (((JSString)strA > (JSString)strB) ? 1 : -1));
        }

        //public static int CompareOrdinal(string strA, int indexA, string strB, int indexB, int length);

        //public int CompareTo(object value);

        //public int CompareTo(string strB);

        //public static string Concat(IEnumerable<string> values);

        //public static string Concat<T>(IEnumerable<T> values);

        public static string Concat(object arg0)
        {
            return arg0 == null ? string.Empty : arg0.ToString();
        }

        public static string Concat(params object[] args)
        {
            return ((JSArray<object>)args).Join("");
        }

        public static string Concat(object arg0, object arg1)
        {
            return Concat(Concat(arg0), Concat(arg1));
        }

        public static string Concat(object arg0, object arg1, object arg2)
        {
            return Concat(Concat(arg0), Concat(arg1), Concat(arg2));
        }

        public static string Concat(object arg0, object arg1, object arg2, object arg3)
        {
            return Concat(Concat(arg0), Concat(arg1), Concat(arg2), Concat(arg3));
        }

        public static string Concat(params string[] values)
        {
            return ((JSArray<string>)values).Join("");
        }

        [ScriptBody(Inline = "str0+str1")]
        public static string Concat(string str0, string str1)
        {
            return string.Concat(str0, str1);
        }

        [ScriptBody(Inline = "str0+str1+str2")]
        public static string Concat(string str0, string str1, string str2)
        {
            return string.Concat(str0, str1, str2);
        }

        [ScriptBody(Inline = "str0+str1+str2+str3")]
        public static string Concat(string str0, string str1, string str2, string str3)
        {
            return string.Concat(str0, str1, str2, str3);
        }

        public bool Contains(string value)
        {
            return str.IndexOf(value) != -1;
        }

        public static string Copy(string str)
        {
            return str;
        }

        //public void CopyTo(int sourceIndex, char[] destination, int destinationIndex, int count);

        public bool EndsWith(string value)
        {
            return new JSRegExp(RegExpEscape(value)+"$").Test(str);
        }

        //public bool EndsWith(string value, StringComparison comparisonType);

        //public bool EndsWith(string value, bool ignoreCase, CultureInfo culture);

        //public override bool Equals(object obj);

        //public bool Equals(string value);

        public static bool Equals(string a, string b)
        {
            return op_Equality(a, b);
        }

        //public bool Equals(string value, StringComparison comparisonType);

        //public static bool Equals(string a, string b, StringComparison comparisonType);

        public static string Format(string format, object arg0)
        {
            return Format(null, format, new [] { arg0 });
        }

        public static string Format(string format, params object[] args)
        {
            return Format(null, format, args);
        }

        public static string Format(IFormatProvider provider, string format, params object[] args)
        {
            return new StringBuilderEquiv().AppendFormat(provider, format, args).ToString();
        }

        public static string Format(string format, object arg0, object arg1)
        {
            return Format(null, format, new [] { arg0, arg1 });
        }

        public static string Format(string format, object arg0, object arg1, object arg2)
        {
            return Format(null, format, new [] { arg0, arg1, arg2 });
        }

        //public CharEnumerator GetEnumerator();

        //public override int GetHashCode();

        //public TypeCode GetTypeCode();

        public int IndexOf(char value)
        {
            return str.IndexOf(JSString.FromCharCode(value));
        }

        public int IndexOf(string value)
        {
            return str.IndexOf(value);
        }

        public int IndexOf(char value, int startIndex)
        {
            return str.IndexOf(JSString.FromCharCode(value), startIndex);
        }

        public int IndexOf(string value, int startIndex)
        {
            return str.IndexOf(value, startIndex);
        }

        //public int IndexOf(string value, StringComparison comparisonType);

        public int IndexOf(char value, int startIndex, int count)
        {
            var idx = IndexOf(value, startIndex);
            if ( idx > startIndex + count)
            {
                return -1;
            }
            return idx;
        }

        public int IndexOf(string value, int startIndex, int count)
        {
            var idx = IndexOf(value, startIndex);
            if (idx > startIndex + count)
            {
                return -1;
            }
            return idx;
        }

        //public int IndexOf(string value, int startIndex, StringComparison comparisonType);

        //public int IndexOf(string value, int startIndex, int count, StringComparison comparisonType);

        public int IndexOfAny(char[] anyOf)
        {
            return str.Search(new JSRegExp(CharsRegexPatten(anyOf)));
        }

        public int IndexOfAny(char[] anyOf, int startIndex)
        {
            var idx = Substring(startIndex).IndexOfAny(anyOf);
            if (idx != -1)
            {
                return idx + startIndex;
            }
            return -1;
        }

        public int IndexOfAny(char[] anyOf, int startIndex, int count)
        {
            var idx = Substring(startIndex).IndexOfAny(anyOf);
            if (idx != -1 && idx < count)
            {
                return idx + startIndex;
            }
            return -1;
        }

        public string Insert(int startIndex, string value)
        {
            return Substring(0, startIndex) + value + Substring(startIndex);
        }

        //public static string Intern(string str);

        //public static string IsInterned(string str);

        //public bool IsNormalized();

        //public bool IsNormalized(NormalizationForm normalizationForm);

        //public static string Join(string separator, IEnumerable<string> values);

        //public static string Join<T>(string separator, IEnumerable<T> values);

        public static string Join(string separator, params object[] values)
        {
            return ((JSArray<object>)values).Join(separator);
        }

        public static string Join(string separator, params string[] values)
        {
            return ((JSArray<string>)values).Join(separator);
            
        }

        //spublic static string Join(string separator, string[] value, int startIndex, int count);

        public int LastIndexOf(char value)
        {
            return str.LastIndexOf(JSString.FromCharCode(value));
        }

        public int LastIndexOf(string value)
        {
            return str.LastIndexOf(value);
        }

        public int LastIndexOf(char value, int startIndex)
        {
            return str.LastIndexOf(JSString.FromCharCode(value), startIndex);
        }

        public int LastIndexOf(string value, int startIndex)
        {
            return str.LastIndexOf(value, startIndex);
        }

        //public int LastIndexOf(string value, StringComparison comparisonType);

        //public int LastIndexOf(char value, int startIndex, int count);

        //public int LastIndexOf(string value, int startIndex, int count);

        //public int LastIndexOf(string value, int startIndex, StringComparison comparisonType);

        //public int LastIndexOf(string value, int startIndex, int count, StringComparison comparisonType);

        //public int LastIndexOfAny(char[] anyOf);

        //public int LastIndexOfAny(char[] anyOf, int startIndex);

        //public int LastIndexOfAny(char[] anyOf, int startIndex, int count);

        //public string Normalize();

        //public string Normalize(NormalizationForm normalizationForm);

        public string PadLeft(int totalWidth)
        {
            return PadLeft(totalWidth, ' ');
        }

        public string PadLeft(int totalWidth, char paddingChar)
        {
            var pad = JSString.FromCharCode(paddingChar);
            var newstr = str;
            while (newstr.Length < totalWidth)
            {
                newstr = pad + newstr;
            }
            return newstr;
        }

        public string PadRight(int totalWidth)
        {
            return PadRight(totalWidth, ' ');
        }

        public string PadRight(int totalWidth, char paddingChar)
        {
            var pad = JSString.FromCharCode(paddingChar);
            var newstr = str;
            while (newstr.Length < totalWidth)
            {
                newstr = newstr + pad;
            }
            return newstr;
        }

        public string Remove(int startIndex)
        {
            return Substring(0, startIndex);
        }

        public string Remove(int startIndex, int count)
        {
            return Substring(0, startIndex) + Substring(startIndex + count);
        }

        public string Replace(char oldChar, char newChar)
        {
            return str.Replace(new JSRegExp(RegExpEscape(JSString.FromCharCode(oldChar)), "g"), JSString.FromCharCode(newChar));
        }

        public string Replace(string oldValue, string newValue)
        {
            return str.Replace(new JSRegExp(RegExpEscape(oldValue), "g"), newValue);
        }

        public string[] Split(params char[] separator)
        {
            return str.Split(new JSRegExp(CharsRegexPatten(separator), "g"));
        }

        public string[] Split(char[] separator, int count)
        {
            return str.Split(new JSRegExp(CharsRegexPatten(separator), "g"), count);
        }

        //public string[] Split(char[] separator, StringSplitOptions options);

        //public string[] Split(string[] separator, StringSplitOptions options);

        //public string[] Split(char[] separator, int count, StringSplitOptions options);

        //public string[] Split(string[] separator, int count, StringSplitOptions options);

        public bool StartsWith(string value)
        {
            return new JSRegExp("^" + RegExpEscape(value)).Test(str);
        }

        //public bool StartsWith(string value, StringComparison comparisonType);

        //public bool StartsWith(string value, bool ignoreCase, CultureInfo culture);

        //public char[] ToCharArray();

        //public char[] ToCharArray(int startIndex, int length);

        public string ToLower()
        {
            return str.TolocaleLowerCase();
        }

        //public string ToLower(CultureInfo culture);

        public string ToLowerInvariant()
        {
            return str.ToLowerCase();
        }

        public override string ToString()
        {
            return str;
        }

        //public string ToString(IFormatProvider provider);

        public string ToUpper()
        {
            return str.TolocaleUpperCase();
        }

        //public string ToUpper(CultureInfo culture);

        public string ToUpperInvariant()
        {
            return str.ToUpperCase();
        }

        public string Trim()
        {
            return TrimEnd().TrimStart();
        }

        public string Trim(params char[] trimChars)
        {
            return TrimEnd(trimChars).TrimStart(trimChars);
        }

        private static readonly JSRegExp RegexTrimEnd = new JSRegExp("\\s+$");
        private static readonly JSRegExp RegexTrimStart = new JSRegExp("^\\s+");

        public string TrimEnd(params char[] trimChars)
        {
            if (trimChars == null || trimChars.Length == 0)
            {
                return str.Replace(RegexTrimEnd, "");
            }
            var chars = ((JSArray<char>)trimChars).Join("");
            return str.Replace(new JSRegExp(CharsRegexPatten(trimChars) + "+$"), "");
        }

        public string TrimStart(params char[] trimChars)
        {
            if (trimChars == null || trimChars.Length == 0)
            {
                return str.Replace(RegexTrimStart, "");
            }
            return str.Replace(new JSRegExp("^" + CharsRegexPatten(trimChars) + "+"), "");
        }

        internal static string CharsRegexPatten(char[] trimChars)
        {
            string expr = "[";
            foreach (char c in trimChars)
            {
                expr = expr + JSString.FromCharCode(c);
            }
            return RegExpEscape(expr + "]");
        }

        internal static string RegExpEscape(JSString str)
        {
            return str.Replace(new JSRegExp("[-/\\\\^$*+?.()|[\\]{}]", "g"), "\\$&");
        }
    }
}
