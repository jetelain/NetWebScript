using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetWebScript.Equivalents
{
    [ScriptEquivalent(typeof(System.Array)), ScriptAvailable]
    internal static class Array
    {
        [ScriptBody(Inline = "array.length")]
        public static int get_Length(System.Array array)
        {
            return array.Length;
        }
    }
}
