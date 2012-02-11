using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.JsClr.TypeSystem.Native;
using NetWebScript.Remoting;
using NetWebScript.JsClr.TypeSystem.Remoting;

namespace NetWebScript.JsClr.TypeSystem
{
    class NativeScriptTypeProvider : IScriptTypeProvider
    {
        private readonly ScriptSystem system;

        public NativeScriptTypeProvider(ScriptSystem system)
        {
            this.system = system;
        }

        public bool TryCreate(Type type, out IScriptType scriptType)
        {
            if (type.IsArray)
            {
                if (system.GetScriptType(type.GetElementType()) != null)
                {
                    scriptType = new ArrayType(system, type);
                }
                else
                {
                    scriptType = null;
                }
                return true;
            }
            if (!type.IsValueType)
            {
                if (type == typeof(object))
                {
                    scriptType = new ObjectType(system);
                    return true;
                }
                if (typeof(Delegate).IsAssignableFrom(type))
                {
                    scriptType = new DelegateType(system, type);
                    return true;
                }
                if (typeof(Type).IsAssignableFrom(type))
                {
                    scriptType = new FunctionType(system, type);
                    return true;
                }
                if (type == typeof(ScriptTransparentProxy))
                {
                    scriptType = new ScriptTransparentProxyType(system);
                    return true;
                }
            }
            scriptType = null;
            return false;
        }

        public void RegisterAssembly(System.Reflection.Assembly assembly)
        {

        }
    }
}
