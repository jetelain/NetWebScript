using System;
using System.Collections.Generic;
using System.Reflection;
using NetWebScript.JsClr.TypeSystem.Helped;
using NetWebScript.Metadata;

namespace NetWebScript.JsClr.TypeSystem
{
    internal sealed class EquivalentScriptTypeProvider : IScriptTypeProvider
    {
        private readonly ScriptSystem system;
        private readonly Dictionary<Type, Type> scriptEquivalent = new Dictionary<Type, Type>();

        public EquivalentScriptTypeProvider(ScriptSystem system)
        {
            this.system = system;
        }

        public bool TryCreate(Type type, out IScriptType scriptType)
        {
            Type helperType = GetEquivalent(type);
            if (helperType != null)
            {
                var helped = new ScriptTypeHelped(system, type, helperType);
                RegisterInMetadata(helped);
                scriptType = helped;
                return true;
            }
            scriptType = null;
            return false;
        }

        private void RegisterInMetadata(ScriptTypeHelped helped)
        {
            system.Metadata.Equivalents.Add(
                new EquivalentMetadata()
                {
                    CRef = CRefToolkit.GetCRef(helped.Type),
                    EquivalentCRef = CRefToolkit.GetCRef(helped.Equivalent.Type)
                });
        }

        private Type GetEquivalent(Type type)
        {
            Type helperType;
            if (scriptEquivalent.TryGetValue(type, out helperType))
            {
                return helperType;
            }
            if (type.IsGenericType)
            {
                if (scriptEquivalent.TryGetValue(type.GetGenericTypeDefinition(), out helperType))
                {
                    return helperType.MakeGenericType(type.GetGenericArguments());
                }
            }
            if (type.DeclaringType != null)
            {
                Type declaring = GetEquivalent(type.DeclaringType);
                if (declaring != null)
                {
                    if (type.IsGenericType)
                    {
                        return declaring.GetNestedType(type.Name).MakeGenericType(type.GetGenericArguments());
                    }
                    else
                    {
                        return declaring.GetNestedType(type.Name);
                    }
                }
            }

            if (typeof(MethodInfo).IsAssignableFrom(type) && type != typeof(MethodInfo))
            {
                return GetEquivalent(typeof(MethodInfo));
            }
            return null;
        }

        public void RegisterAssembly(System.Reflection.Assembly assembly)
        {
            foreach (Type type in assembly.GetTypes())
            {
                var attr = (ScriptEquivalentAttribute)Attribute.GetCustomAttribute(type, typeof(ScriptEquivalentAttribute), false);
                if (attr != null)
                {
                    if (scriptEquivalent.ContainsKey(attr.Type))
                    {
                        throw new Exception(string.Format("Type '{0}' has more than one equivalent : at least '{1}' and '{2}'", attr.Type, type.FullName, scriptEquivalent[attr.Type].FullName));
                    }
                    else
                    {
                        scriptEquivalent.Add(attr.Type, type);
                    }
                }
            }
        }
    }
}
