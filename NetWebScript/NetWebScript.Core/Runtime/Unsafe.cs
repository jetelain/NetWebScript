using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript;
using System.Globalization;
using System.Diagnostics.Contracts;
using NetWebScript.Remoting.Serialization;

namespace NetWebScript.Script
{
    [ScriptAvailable]
    public static class Unsafe
    {
        [ScriptBody(Body = "function(o){var a=[];for(var k in o){if(!$.isFunction(o[k]))a.push(k);}return a;}")]
        public static string[] GetFields(object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }
            Contract.EndContractBlock();
            JSObject scriptObject = obj as JSObject;
            if (scriptObject != null)
            {
                return scriptObject.data.Keys.ToArray();
            }
            else
            {
                return MetadataProvider.Current.GetTypeMapping(obj.GetType()).Keys.ToArray();
            }
        }


        [ScriptBody(Body = "function(o){var a=[];for(var k in o){a.push(k);}return a;}")]
        public static string[] GetAll(object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }
            Contract.EndContractBlock();
            return GetFields(obj);
        }

        [ScriptBody(Body = "function(o){if (o.constructor && o.constructor.$n) return o.constructor.$n; return typeof o;}")]
        public static string GetScriptTypeName(object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }
            Contract.EndContractBlock();
            return MetadataProvider.Current.GetScriptTypeName(obj.GetType());
        }

        [ScriptBody(Inline = "obj.toString()")]
        public static string NumberToString(object obj)
        {
            return Convert.ToDouble(obj).ToString(CultureInfo.InvariantCulture);
        }

        [ScriptBody(Inline = "obj.toString()")]
        public static string NumberToString(int obj)
        {
            return obj.ToString(CultureInfo.InvariantCulture);
        }
    }
}
