using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.Script;
using System.ComponentModel;

namespace NetWebScript.Equivalents.Reflection
{
    [ScriptEquivalent(typeof(System.Reflection.MethodInfo))]
    [ScriptAvailable]
    public class MethodInfo : MethodBase
    {
        private readonly JSFunction method;

        internal MethodInfo(JSFunction method)
        {
            this.method = method;
        }

        public override object Invoke(object target, object[] parameters)
        {
            return method.Apply(target, parameters);
        }

        [ScriptBody(Inline = "method")]
        public static implicit operator MethodInfo(System.Reflection.MethodInfo method)
        {
            if (method != null)
            {
                return new MethodInfo(new JSFunction(method));
            }
            return null;
        }
    }
}
