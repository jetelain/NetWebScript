using System.Reflection;
using NetWebScript.JsClr.ScriptAst;

namespace NetWebScript.JsClr.TypeSystem
{
    public interface IScriptMethodBase : IInvocableMethodBase
    {
        MethodBase Method { get; }

        IScriptType OwnerScriptType { get; }
    }
}
