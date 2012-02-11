using System.Reflection;
using NetWebScript.JsClr.ScriptAst;
using NetWebScript.JsClr.TypeSystem.Invoker;

namespace NetWebScript.JsClr.TypeSystem
{
    public interface IScriptField : IInvocableField
    {
        FieldInfo Field { get; }

        IScriptType Owner { get; }
    }
}
