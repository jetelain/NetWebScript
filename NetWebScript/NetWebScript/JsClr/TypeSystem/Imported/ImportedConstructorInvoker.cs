using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.JsClr.TypeSystem.Invoker;
using NetWebScript.JsClr.JsBuilder.JsSyntax;
using NetWebScript.JsClr.Ast;

namespace NetWebScript.JsClr.TypeSystem.Imported
{
    class ImportedConstructorInvoker : IMethodInvoker, IObjectCreationInvoker
    {
        public static readonly ImportedConstructorInvoker Instance = new ImportedConstructorInvoker();

        public JsToken WriteObjectCreation(IScriptConstructor ctor, ObjectCreationExpression creationExpression, IRootInvoker converter)
        {
            return JsToken.CreateInstance(
                ctor.Owner.TypeId, 
                creationExpression.Arguments.Select(a => a.Accept(converter)));
        }

        public JsToken WriteMethod(IScriptMethodBase method, MethodInvocationExpression methodExpression, IRootInvoker converter)
        {
            JsTokenWriter writer = new JsTokenWriter();
            writer.Write(method.Owner.TypeId);
            writer.Write(".call");
            writer.WriteOpenArgs();
            writer.WriteArg(methodExpression.Target.Accept(converter));
            foreach (Expression expr in methodExpression.Arguments)
            {
                writer.WriteArg(expr.Accept(converter));
            }
            writer.WriteCloseArgs();
            return writer.ToToken(JsPrecedence.FunctionCall);
        }

        public JsToken WriteMethodReference(IScriptMethodBase method)
        {
            return JsToken.Name(method.Owner.TypeId);
        }

    }
}
