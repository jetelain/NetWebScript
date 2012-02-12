using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.Remoting;

namespace NetWebScript.JsClr.TypeSystem.Remoting
{
    class ServerSideTypeProvider : IScriptTypeProvider
    {
        private readonly ScriptSystem system;

        public ServerSideTypeProvider(ScriptSystem system)
        {
            this.system = system;
        }

        public bool TryCreate(Type type, out IScriptType scriptType)
        {
            if ( Attribute.IsDefined(type, typeof(ServerSideAttribute)))
            {
                if (type.IsAbstract)
                {
                    throw new Exception(string.Format("Type '{0}' : A type cannot be marked with ServerSide and be abstract.", type.FullName));
                }
                scriptType = new ServerSideType(system, type);
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
