using System;
using System.Linq;
using NetWebScript.JsClr.JsBuilder.JsSyntax;

namespace NetWebScript.JsClr.TypeSystem.Invoker
{
    /// <summary>
    /// Invoker using standard conventions
    /// </summary>
    public class GlobalsInvoker : IFieldInvoker, IMethodInvoker, IObjectCreationInvoker
    {
        public static readonly GlobalsInvoker Instance = new GlobalsInvoker();

        public JsToken WriteField(IScriptField field, Ast.FieldReferenceExpression fieldExpression, IRootInvoker converter)
        {
            if (field.Field.IsStatic)
            {
                return JsToken.Name(field.SlodId);
            }
            throw new NotSupportedException();
        }

        public JsToken WriteMethod(IScriptMethodBase method, Ast.MethodInvocationExpression methodExpression, IRootInvoker converter)
        {
            var writer = new JsTokenWriter();
            writer.Write(method.ImplId);
            writer.WriteArgs(methodExpression.Arguments.Select(a => a.Accept(converter)));
            return writer.ToToken(JsPrecedence.FunctionCall);
        }

        public JsToken WriteObjectCreation(IScriptConstructor ctor, Ast.ObjectCreationExpression creationExpression, IRootInvoker converter)
        {
            throw new NotSupportedException();
        }

        public JsToken WriteMethodReference(IScriptMethodBase method)
        {
            return JsToken.Name(method.ImplId);
        }

    }
}
