using NetWebScript.JsClr.TypeSystem.Invoker;
using System;

namespace NetWebScript.JsClr.ScriptAst
{
    public interface IInvocableField
    {
        string SlodId { get; }

        bool IsStatic { get; }

        IInvocableType DeclaringType { get; }

        IFieldInvoker Invoker { get; }

        string DisplayName { get; }
    }
}
