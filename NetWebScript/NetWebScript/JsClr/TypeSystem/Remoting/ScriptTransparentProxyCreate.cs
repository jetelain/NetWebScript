using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using NetWebScript.JsClr.TypeSystem.Invoker;
using NetWebScript.JsClr.ScriptAst;
using NetWebScript.JsClr.JsBuilder.JsSyntax;

namespace NetWebScript.JsClr.TypeSystem.Remoting
{
    class ScriptTransparentProxyCreate : MappedMethodBase, IMethodInvoker, IScriptMethod
    {
        private readonly TransparentType proxyType;

        public ScriptTransparentProxyCreate(ScriptTransparentProxyType owner, MethodInfo method, TransparentType proxyType)
            : base(owner, method)
        {
            this.proxyType = proxyType;
        }

        public override string ImplId
        {
            get { return null; }
        }

        public override IMethodInvoker Invoker
        {
            get
            {
                return this;
            }
        }

        public JsToken WriteMethod(IInvocableMethodBase method, ScriptMethodInvocationExpression methodExpression, IRootInvoker converter)
        {
            return new ScriptObjectCreationExpression(methodExpression.IlOffset, proxyType.Constructor, methodExpression.Arguments).Accept(converter);
        }

        public JsBuilder.JsSyntax.JsToken WriteMethodReference(ScriptAst.IInvocableMethodBase method)
        {
            throw new NotSupportedException();
        }

        public string SlodId
        {
            get { return null; }
        }

        public bool InlineMethodCall
        {
            get { return false; }
        }

        public ScriptAst.MethodScriptAst Ast
        {
            get { return null; }
        }
    }
}
