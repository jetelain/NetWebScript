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
        private readonly HashSet<Type> implicitScriptAvailable = new HashSet<Type>();

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
            if (Attribute.IsDefined(type, typeof(ScriptAvailableAttribute)) || Attribute.IsDefined(type.Assembly, typeof(ScriptAvailableAttribute)))
            {
                return true;
            }
            if (type.DeclaringType != null && IsScriptAvailable(type.DeclaringType))
            {
                return true;
            }
            if (implicitScriptAvailable.Contains(type))
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
            foreach (ForceScriptAvailableAttribute force in Attribute.GetCustomAttributes(assembly, typeof(ForceScriptAvailableAttribute)))
            {
                implicitScriptAvailable.Add(force.Type);
            }
        }

    }
}
