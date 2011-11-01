using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace NetWebScript.Script
{
    [ScriptAvailable]
    public static class RuntimeHelper
    {
        [ScriptBody(Inline = "obj.$itfs||null")]
        private static JSFunction[] GetImplementedInterfaces(object obj)
        {
            throw new PlatformNotSupportedException();
        }

        [ScriptBody(Body = "function(t,f) { return function() { return f.apply(t, arguments); }; }")]
        public static Delegate CreateDelegate(object target, JSFunction function)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Operate a safe cast. Implements the "isinst" IL operation.
        /// </summary>
        /// <param name="obj">Value to cast</param>
        /// <param name="type">Expected type</param>
        /// <returns>Value if it is of excepected type, null otherwise</returns>
        [DebuggerHidden]
        public static object As(object obj, JSFunction type)
        {
            if (obj != null) 
            {
                if (JSObject.IsInstanceOf(obj, type)) 
                {
                    return obj;
                }
                var itfs = GetImplementedInterfaces(obj);
                if (itfs != null) 
                {
                    for (var i = 0; i < itfs.Length; ++i)
                    {
                        if (itfs[i] == type)
                        {
                            return obj;
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Operate a cast. Implements the "classcast" IL operation.
        /// Rise an exception if value is not of the expected type.
        /// </summary>
        /// <param name="obj">Value to cast</param>
        /// <param name="type">Expected type</param>
        /// <returns>Value if it is of excepected type.</returns>
        [DebuggerHidden]
        public static object Cast(object obj, JSFunction type)
        {
            if (obj != null)
            {
                object value = As(obj, type);
                if (value == null)
                {
                    throw new Exception("Could not cast.");
                }
                return value;
            }
            return null;
        }

        /// <summary>
        /// Initialize an array of a given size filled with an initial value.
        /// </summary>
        /// <param name="size"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [DebuggerHidden]
        public static JSArray<object> CreateArray(int size, object value)
        {
            var count = size;
            var array = new JSArray<object>();
            while (count > 0) { array.Push(value); count--; }
            return array;
        }
    }
}
