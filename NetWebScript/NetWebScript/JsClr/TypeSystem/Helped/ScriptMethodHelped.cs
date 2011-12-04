using System;
using System.Collections.Generic;
using System.Reflection;
using NetWebScript.JsClr.JsBuilder.JsSyntax;
using NetWebScript.JsClr.ScriptAst;
using NetWebScript.JsClr.TypeSystem.Invoker;

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

        public JsBuilder.JsSyntax.JsToken WriteMethod(IScriptMethodBase scriptMethod, ScriptMethodInvocationExpression methodExpression, IRootInvoker converter)
        {
            if (helper.Method.IsStatic && !method.IsStatic)
            {
                var arguments = new List<ScriptExpression>();
                arguments.Add(methodExpression.Target);
                arguments.AddRange(methodExpression.Arguments);

                var methdoExpressionProxy = new ScriptMethodInvocationExpression(methodExpression.IlOffset, false, helper, null, arguments);
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
