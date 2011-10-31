using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using NetWebScript.JsClr.TypeSystem.Invoker;
using NetWebScript.JsClr.Ast;

namespace NetWebScript.JsClr.TypeSystem.Native
{
    public class NativeMethodHelper : IScriptMethod, IMethodInvoker
    {
        private readonly IScriptType owner;
        private readonly IScriptMethodBase helper;
        private readonly MethodInfo method;
        private readonly string slotId;
        private readonly string implId;

        public NativeMethodHelper(IScriptType owner, MethodInfo method, string name, IScriptMethodBase helper)
        {
            this.owner = owner;
            this.method = method;
            this.helper = helper;
            if (method.IsVirtual)
            {
                slotId = name;
                implId = name;
            }
            else
            {
                implId = name;
            }
        }


        #region IScriptMethod Members

        public string SlodId
        {
            get { return slotId; }
        }

        #endregion

        #region IScriptMethodBase Members

        public string ImplId
        {
            get { return implId; }
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
            get { return this; }
        }

        #endregion

        public JsBuilder.JsSyntax.JsToken WriteMethod(IScriptMethodBase method, Ast.MethodInvocationExpression methodExpression, IRootInvoker converter)
        {
            var arguments = new List<Expression>();
            arguments.Add(methodExpression.Target);
            arguments.AddRange(methodExpression.Arguments);

            var methdoExpressionProxy = new MethodInvocationExpression(methodExpression.IlOffset, false, helper.Method, null, arguments);
            return helper.Invoker.WriteMethod(helper, methdoExpressionProxy, converter);
        }

        public JsBuilder.JsSyntax.JsToken WriteMethodReference(IScriptMethodBase method)
        {
            throw new NotImplementedException();
        }
    }
}
