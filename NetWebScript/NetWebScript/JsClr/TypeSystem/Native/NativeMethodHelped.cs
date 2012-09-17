using System;
using System.Collections.Generic;
using System.Reflection;
using NetWebScript.JsClr.ScriptAst;
using NetWebScript.JsClr.TypeSystem.Invoker;

namespace NetWebScript.JsClr.TypeSystem.Native
{
    public class NativeMethodHelper : MappedMethodBase, IScriptMethod, IMethodInvoker
    {
        private readonly IScriptMethodBase helper;
        private readonly string slotId;
        private readonly string implId;

        public NativeMethodHelper(IScriptType owner, MethodInfo method, string name, IScriptMethodBase helper)
            : base(owner, method)
        {
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

        public string SlodId
        {
            get { return slotId; }
        }

        public override string ImplId
        {
            get { return implId; }
        }

        public override IMethodInvoker Invoker
        {
            get { return this; }
        }

        public JsBuilder.JsSyntax.JsToken WriteMethod(IInvocableMethodBase method, ScriptAst.ScriptMethodInvocationExpression methodExpression, IRootInvoker converter)
        {
            var arguments = new List<ScriptExpression>();
            arguments.Add(methodExpression.Target);
            arguments.AddRange(methodExpression.Arguments);

            var methdoExpressionProxy = new ScriptMethodInvocationExpression(methodExpression.IlOffset, false, helper, null, arguments);
            return helper.Invoker.WriteMethod(helper, methdoExpressionProxy, converter);
        }

        public JsBuilder.JsSyntax.JsToken WriteMethodReference(IInvocableMethodBase method)
        {
            throw new NotImplementedException();
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
