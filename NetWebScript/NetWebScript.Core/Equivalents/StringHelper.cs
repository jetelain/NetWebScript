
namespace NetWebScript.Equivalents
{
    [ScriptAvailable]
    public static class StringHelper
    {
        [ScriptBody(Inline = "a==b")]
        internal static bool op_Equality(string a, string b)
        {
            return a == b;
        }

        [ScriptBody(Inline = "a!=b")]
        internal static bool op_Inequality(string a, string b)
        {
            return a != b;
        }

        [ScriptBody(Inline = "str.substr(index)")]
        internal static string Substring(string str, int index)
        {
            return str.Substring(index);
        }

        [ScriptBody(Inline = "str.substr(index,length)")]
        internal static string Substring(string str, int index, int length)
        {
            return str.Substring(index,length);
        }

        [ScriptBody(Inline = "str.charCodeAt(index)")]
        internal static char get_Chars(string str, int index)
        {
            return str[index];
        }

        [ScriptBody(Inline = "str.length")]
        internal static int get_Length(string str)
        {
            return str.Length;
        }

        [ScriptBody(Inline = "str.indexOf(String.fromCharCode(c))")]
        internal static int IndexOf(string str, char c)
        {
            return str.IndexOf(str, c);
        }

        internal static bool IsNullOrEmpty(string str)
        {
            return str == null || str.Length == 0;
        }

        [ScriptBody(Inline = "''")]
        internal static readonly string Empty; // Initialize to null to avoid static constructor

        [ScriptBody(Inline = "values.join('')")]
        internal static string Concat(params string[] values)
        {
            return string.Concat(values);
        }

        [ScriptBody(Inline = "str0+str1")]
        internal static string Concat(string str0, string str1)
        {
            return string.Concat(str0, str1);
        }

        [ScriptBody(Inline = "str0+str1+str2")]
        internal static string Concat(string str0, string str1, string str2)
        {
            return string.Concat(str0, str1, str2);
        }

        [ScriptBody(Inline = "str0+str1+str2+str3")]
        internal static string Concat(string str0, string str1, string str2, string str3)
        {
            return string.Concat(str0, str1, str2, str3);
        }

        [ScriptBody(Inline = "str0+str1+str2")] // FIXME: may not work as expected
        internal static string Concat(object str0, object str1, object str2)
        {
            return string.Concat(str0, str1, str2);
        }


        [ScriptBody(Inline = "values.join('')")]// FIXME: may not work as expected
        internal static string Concat(params object[] values)
        {
            return string.Concat(values);
        }

        [ScriptBody(Inline = "$.trim(str)")]
        internal static string Trim(string str)
        {
            return str.Trim();
        }
    }
}
