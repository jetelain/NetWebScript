using NetWebScript.JsClr.ScriptAst;

namespace NetWebScript.JsClr.ScriptWriter.Declaration
{
    /// <summary>
    /// Field declaration
    /// </summary>
    public interface IScriptFieldDeclaration : IScriptMemberDeclaration
    {
        /// <summary>
        /// Slot identifier (JavaScript-side member name reserved for field)
        /// </summary>
        string SlodId { get; }

        /// <summary>
        /// Initial value of field
        /// </summary>
        ScriptLiteralExpression InitialValue { get; }
    }
}
