using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace NetWebScript.JsClr.TypeSystem
{
    public static class ScriptTypeHelper
    {

        public static void EnsureAllPublicMembers(this IScriptType scriptType)
        {
            var type = scriptType.Type;

            foreach (var ctor in type.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly))
            {
                scriptType.GetScriptConstructor(ctor);
            }

            foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly))
            {
                if (!method.IsGenericMethodDefinition)
                {
                    scriptType.GetScriptMethod(method);
                }
            }

            foreach (var field in type.GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public))
            {
                if (field.DeclaringType == type)
                {
                    scriptType.GetScriptField(field);
                }
            }

        }

    }
}
