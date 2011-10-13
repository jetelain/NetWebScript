using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.JsClr.TypeSystem.Invoker;
using NetWebScript.JsClr.JsBuilder.JsSyntax;
using NetWebScript.JsClr.Ast;
using System.Reflection;

namespace NetWebScript.JsClr.TypeSystem.Native
{
    class DelegateMethodInvoke : IScriptMethod, IMethodInvoker
    {
        private readonly DelegateType owner;
        private readonly MethodInfo method;

        public DelegateMethodInvoke(DelegateType owner, MethodInfo method)
        {
            this.owner = owner;
            this.method = method;
        }

        public JsToken WriteMethod(IScriptMethodBase method, MethodInvocationExpression methodExpression, IRootInvoker converter)
        {
            var writer = new JsTokenWriter();
            writer.WriteLeft(JsPrecedence.FunctionCall, methodExpression.Target.Accept(converter));
            writer.WriteArgs(methodExpression.Arguments.Select(a => a.Accept(converter)));
            return writer.ToToken(JsPrecedence.FunctionCall);
        }

        public JsToken WriteMethodReference(IScriptMethodBase method)
        {
            throw new NotSupportedException();
        }


        #region IScriptMethod Members

        public string SlodId
        {
            get { return null; }
        }

        #endregion

        #region IScriptMethodBase Members

        public string ImplId
        {
            get { return null; }
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
    }
}
