using NetWebScript.JsClr.ScriptAst;
using NetWebScript.Metadata;

namespace NetWebScript.JsClr.ScriptWriter.Declaration
{
    /// <summary>
    /// Method or constructor definition
    /// </summary>
    public interface IScriptMethodBaseDeclaration : IScriptMemberDeclaration
    {
        /// <summary>
        /// Method implementation identifier (JavaScript-side member name reserved for method)
        /// </summary>
        /// <remarks>
        /// Abstract methods should not be declared, and thus this property should not be null when object is get throw <see cref="IScriptTypeDeclaration"/>.
        /// </remarks>
        string ImplId { get; }

        /// <summary>
        /// Method have a plain JavaScript implementation
        /// </summary>
        bool HasNativeBody { get;}

        /// <summary>
        /// Method plain JavaScript implementation
        /// </summary>
        string NativeBody { get; }

        /// <summary>
        /// Method implementation abstract syntax tree.
        /// </summary>
        /// <remarks>
        /// null if <see cref="HasNativeBody" /> is true, non-null otherwise.
        /// </remarks>
        MethodScriptAst Ast { get; }

        /// <summary>
        /// Method metadata
        /// </summary>
        MethodBaseMetadata Metadata { get; }
    }
}
