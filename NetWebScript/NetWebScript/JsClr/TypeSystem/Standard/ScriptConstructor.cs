using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.JsClr.TypeSystem.Invoker;
using System.Reflection;

namespace NetWebScript.JsClr.TypeSystem.Standard
{
    class ScriptConstructor : ScriptMethodBase, IScriptConstructor
    {
        internal ScriptConstructor(ScriptSystem system, ScriptType owner, ConstructorInfo method)
            : base(system,owner,method,null)
        {
        }

        public IObjectCreationInvoker CreationInvoker
        {
            get { return StandardInvoker.Instance; }
        }

    }
}
