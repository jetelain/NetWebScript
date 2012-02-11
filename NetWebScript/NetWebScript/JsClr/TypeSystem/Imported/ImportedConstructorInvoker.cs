using System.Linq;
using NetWebScript.JsClr.JsBuilder.JsSyntax;
using NetWebScript.JsClr.ScriptAst;
using NetWebScript.JsClr.TypeSystem.Invoker;

namespace NetWebScript.JsClr.TypeSystem.Imported
{
    class ImportedConstructorInvoker : IMethodInvoker, IObjectCreationInvoker
    {
        public static readonly ImportedConstructorInvoker Instance = new ImportedConstructorInvoker();

        public JsToken WriteObjectCreation(IInvocableConstructor ctor, ScriptObjectCreationExpression creationExpression, IRootInvoker converter)
        {
            return JsToken.CreateInstance(
                ctor.Owner.TypeId, 
                creationExpression.Arguments.Select(a => a.Accept(converter)));
        }

        public JsToken WriteMethod(IInvocableMethodBase method, ScriptMethodInvocationExpression methodExpression, IRootInvoker converter)
        {
            JsTokenWriter writer = new JsTokenWriter();
            writer.Write(method.Owner.TypeId);
            writer.Write(".call");
            writer.WriteOpenArgs();
            writer.WriteArg(methodExpression.Target.Accept(converter));
            foreach (ScriptExpression expr in methodExpression.Arguments)
            {
                writer.WriteArg(expr.Accept(converter));
            }
            writer.WriteCloseArgs();
            return writer.ToToken(JsPrecedence.FunctionCall);
        }

        public JsToken WriteMethodReference(IInvocableMethodBase method)
        {
            return JsToken.Name(method.Owner.TypeId);
        }

    }
}
