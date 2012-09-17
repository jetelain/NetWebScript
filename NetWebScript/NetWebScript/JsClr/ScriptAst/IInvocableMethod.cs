
namespace NetWebScript.JsClr.ScriptAst
{
    /// <summary>
    /// Method that can be called within script
    /// </summary>
    public interface IInvocableMethod : IInvocableMethodBase
    {
        /// <summary>
        /// Virtual slot of method, if method is virtual, null otherwise
        /// </summary>
        string SlodId { get; }

        /// <summary>
        /// Specify if call must inline method call using <see cref="Ast"/>.
        /// </summary>
        bool InlineMethodCall { get; }

        /// <summary>
        /// If <see cref="InlineMethodCall"/>, method body to inline
        /// </summary>
        MethodScriptAst Ast { get; }
    }
}
