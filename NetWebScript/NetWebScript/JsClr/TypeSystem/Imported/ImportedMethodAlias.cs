using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.JsClr.TypeSystem.Invoker;
using System.Reflection;
using NetWebScript.JsClr.JsBuilder.JsSyntax;
using NetWebScript.JsClr.ScriptAst;

namespace NetWebScript.JsClr.TypeSystem.Imported
{
    class ImportedMethodAlias : MappedMethodBase, IScriptMethod, IMethodInvoker
    {
        private readonly string name;

        public ImportedMethodAlias(ImportedType owner, MethodInfo method, string name)
            : base(owner, method)
        {
            this.name = name;
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

        public JsBuilder.JsSyntax.JsToken WriteMethod(IInvocableMethodBase method, ScriptAst.ScriptMethodInvocationExpression methodExpression, IRootInvoker converter)
        {
            var writer = new JsTokenWriter();
            writer.Write(name);
            writer.WriteArgs(methodExpression.Arguments.Select(a => a.Accept(converter)));
            return writer.ToToken(JsPrecedence.FunctionCall);
        }

        public JsBuilder.JsSyntax.JsToken WriteMethodReference(IInvocableMethodBase method)
        {
            return JsToken.Name(name);
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
