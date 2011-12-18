using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.JsClr.TypeSystem.Invoker;
using NetWebScript.JsClr.JsBuilder.JsSyntax;
using System.Reflection;

namespace NetWebScript.JsClr.TypeSystem.Serializers
{
    class MethodBaseSerializer : IValueSerializer
    {
        private readonly ScriptSystem system;

        internal MethodBaseSerializer ( ScriptSystem system )
        {
            this.system = system;
        }

        public JsToken LiteralValue(IScriptType valueType, object value, IRootInvoker converter)
        {
            MethodBase methodBase = (MethodBase)value;
            var targetMethod = system.GetScriptMethodBase(methodBase);
            if (targetMethod == null || targetMethod.Invoker == null)
            {
                throw new InvalidOperationException();
            }
            return targetMethod.Invoker.WriteMethodReference(targetMethod);
        }
    }
}
