using System;
using System.Linq;
using NetWebScript.JsClr.JsBuilder.JsSyntax;
using NetWebScript.JsClr.ScriptAst;

namespace NetWebScript.JsClr.TypeSystem.Invoker
{
    /// <summary>
    /// Invoker using standard conventions
    /// </summary>
    public class GlobalsInvoker : IFieldInvoker, IMethodInvoker, IObjectCreationInvoker
    {
        public static readonly GlobalsInvoker Instance = new GlobalsInvoker();

        public JsToken WriteField(IInvocableField field, ScriptAst.ScriptFieldReferenceExpression fieldExpression, IRootInvoker converter)
        {
            if (field.IsStatic)
            {
                return JsToken.Name(field.SlodId);
            }
            throw new NotSupportedException();
        }

        public JsToken WriteMethod(IInvocableMethodBase method, ScriptAst.ScriptMethodInvocationExpression methodExpression, IRootInvoker converter)
        {
            var writer = new JsTokenWriter();
            writer.Write(method.ImplId);
            writer.WriteArgs(methodExpression.Arguments.Select(a => a.Accept(converter)));
            return writer.ToToken(JsPrecedence.FunctionCall);
        }

        public JsToken WriteObjectCreation(IInvocableConstructor ctor, ScriptAst.ScriptObjectCreationExpression creationExpression, IRootInvoker converter)
        {
            throw new NotSupportedException();
        }

        public JsToken WriteMethodReference(IInvocableMethodBase method)
        {
            return JsToken.Name(method.ImplId);
        }

    }
}
