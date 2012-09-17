using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using NetWebScript.JsClr.TypeSystem.Standard;

namespace NetWebScript.JsClr.TypeSystem
{
    internal sealed class StandardScriptTypeProvider : IScriptTypeProvider
    {
        private readonly ScriptSystem system;

        public StandardScriptTypeProvider(ScriptSystem system)
        {
            this.system = system;
        }

        public bool TryCreate(Type type, out IScriptType scriptType)
        {
            if (IsScriptAvailable(type))
            {
                var extender = (ImportedExtenderAttribute)Attribute.GetCustomAttribute(type, typeof(ImportedExtenderAttribute));
                if (extender != null)
                {
                    scriptType = new ScriptExtenderType(system, type, extender.ExtendedType);
                    return true;
                }
                scriptType = new ScriptType(system, type);
                return true;
            }
            scriptType = null;
            return false;
        }

        private bool IsScriptAvailable(Type type)
        {
            if (type.IsArray || type.IsEnum || type.IsValueType)
            {
                return false;
            }
            if (type.IsInterface)
            {
                if (ScriptType.HasGenericMethod(type))
                {
                    return false;
                }
                // All interfaces are implicitly script available
                return true;
            }
            if (Attribute.IsDefined(type, typeof(ScriptAvailableAttribute)) || Attribute.IsDefined(type.Assembly, typeof(ScriptAvailableAttribute)))
            {
                return true;
            }
            if (type.DeclaringType != null && IsScriptAvailable(type.DeclaringType))
            {
                return true;
            }
            if (type.IsGenericType && !type.IsGenericTypeDefinition && IsScriptAvailable(type.GetGenericTypeDefinition()))
            {
                return true;
            }
            return false;
        }

        public void RegisterAssembly(Assembly assembly)
        {

        }

    }
}
