using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using NetWebScript.JsClr.TypeSystem.Invoker;

namespace NetWebScript.JsClr.TypeSystem.Imported
{
    class ImportedMethod : IScriptMethod
    {
        private readonly MethodInfo method;
        private readonly string name;
        private readonly ImportedType owner;
        private readonly bool isOperator;

        public ImportedMethod(ImportedType owner, MethodInfo method)
        {
            if (method.IsStatic && method.IsSpecialName && method.Name.StartsWith("op_"))
            {
                this.owner = owner;
                this.method = method;
                isOperator = true;
                switch (method.Name)
                {
                    case "op_Subtraction":
                        name = "-";
                        break;
                    case "op_Equality":
                        name = "==";
                        break;
                    default:
                        throw new Exception(method.Name);
                }
            }
            else
            {
                this.owner = owner;
                this.method = method;
                this.name = owner.Name(method);
            }
        }

        public string SlodId
        {
            get { return name; }
        }

        public string ImplId
        {
            get { return name; }
        }

        public MethodBase Method
        {
            get { return method; }
        }

        public IScriptType Owner
        {
            get { return owner; }
        }

        public IMethodInvoker Invoker
        {
            // Methods works in a standard way
            get 
            {
                if (isOperator)
                {
                    return OperatorInvoker.Instance;
                }
                return StandardInvoker.Instance;
            }
        }

    }
}
