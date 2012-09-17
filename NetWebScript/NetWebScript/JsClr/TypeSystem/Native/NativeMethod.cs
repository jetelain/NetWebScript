using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using NetWebScript.JsClr.TypeSystem.Invoker;

namespace NetWebScript.JsClr.TypeSystem.Native
{
    public class NativeMethod : MappedMethodBase, IScriptMethod
    {
        private readonly string slotId;
        private readonly string implId;

        public NativeMethod(IScriptType owner, MethodInfo method, string name) : base(owner, method)
        {
            if (method.IsVirtual)
            {
                slotId = name;
                implId = name;
            }
            else
            {
                implId = name;
            }
        }

        public string SlodId
        {
            get { return slotId; }
        }

        public override string ImplId
        {
            get { return implId; }
        }
        public bool InlineMethodCall
        {
            get { return false; }
        }

        public ScriptAst.MethodScriptAst Ast
        {
            get { return null; }
        }
    }
}
