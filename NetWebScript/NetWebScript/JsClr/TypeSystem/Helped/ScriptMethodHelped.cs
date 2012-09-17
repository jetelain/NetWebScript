using System;
using System.Collections.Generic;
using System.Reflection;
using NetWebScript.JsClr.JsBuilder.JsSyntax;
using NetWebScript.JsClr.ScriptAst;
using NetWebScript.JsClr.TypeSystem.Invoker;

namespace NetWebScript.JsClr.TypeSystem.Helped
{
    class ScriptMethodHelped : MappedMethodBase, IScriptMethod, IMethodInvoker
    {
        private readonly IScriptMethod helper;

        public ScriptMethodHelped(IScriptType owner, MethodInfo method, IScriptMethod helper)
            : base(owner, method)
        {
            this.helper = helper;
        }


        public string SlodId
        {
            get { return helper.SlodId; }
        }

        public override string ImplId
        {
            get { return helper.ImplId; }
        }

        public override IMethodInvoker Invoker
        {
            get { return this; }
        }


        public JsBuilder.JsSyntax.JsToken WriteMethod(IInvocableMethodBase scriptMethod, ScriptMethodInvocationExpression methodExpression, IRootInvoker converter)
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

        public JsToken WriteMethodReference(IInvocableMethodBase method)
        {
            throw new NotSupportedException();
        }

        public bool InlineMethodCall
        {
            get 
            {
                if (helper.Method.IsStatic && !method.IsStatic)
                {
                    return false;
                }
                return helper.InlineMethodCall;
            }
        }

        public MethodScriptAst Ast
        {
            get { return helper.Ast; }
        }
    }
}
