using System;
using System.Reflection;
using NetWebScript.JsClr.JsBuilder.JsSyntax;
using NetWebScript.JsClr.TypeSystem.Invoker;
using NetWebScript.JsClr.ScriptAst;

namespace NetWebScript.JsClr.TypeSystem.Native
{
    class ObjectConstructor : MappedMethodBase, IScriptConstructor, IObjectCreationInvoker, IMethodInvoker
    {
        public ObjectConstructor(IScriptType owner)
            : base(owner, owner.Type.GetConstructor(Type.EmptyTypes))
        {
  
        }

        public IObjectCreationInvoker CreationInvoker
        {
            get { return this; }
        }

        public override string ImplId
        {
            get { return null ; }
        }

        public override IMethodInvoker Invoker
        {
            get { return this; }
        }

        public JsToken WriteObjectCreation(IInvocableConstructor ctor, ScriptAst.ScriptObjectCreationExpression creationExpression, IRootInvoker converter)
        {
            JsTokenWriter writer = new JsTokenWriter();
            writer.Write("new Object()");
            return writer.ToToken(JsPrecedence.FunctionCall);
        }

        public JsToken WriteMethod(IInvocableMethodBase method, ScriptAst.ScriptMethodInvocationExpression methodExpression, IRootInvoker converter)
        {
            return null;
        }

        public JsToken WriteMethodReference(IInvocableMethodBase method)
        {
            throw new NotSupportedException();
        }
    }
}
