using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.JsClr.TypeSystem.Imported;

namespace NetWebScript.JsClr.TypeSystem
{
    class ImportedScriptTypeProvider : IScriptTypeProvider
    {        
        private readonly ScriptSystem system;

        public ImportedScriptTypeProvider(ScriptSystem system)
        {
            this.system = system;
        }

        public bool TryCreate(Type type, out IScriptType scriptType)
        {
            var imported = (ImportedAttribute)Attribute.GetCustomAttribute(type, typeof(ImportedAttribute), false);
            if (imported != null)
            {
                scriptType = new ImportedType(system, type, imported);
                return true;
            }
            scriptType = null;
            return false;
        }

        public void RegisterAssembly(System.Reflection.Assembly assembly)
        {

        }
    }
}
