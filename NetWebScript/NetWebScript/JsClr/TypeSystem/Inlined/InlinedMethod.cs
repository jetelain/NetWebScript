using System;
using System.Collections.Generic;
using System.Reflection;
using NetWebScript.JsClr.JsBuilder.JsSyntax;
using NetWebScript.JsClr.JsBuilder.Pattern;
using NetWebScript.JsClr.ScriptAst;
using NetWebScript.JsClr.TypeSystem.Invoker;

namespace NetWebScript.JsClr.TypeSystem.Inlined
{
    class InlinedMethod : MappedMethodBase, IScriptMethod, IMethodInvoker
    {
        private readonly InlineFragment pattern;

        public InlinedMethod(IScriptType owner, MethodInfo method, string patternString)
            : base(owner, method)
        {
            this.pattern = new InlineFragment(patternString);
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

        public JsToken WriteMethod(IInvocableMethodBase method, ScriptAst.ScriptMethodInvocationExpression methodExpression, IRootInvoker converter)
        {
            var locals = new Dictionary<string, JsToken>();
            if (methodExpression.Target != null)
            {
                locals.Add("this", methodExpression.Target.Accept(converter));
            }
            var args = methodExpression.Arguments;
            if (args != null && args.Count > 0)
            {
                var argsDef = this.method.GetParameters();
                for (int i = 0; i < argsDef.Length; ++i)
                {
                    locals.Add(argsDef[i].Name, args[i].Accept(converter));
                }
            }
            return pattern.Execute(locals);

        }

        public JsToken WriteMethodReference(IInvocableMethodBase method)
        {
            throw new NotSupportedException();
        }
    }
}
