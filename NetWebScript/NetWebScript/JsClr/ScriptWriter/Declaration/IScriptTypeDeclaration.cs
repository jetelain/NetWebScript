using System.Collections.Generic;

namespace NetWebScript.JsClr.ScriptWriter.Declaration
{
    public interface IScriptTypeDeclaration
    {
        bool CreateType { get; }

        string PrettyName { get; }

        string TypeId { get; }

        string BaseTypeId { get; }

        IEnumerable<string> InterfacesTypeIds { get; }

        IEnumerable<IScriptMethodBaseDeclaration> Constructors { get; }

        IEnumerable<IScriptMethodDeclaration> Methods { get; }

        IEnumerable<IScriptFieldDeclaration> Fields { get; }

        IEnumerable<ScriptSlotImplementation> InterfacesMapping { get; }
    }
}
