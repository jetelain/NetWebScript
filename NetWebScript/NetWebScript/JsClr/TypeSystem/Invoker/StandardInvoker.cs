using System.Linq;
using NetWebScript.JsClr.JsBuilder.JsSyntax;

namespace NetWebScript.JsClr.TypeSystem.Invoker
{
    /// <summary>
    /// Invoker using standard conventions
    /// </summary>
    public class StandardInvoker : IFieldInvoker, IMethodInvoker, IObjectCreationInvoker
    {
        public static readonly StandardInvoker Instance = new StandardInvoker();

        public JsToken WriteField(IScriptField field, ScriptAst.ScriptFieldReferenceExpression fieldExpression, IRootInvoker converter)
        {
            if (field.Field.IsStatic)
            {
                return JsToken.Member(JsToken.Name(field.Owner.TypeId), field.SlodId);
            }
            return JsToken.Member(fieldExpression.Target.Accept(converter), field.SlodId);
        }

        public JsToken WriteMethod(IScriptMethodBase method, ScriptAst.ScriptMethodInvocationExpression methodExpression, IRootInvoker converter)
        {
            var writer = new JsTokenWriter();
            var explicitCall = methodExpression.IsExplicit;
            if (method.Method.IsStatic)
            {
                writer.Write(method.Owner.TypeId);
            }
            else
            {
                writer.WriteTarget(methodExpression.Target.Accept(converter));
            }
            writer.Write('.');
            if (methodExpression.IsExplicit)
            {
                writer.Write(method.ImplId);
            }
            else
            {
                writer.Write(((IScriptMethod)method).SlodId);
            }
            writer.WriteArgs(methodExpression.Arguments.Select(a => a.Accept(converter)));
            return writer.ToToken(JsPrecedence.FunctionCall);
        }

        public JsToken WriteObjectCreation(IScriptConstructor ctor, ScriptAst.ScriptObjectCreationExpression creationExpression, IRootInvoker converter)
        {
            JsTokenWriter writer = new JsTokenWriter();
            writer.Write("new ");
            writer.Write(ctor.Owner.TypeId);
            writer.Write("().");
            writer.Write(ctor.ImplId);
            writer.WriteArgs(creationExpression.Arguments.Select(a => a.Accept(converter)));
            return writer.ToToken(JsPrecedence.FunctionCall);
        }

        public JsToken WriteMethodReference(IScriptMethodBase method)
        {
            var writer = new JsTokenWriter();
            writer.Write(method.Owner.TypeId);
            if (!method.Method.IsStatic)
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
