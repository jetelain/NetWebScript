using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using NetWebScript.JsClr.TypeSystem.Invoker;

namespace NetWebScript.JsClr.TypeSystem.Imported
{
    class ImportedMethod : MappedMethodBase, IScriptMethod
    {
        private readonly string name;
        private readonly bool isOperator;

        public ImportedMethod(ImportedType owner, MethodInfo method)
            : base(owner, method)
        {
            if (method.IsStatic && method.IsSpecialName && method.Name.StartsWith("op_"))
            {
                isOperator = true;
                switch (method.Name)
                {
                    case "op_Subtraction":
                        name = "-";
                        break;
                    case "op_Equality":
                        name = "==";
                        break;
                    case "op_Inequality":
                        name = "!=";
                        break;
                    case "op_GreaterThan":
                        name = ">";
                        break;
                    case "op_LowerThan":
                        name = "<";
                        break;
                    default:
                        throw new Exception(method.Name);
                }
            }
            else
            {
                this.name = owner.Name(method);
            }
        }

        public string SlodId
        {
            get { return name; }
        }

        public override string ImplId
        {
            get { return name; }
        }

        public override IMethodInvoker Invoker
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
