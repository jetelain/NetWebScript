using NetWebScript.JsClr.TypeSystem.Invoker;
using System;

namespace NetWebScript.JsClr.ScriptAst
{
    public interface IInvocableConstructor : IInvocableMethodBase
    {
        IObjectCreationInvoker CreationInvoker { get; }
    }
}
