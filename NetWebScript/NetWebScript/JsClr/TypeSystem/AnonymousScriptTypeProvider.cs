using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.JsClr.TypeSystem.Anonymous;

namespace NetWebScript.JsClr.TypeSystem
{
    class AnonymousScriptTypeProvider : IScriptTypeProvider
    {
        public bool TryCreate(Type type, out IScriptType scriptType)
        {
            var anonymous = (AnonymousObjectAttribute)Attribute.GetCustomAttribute(type, typeof(AnonymousObjectAttribute));
            if (anonymous != null)
            {
                scriptType = new AnonymousType(type, anonymous.Convention);
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
