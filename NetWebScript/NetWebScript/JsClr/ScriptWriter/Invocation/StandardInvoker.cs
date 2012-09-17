using System.Linq;
using NetWebScript.JsClr.JsBuilder.JsSyntax;
using NetWebScript.JsClr.ScriptAst;

namespace NetWebScript.JsClr.TypeSystem.Invoker
{
    /// <summary>
    /// Invoker using standard conventions
    /// </summary>
    public class StandardInvoker : IFieldInvoker, IMethodInvoker, IObjectCreationInvoker
    {
        public static readonly StandardInvoker Instance = new StandardInvoker();

        public JsToken WriteField(IInvocableField field, ScriptAst.ScriptFieldReferenceExpression fieldExpression, IRootInvoker converter)
        {
            if (field.IsStatic)
            {
                return JsToken.Member(JsToken.Name(field.DeclaringType.TypeId), field.SlodId);
            }
            return JsToken.Member(fieldExpression.Target.Accept(converter), field.SlodId);
        }

        public JsToken WriteMethod(IInvocableMethodBase methodBase, ScriptAst.ScriptMethodInvocationExpression methodExpression, IRootInvoker converter)
        {
            IInvocableMethod method = methodBase as IInvocableMethod;
            if (methodExpression.IsExplicit && method != null && method.InlineMethodCall)
            {
                return WriteInlineMethodCall(method, methodExpression, converter);
            }

            var writer = new JsTokenWriter();
            var explicitCall = methodExpression.IsExplicit;
            if (methodBase.IsStatic)
            {
                writer.Write(methodBase.Owner.TypeId);
            }
            else
            {
                writer.WriteTarget(methodExpression.Target.Accept(converter));
            }
            writer.Write('.');
            if (methodExpression.IsExplicit)
            {
                writer.Write(methodBase.ImplId);
            }
            else
            {
                writer.Write(method.SlodId);
            }
            writer.WriteArgs(methodExpression.Arguments.Select(a => a.Accept(converter)));
            return writer.ToToken(JsPrecedence.FunctionCall);
        }

        private JsToken WriteInlineMethodCall(IInvocableMethod method, ScriptMethodInvocationExpression methodExpression, IRootInvoker converter)
        {
            


            throw new System.NotImplementedException();
        }

        public JsToken WriteObjectCreation(IInvocableConstructor ctor, ScriptAst.ScriptObjectCreationExpression creationExpression, IRootInvoker converter)
        {
            JsTokenWriter writer = new JsTokenWriter();
            writer.Write("new ");
            writer.Write(ctor.Owner.TypeId);
            writer.Write("().");
            writer.Write(ctor.ImplId);
            writer.WriteArgs(creationExpression.Arguments.Select(a => a.Accept(converter)));
            return writer.ToToken(JsPrecedence.FunctionCall);
        }

        public JsToken WriteMethodReference(IInvocableMethodBase method)
        {
            var writer = new JsTokenWriter();
            writer.Write(method.Owner.TypeId);
            if (!method.IsStatic)
            {
                writer.Write(".prototype.");
            }
            else
            {
                writer.Write('.');
            }
            writer.Write(method.ImplId);
            return writer.ToToken(JsPrecedence.Member);
        }

    }
}
