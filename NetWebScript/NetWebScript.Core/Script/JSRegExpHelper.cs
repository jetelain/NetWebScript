using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetWebScript.Script
{
    [ScriptAvailable]
    public static class JSRegExpHelper
	{
		/// <summary>
		/// Replace regular expression special characters by the corresonding escape sequence.
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static string Escape(JSString str)
		{
			return str.Replace(new JSRegExp("[-/\\\\^$*+?.()|[\\]{}]", "g"), "\\$&");
		}

		[ScriptBody(Inline="str.replace(regex,replaceText)")]
		public static string Replace(this string str, JSRegExp regex, string replaceText)
		{
			return ((JSString)str).Replace(regex, replaceText);
		}

	}
}
