using System.IO;

namespace NetWebScript.JsClr.ScriptWriter.Declaration
{
    public interface IScriptTypeDeclarationWriter
    {
        string PrettyName { get; }

        bool IsEmpty { get; }
        
        void WriteDeclaration(TextWriter writer, WriterContext context);
    }
}
