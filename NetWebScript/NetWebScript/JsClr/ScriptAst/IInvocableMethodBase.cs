using NetWebScript.JsClr.TypeSystem.Invoker;

namespace NetWebScript.JsClr.ScriptAst
{
    public interface IInvocableMethodBase
    {
        string ImplId { get; }

        bool IsStatic { get; }

        IInvocableType Owner { get; }

        IMethodInvoker Invoker { get; }

        string DisplayName { get; }

        bool IsVirtual { get; }
    }
}
