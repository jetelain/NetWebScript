﻿using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Diagnostics.Contracts;
namespace NetWebScript.Script
{
    /// <summary>
    /// JavaScript string
    /// </summary>
    [Imported(Name="String", IgnoreNamespace=true)]
    public sealed class JSString
    {
        private readonly string data;

        private JSString(string data)
        {
            Contract.Requires(data != null);
            this.data = data;
        }

        [ScriptAlias("String")]
        public static JSString ToString(object obj)
        {
            return new JSString(obj.ToString());
        }

        public char CharAt(int index)
        {
            return data[index];
        }

        public int CharCodeAt(int index)
        {
            return (int)data[index];
        }

        public static string Concat(object o1, object o2)
        {
            return string.Concat(o1, o2);
        }

        public static string Concat(string s1, string s2)
        {
            return string.Concat(s1, s2);
        }

        public static string Concat(object o1, object o2, object o3)
        {
            return string.Concat(o1, o2, o3);
        }

        public static string Concat(string s1, string s2, string s3)
        {
            return string.Concat(s1, s2, s3);
        }

        public static string Concat(object o1, object o2, object o3, object o4)
        {
            return string.Concat(o1, o2, o3, o4);
        }

        public static string Concat(string s1, string s2, string s3, string s4)
        {
            return string.Concat(s1, s2, s3, s4);
        }

        public static string FromCharCode(int charCode)
        {
            return ((char)charCode).ToString();
        }

        public int IndexOf(string subString)
        {
            return data.IndexOf(subString);
        }

        public int IndexOf(string subString, int startIndex)
        {
            return data.IndexOf(subString, startIndex);
        }

        public int LastIndexOf(string subString)
        {
            return data.LastIndexOf(subString);
        }

        public int LastIndexOf(string subString, int startIndex)
        {
            return data.LastIndexOf(subString, startIndex);
        }

        public string[] Match(JSRegExp regex)
        {
            return regex.Exec(data);
        }

        public static bool operator ==(JSString s1, JSString s2)
        {
            return (string)s1 == (string)s2;
        }

        public static bool operator !=(JSString s1, JSString s2)
        {
            return (string)s1 != (string)s2;
        }

        public static bool operator ==(JSString s1, object s2)
        {
            return (string)s1 == (s2 as string);
        }

        public static bool operator !=(JSString s1, object s2)
        {
            return (string)s1 != (s2 as string);
        }

        public string Replace(JSRegExp regex, string replaceText)
        {
            if (regex.Global)
            {
                return regex.Expression.Replace(data, replaceText);
            }
            return regex.Expression.Replace(data, replaceText, 1);
        }

        public string Replace(JSRegExp regex, Func<string,string> callback)
        {
            MatchEvaluator evaluator = delegate(Match match) { return callback(match.Value); };
            if (regex.Global)
            {
                return regex.Expression.Replace(data, evaluator);
            }
            return regex.Expression.Replace(data, evaluator, 1);
        }

        public string Replace(string oldText, string replaceText)
        {
            int pos = data.IndexOf(oldText);
            if (pos != -1)
            {
                return data.Substring(0, pos) + replaceText + data.Substring(pos + oldText.Length);
            }
            return data;
        }

        public int Search(JSRegExp regex)
        {
            var match = regex.Expression.Match(data);
            if (match.Success)
            {
                return match.Index;
            }
            return -1;
        }

        public string[] Split(JSRegExp regex)
        {
            return regex.Expression.Split(data);
        }

        public string[] Split(string separator)
        {
            return data.Split(new[] { separator }, StringSplitOptions.None);
        }

        public string[] Split(JSRegExp regex, int limit)
        {
            return regex.Expression.Split(data, limit);
        }

        public string[] Split(string separator, int limit)
        {
            return data.Split(new [] { separator }, limit, StringSplitOptions.None);
        }

        public string Substr(int startIndex)
        {
            return data.Substring(startIndex);
        }

        public string Substr(int startIndex, int length)
        {
            return data.Substring(startIndex, length);
        }

        public string Substring(int startIndex, int endIndex)
        {
            return data.Substring(startIndex, endIndex - startIndex);
        }

        public string TolocaleLowerCase()
        {
            return data.ToLower(CultureInfo.CurrentCulture);
        }

        public string TolocaleUpperCase()
        {
            return data.ToUpper(CultureInfo.CurrentCulture);
        }

        public string ToLowerCase()
        {
            return data.ToLowerInvariant();
        }

        public string ToUpperCase()
        {
            return data.ToUpperInvariant();
        }

        [IntrinsicProperty]
        public int Length
        {
            get
            {
                return data.Length;
            }
        }

        [ScriptBody(Inline="str")]
        public static implicit operator JSString(string str)
        {
            if (str != null)
            {
                return new JSString(str);
            }
            return null;
        }

        [ScriptBody(Inline = "str")]
        public static implicit operator string(JSString str)
        {
            if (str != null)
            {
                return str.data;
            }
            return null;
        }

        [ImportedExtension]
        public override int GetHashCode()
        {
	        int hash = 0;
	        for (int i = 0; i < Length; i++) {
		        hash = ((hash<<5)-hash)+CharCodeAt(i);
		        hash = hash & hash; // Convert to 32bit integer
	        }
	        return hash;
        }

        [ImportedExtension]
        public override bool Equals(object obj)
        {
            return this == obj;
        }

        public override string ToString()
        {
            return data;
        }
    }
}

