using System.IO;
using NetWebScript.JsClr.ScriptWriter.Declaration;

namespace NetWebScript.JsClr.ScriptWriter.Declaration
{
    public interface IScriptTypeStandardDeclarationWriter
    {
        void WriteDeclaration(TextWriter writer, WriterContext context, IScriptTypeDeclaration type);
    }
}
