using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace NetWebScript.Equivalents.Reflection
{
    [ScriptEquivalent(typeof(System.Reflection.MethodBase))]
    [ScriptAvailable]
    internal abstract class MethodBase
    {
        public abstract object Invoke(object target, object[] parameters);
    }
}
