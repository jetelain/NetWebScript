using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetWebScript.Equivalents
{
    [ScriptAvailable]
    public static class ObjectHelper
    {
        // On some browser (IE) all objects does nto inherits from Object and it's not possible to change Object.prototype
        // This class provide a "workaround" for GetHashCode and Equals.

        // FIXME: implement $ghc (and $eq) on Number, String and Regex to have correct behaviour

        [ScriptBody(Body = "(function(){var c=0;return function(o){if(o.$hc !== undefined)return o.$hc;return o.$hc = ++c;};})()")]
        public static int DefaultGetHashCode(this object obj)
        {
            return obj.GetHashCode();
        }

        [ScriptBody(Body = "function(a,b){return a==b;}")]
        public static bool DefaultEquals(this object obj, object other)
        {
            return obj.Equals(other);
        }

        [ScriptBody(Inline = "obj.$ghc")]
        private static bool HasGetHashCode(this object obj)
        {
            return true;
        }

        [ScriptBody(Inline = "obj.$eq")]
        private static bool HasEquals(this object obj)
        {
            return true;
        }

        [ScriptBody(Inline = "obj.$ghc()")]
        private static int InvokeGetHashCode(this object obj)
        {
            return obj.GetHashCode();
        }

        [ScriptBody(Inline = "obj.$eq(other)")]
        private static bool InvokeEquals(this object obj, object other)
        {
            return obj.Equals(other);
        }

        public static int SafeGetHashCode(this object obj)
        {
            if (HasGetHashCode(obj))
            {
                return InvokeGetHashCode(obj);
            }
            return DefaultGetHashCode(obj);
        }

        public static bool SafeEquals(this object obj, object other)
        {
            if (HasEquals(obj))
            {
                return InvokeEquals(obj, other);
            }
            return DefaultEquals(obj, other);
        }


        public new static bool Equals(object a, object b)
        {
            if (a == b)
            {
                return true;
            }
            else if (a != null && b != null)
            {
                return SafeEquals(a, b);
            }
            return false;
        }

        [ScriptBody(Inline = "obj.constructor")]
        private static Type GetType(this object obj)
        {
            return obj.GetType();
        }
        
        [ScriptBody(Body="function(o){var c=new (o.constructor||Object)();for(var n in o){if(c[n]!==o[n])c[n]=o[n];}return c;}")]
        public static object MemberwiseClone(object obj)
        {
            throw new PlatformNotSupportedException();
        }

    }
}
