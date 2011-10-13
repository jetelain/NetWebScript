using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.JsClr.TypeSystem.Invoker;
using System.Reflection;
using NetWebScript.JsClr.JsBuilder.JsSyntax;

namespace NetWebScript.JsClr.TypeSystem.Imported
{
    class ImportedMethodAlias : IScriptMethod, IMethodInvoker
    {
        private readonly MethodInfo method;
        private readonly string name;
        private readonly ImportedType owner;

        public ImportedMethodAlias(ImportedType owner, MethodInfo method, string name)
        {
            this.owner = owner;
            this.method = method;
            this.name = name;
        }

        public string SlodId
        {
            get { return null; }
        }

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

        public JsBuilder.JsSyntax.JsToken WriteMethod(IScriptMethodBase method, Ast.MethodInvocationExpression methodExpression, IRootInvoker converter)
        {
            var writer = new JsTokenWriter();
            writer.Write(name);
            writer.WriteArgs(methodExpression.Arguments.Select(a => a.Accept(converter)));
            return writer.ToToken(JsPrecedence.FunctionCall);
        }

        public JsBuilder.JsSyntax.JsToken WriteMethodReference(IScriptMethodBase method)
        {
            return JsToken.Name(name);
        }

    }
}
