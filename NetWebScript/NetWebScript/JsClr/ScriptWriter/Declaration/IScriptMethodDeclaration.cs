
namespace NetWebScript.JsClr.ScriptWriter.Declaration
{
    public interface IScriptMethodDeclaration : IScriptMethodBaseDeclaration
    {
        bool IsGlobal { get; }

        /// <summary>
        /// Method virtual slot (JavaScript-side member name reserved for method virtual slot)
        /// </summary>
        /// <remarks>
        /// null for non virtual methods
        /// </remarks>
        string SlodId { get; }
    }
}
