using System;
using System.Collections.Generic;
using System.Reflection;
using NetWebScript.JsClr.JsBuilder.JsSyntax;
using NetWebScript.JsClr.JsBuilder.Pattern;
using NetWebScript.JsClr.TypeSystem.Invoker;

namespace NetWebScript.JsClr.TypeSystem.Inlined
{
    class InlinedMethod : IScriptMethod, IMethodInvoker
    {
        private readonly MethodInfo method;
        private readonly InlineFragment pattern;
        private readonly IScriptType owner;

        public InlinedMethod(IScriptType owner, MethodInfo method, string patternString)
        {
            this.owner = owner;
            this.method = method;
            this.pattern = new InlineFragment(patternString);
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

        #region IMethodInvoker Members

        public JsToken WriteMethod(IScriptMethodBase method, ScriptAst.ScriptMethodInvocationExpression methodExpression, IRootInvoker converter)
        {
            var locals = new Dictionary<string, JsToken>();
            if (methodExpression.Target != null)
            {
                locals.Add("this", methodExpression.Target.Accept(converter));
            }
            var args = methodExpression.Arguments;
            if (args != null && args.Count > 0)
            {
                var argsDef = method.Method.GetParameters();
                for (int i = 0; i < argsDef.Length; ++i)
                {
                    locals.Add(argsDef[i].Name, args[i].Accept(converter));
                }
            }
            return pattern.Execute(locals);

        }

        #endregion

        #region IMethodInvoker Members


        public JsToken WriteMethodReference(IScriptMethodBase method)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}
