using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetWebScript.Script
{
    [Globals, ScriptAvailable]
    public static class TypeSystemHelper
    {
        /// <summary>
        /// Create a type 
        /// </summary>
        /// <param name="name">Type name</param>
        /// <param name="baseType">Base type</param>
        /// <param name="interfaces">Implemented interfaces</param>
        /// <returns>The new type</returns>
        [ScriptBody(Body = @"function (n, b, i) {
var t = function () { }
t.$n = n;
if (b) { t.$b = b; t.prototype = new b; t.prototype.constructor = t; }
if (i) { t.prototype.$itfs = i; } 
return t;
}")]
        public static Type CreateType(string name, Type baseType, Type[] interfaces)
        {
            throw new PlatformNotSupportedException();
        }
    }
}
