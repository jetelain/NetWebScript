using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using NetWebScript.JsClr.TypeSystem.Invoker;
using NetWebScript.JsClr.Ast;
using NetWebScript.JsClr.JsBuilder.JsSyntax;

namespace NetWebScript.JsClr.TypeSystem.Helped
{
    class ScriptMethodHelped : IScriptMethod, IMethodInvoker
    {
        private readonly MethodInfo method;
        private readonly IScriptType owner;
        private readonly IScriptMethod helper;

        public ScriptMethodHelped(IScriptType owner, MethodInfo method, IScriptMethod helper)
        {
            this.owner = owner;
            this.method = method;
            this.helper = helper;
        }

        #region IScriptMethod Members

        public string SlodId
        {
            get { return helper.SlodId; }
        }

        #endregion

        #region IScriptMethodBase Members

        public string ImplId
        {
            get { return helper.ImplId; }
        }

        public System.Reflection.MethodBase Method
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

        #region IMethodInvoker Members

        public JsBuilder.JsSyntax.JsToken WriteMethod(IScriptMethodBase scriptMethod, MethodInvocationExpression methodExpression, IRootInvoker converter)
        {
            if (helper.Method.IsStatic && !method.IsStatic)
            {
                var arguments = new List<Expression>();
                arguments.Add(methodExpression.Target);
                arguments.AddRange(methodExpression.Arguments);

                var methdoExpressionProxy = new MethodInvocationExpression(methodExpression.IlOffset, false, helper.Method, null, arguments);
                return helper.Invoker.WriteMethod(helper, methdoExpressionProxy, converter);
            }
            return helper.Invoker.WriteMethod(helper, methodExpression, converter);
        }

        public JsToken WriteMethodReference(IScriptMethodBase method)
        {
            throw new NotSupportedException();
        }
        #endregion
    }
}
