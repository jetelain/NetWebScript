using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.JsClr.TypeSystem.Invoker;
using NetWebScript.JsClr.JsBuilder.JsSyntax;
using System.Reflection;

namespace NetWebScript.JsClr.TypeSystem.Native
{
    internal class FunctionType : NativeType, IValueSerializer
    {
        private readonly ScriptSystem system;

        public FunctionType(ScriptSystem system, Type type)
            : base(system, "Function", type)
        {
            this.system = system;
        }

        public override IValueSerializer Serializer
        {
            get
            {
                return this;
            }
        }

        public JsToken LiteralValue(IValueSerializer valueType, object value, IRootInvoker converter)
        {
            Type type = value as Type;
            if (type != null)
            {
                var scriptType = system.GetScriptType(type);
                if (scriptType == null)
                {
                    throw new Exception();
                }
                return JsToken.Name(scriptType.TypeId);
            }
            MethodBase methodBase = value as MethodBase;
            if (methodBase != null)
            {
                var targetMethod = system.GetScriptMethodBase(methodBase);
                if (targetMethod == null || targetMethod.Invoker == null)
                {
                    throw new Exception();
                }
                return targetMethod.Invoker.WriteMethodReference(targetMethod);
            }
            throw new Exception();
        }
    }
}
