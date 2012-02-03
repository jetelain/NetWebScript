using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using NetWebScript.JsClr.ScriptWriter.Declaration;

namespace NetWebScript.JsClr.TypeSystem.Standard
{
    class ScriptMethod : ScriptMethodBase, IScriptMethod, IScriptMethodDeclaration
    {
        private readonly string vslot;
        private readonly bool isGlobal;

        internal ScriptMethod(ScriptSystem system, ScriptType owner, MethodInfo method, string body, string exportedName)
            : this(system, owner, method, body, exportedName, owner.IsGlobals)
        {
        }

        internal ScriptMethod(ScriptSystem system, IScriptType owner, MethodInfo method, string body, string exportedName, bool isGlobal)
            : base(system, owner, method, body, isGlobal)
        {
            this.isGlobal = isGlobal;

            if (method.IsVirtual)
            {
                var baseMethod = method.GetBaseDefinition();
                if (baseMethod == method || baseMethod == null)
                {
                    if (exportedName != null)
                    {
                        vslot = exportedName;
                    }
                    else //if ( !method.IsFinal )
                    {
                        vslot = system.CreateSplotId();
                    }
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
            else if ( exportedName != null )
            {
                vslot = exportedName;
            }
        }

        /// <summary>
        /// Identifier of method slot (name to use for a virtual call of method)
        /// </summary>
        public string SlodId
        {
            get { return vslot; }
        }


        public bool IsGlobal
        {
            get { return isGlobal; }
        }
    }
}
