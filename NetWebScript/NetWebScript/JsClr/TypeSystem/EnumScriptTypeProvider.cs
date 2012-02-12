using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.JsClr.TypeSystem.Standard;

namespace NetWebScript.JsClr.TypeSystem
{
    class EnumScriptTypeProvider : IScriptTypeProvider
    {
        private readonly ScriptSystem system;

        public EnumScriptTypeProvider(ScriptSystem system)
        {
            this.system = system;
        }

        public bool TryCreate(Type type, out IScriptType scriptType)
        {
            if (type.IsEnum)
            {
                // All enums are automaticly script avaiblable
                scriptType = new ScriptEnumType(system, type);
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
