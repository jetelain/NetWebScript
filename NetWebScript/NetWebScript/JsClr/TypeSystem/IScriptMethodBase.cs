using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using NetWebScript.JsClr.TypeSystem.Invoker;
using NetWebScript.JsClr.ScriptAst;

namespace NetWebScript.JsClr.TypeSystem
{
    public interface IScriptMethodBase : IInvocableMethodBase
    {
        MethodBase Method { get; }

        IScriptType Owner { get; }
    }
}
