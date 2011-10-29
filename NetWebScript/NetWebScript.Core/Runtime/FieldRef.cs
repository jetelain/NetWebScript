using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.Script;
using System.Diagnostics;

namespace NetWebScript.Runtime
{
    /// <summary>
    /// Create a reference to a field of an object.
    /// </summary>
    [ScriptAvailable]
    public sealed class FieldRef : IRef
    {
        private readonly JSObject obj;
        private readonly string prop;

        /// <summary>
        /// Create a reference to a field of an object
        /// </summary>
        /// <param name="obj">Object</param>
        /// <param name="prop">Native slot name</param>
        [DebuggerHidden]
        public FieldRef(JSObject obj, string prop)
        {
            this.obj = obj;
            this.prop = prop;
        }

        /// <summary>
        /// Sets the value
        /// </summary>
        /// <param name="value">Value</param>
        [DebuggerHidden]
        public object Set(object value)
        {
            return obj[prop] = value;
        }

        /// <summary>
        /// Gets the value
        /// </summary>
        /// <returns>Value</returns>
        [DebuggerHidden]
        public object Get()
        {
            return obj[prop];
        }
    }
}
