using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace NetWebScript.JsClr.TypeSystem.Standard
{
    class ScriptMethod : ScriptMethodBase, IScriptMethod
    {
        private readonly string vslot;

        internal ScriptMethod(ScriptSystem system, ScriptType owner, MethodInfo method, string body)
            : this(system, owner, method, body, owner.IsGlobals)
        {
        }

        internal ScriptMethod(ScriptSystem system, IScriptType owner, MethodInfo method, string body, bool isGlobal)
            : base(system, owner, method, body, isGlobal)
        {
            if (method.IsVirtual)
            {
                var baseMethod = method.GetBaseDefinition();
                if (baseMethod == method || baseMethod == null)
                {
                    vslot = system.CreateSplotId();
                }
                else
                {
                    vslot = system.GetSlotId(baseMethod);
                    if (vslot == null)
                    {
                        throw new Exception();
                    }
                }
            }           
        }

        /// <summary>
        /// Identifier of method slot (name to use for a virtual call of method)
        /// </summary>
        public string SlodId
        {
            get { return vslot; }
        }

    }
}
