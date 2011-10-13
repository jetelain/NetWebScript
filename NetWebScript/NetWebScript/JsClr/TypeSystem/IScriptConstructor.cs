using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.JsClr.TypeSystem.Invoker;

namespace NetWebScript.JsClr.TypeSystem
{
    public interface IScriptConstructor : IScriptMethodBase
    {
        IObjectCreationInvoker CreationInvoker { get; }
    }
}
