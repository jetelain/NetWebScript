
namespace NetWebScript.JsClr.ScriptWriter.Declaration
{
    /// <summary>
    /// Member declaration
    /// </summary>
    public interface IScriptMemberDeclaration
    {
        /// <summary>
        /// User freindly name of member (used for errors and pretty print comments)
        /// </summary>
        string PrettyName { get; }

        /// <summary>
        /// Static member
        /// </summary>
        bool IsStatic { get; }
    }
}
