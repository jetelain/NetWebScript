using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using NetWebScript.JsClr.TypeSystem.Invoker;

namespace NetWebScript.JsClr.TypeSystem
{
    public interface IScriptMethodBase
    {
        string ImplId { get; }

        MethodBase Method { get; }

        IScriptType Owner { get; }

        IMethodInvoker Invoker { get; }
    }
}
