using System;
using System.Reflection;
using NetWebScript.JsClr.JsBuilder.JsSyntax;
using NetWebScript.JsClr.TypeSystem.Invoker;

namespace NetWebScript.JsClr.TypeSystem.Native
{
    class ObjectConstructor : IScriptConstructor, IObjectCreationInvoker, IMethodInvoker
    {
        private readonly IScriptType owner;
        private readonly ConstructorInfo ctor;

        public ObjectConstructor(IScriptType owner)
        {
            this.owner = owner;
            ctor = owner.Type.GetConstructor(Type.EmptyTypes);
        }

        #region IScriptConstructor Members

        public IObjectCreationInvoker CreationInvoker
        {
            get { return this; }
        }

        #endregion

        #region IScriptMethodBase Members

        public string ImplId
        {
            get { return null ; }
        }

        public MethodBase Method
        {
            get { return ctor; }
        }

        public IScriptType Owner
        {
            get { return owner; }
        }

        public IMethodInvoker Invoker
        {
            get { return this; }
        }

        #endregion

        #region IObjectCreationInvoker Members

        public JsToken WriteObjectCreation(IScriptConstructor ctor, ScriptAst.ScriptObjectCreationExpression creationExpression, IRootInvoker converter)
        {
            JsTokenWriter writer = new JsTokenWriter();
            writer.Write("new Object()");
            return writer.ToToken(JsPrecedence.FunctionCall);
        }

        #endregion

        #region IMethodInvoker Members

        public JsToken WriteMethod(IScriptMethodBase method, ScriptAst.ScriptMethodInvocationExpression methodExpression, IRootInvoker converter)
        {
            return null;
        }

        public JsToken WriteMethodReference(IScriptMethodBase method)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}
