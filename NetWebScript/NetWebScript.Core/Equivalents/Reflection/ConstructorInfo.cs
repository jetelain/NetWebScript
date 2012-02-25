using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.Script;
using System.ComponentModel;

namespace NetWebScript.Equivalents.Reflection
{
    [ScriptEquivalent(typeof(System.Reflection.ConstructorInfo))]
    [ScriptAvailable]
    internal class ConstructorInfo : MethodBase
    {
        private readonly Type type;
        private readonly JSFunction method;

        internal ConstructorInfo(Type type, JSFunction method)
        {
            this.type = type;
            this.method = method;
        }

        [ScriptBody(Inline = "new t()")]
        private static object CreateInstance(Type t)
        {
            throw new System.NotImplementedException();
        }

        public object Invoke(object[] parameters)
        {
            return method.Apply(CreateInstance(type), parameters);
        }

        public override object Invoke(object target, object[] parameters)
        {
            return method.Apply(target, parameters);
        }

        [ScriptBody(Inline = "ctor")]
        public static implicit operator ConstructorInfo(System.Reflection.ConstructorInfo ctor)
        {
            if (ctor != null)
            {
                return new ConstructorInfo(ctor.DeclaringType, new JSFunction(ctor));
            }
            return null;
        }
    }
}
