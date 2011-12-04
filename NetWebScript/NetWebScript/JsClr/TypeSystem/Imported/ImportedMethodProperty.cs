using System;
using System.Reflection;
using NetWebScript.JsClr.JsBuilder.JsSyntax;
using NetWebScript.JsClr.TypeSystem.Invoker;

namespace NetWebScript.JsClr.TypeSystem.Imported
{
    class ImportedMethodProperty : IScriptMethod, IMethodInvoker
    {
        private readonly MethodInfo method;
        private readonly string name;
        private readonly ImportedType owner;
        private readonly bool isSetter;
        private readonly bool isIndexed;

        public ImportedMethodProperty(ImportedType owner, MethodInfo method, PropertyInfo property)
        {
            this.owner = owner;
            this.method = method;
            this.name = owner.Name(property);
            this.isSetter = property.GetSetMethod() == method;
            this.isIndexed = property.GetIndexParameters().Length > 0;
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

        public JsBuilder.JsSyntax.JsToken WriteMethod(IScriptMethodBase methodBase, ScriptAst.ScriptMethodInvocationExpression methodExpression, IRootInvoker converter)
        {
            JsToken token;
            int setArg = 0;
            if (isIndexed)
            {
                setArg = 1;
                var writer = new JsTokenWriter();
                writer.WriteTarget(methodExpression.Target.Accept(converter));
                writer.Write('[');
                writer.WriteCommaSeparated(methodExpression.Arguments[0].Accept(converter));
                writer.Write(']');
                token = writer.ToToken(JsPrecedence.Member);
            }
            else
            {
                if (method.IsStatic)
                {
                    token = JsToken.Member(JsToken.Name(owner.TypeId), name);
                }
                else
                {
                    token = JsToken.Member(methodExpression.Target.Accept(converter), name);
                }
            }

            if (isSetter)
            {
                return JsToken.Assign(token, methodExpression.Arguments[setArg].Accept(converter));
            }
            else
            {
                return token;
            }
        }

        public JsBuilder.JsSyntax.JsToken WriteMethodReference(IScriptMethodBase method)
        {
            throw new NotSupportedException();
        }

    }
}
