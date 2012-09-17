using System;
using System.Linq;
using System.Reflection;
using NetWebScript.JsClr.JsBuilder.JsSyntax;
using NetWebScript.JsClr.ScriptAst;
using NetWebScript.JsClr.TypeSystem.Invoker;

namespace NetWebScript.JsClr.TypeSystem.Native
{
    class DelegateMethodInvoke : MappedMethodBase, IScriptMethod, IMethodInvoker
    {

        public DelegateMethodInvoke(DelegateType owner, MethodInfo method)
            : base(owner, method)
        {

        }

        public JsToken WriteMethod(IInvocableMethodBase method, ScriptMethodInvocationExpression methodExpression, IRootInvoker converter)
        {
            var writer = new JsTokenWriter();
            writer.WriteLeft(JsPrecedence.FunctionCall, methodExpression.Target.Accept(converter));
            writer.WriteArgs(methodExpression.Arguments.Select(a => a.Accept(converter)));
            return writer.ToToken(JsPrecedence.FunctionCall);
        }

        public JsToken WriteMethodReference(IInvocableMethodBase method)
        {
            throw new NotSupportedException();
        }

        public string SlodId
        {
            get { return null; }
        }

        public override string ImplId
        {
            get { return null; }
        }

        public override IMethodInvoker Invoker
        {
            get { return this; }
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
