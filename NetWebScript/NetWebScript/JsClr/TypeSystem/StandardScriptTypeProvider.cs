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
        private readonly List<ScriptType> typesToWrite = new List<ScriptType>();

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
                    var scriptExtType = new ScriptExtenderType(system, type, extender.ExtendedType);
                    typesToWrite.Add(scriptExtType);
                    scriptType = scriptExtType;
                    return true;
                }
                var scriptStdType = new ScriptType(system, type);
                typesToWrite.Add(scriptStdType);
                scriptType = scriptStdType;
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

        internal List<ScriptType> TypesToWrite
        {
            get { return typesToWrite; }
        }

    }
}
