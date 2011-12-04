using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using NetWebScript.JsClr.TypeSystem.Invoker;

namespace NetWebScript.JsClr.TypeSystem.Helped
{
    class ScriptContructorHelped : IScriptConstructor, IMethodInvoker, IObjectCreationInvoker
    {
        private readonly ConstructorInfo ctor;
        private readonly IScriptConstructor helper;
        private readonly IScriptType owner;

        public ScriptContructorHelped(IScriptType owner, ConstructorInfo ctor, IScriptConstructor helper)
        {
            this.owner = owner;
            this.ctor = ctor;
            this.helper = helper;
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
            get { return helper.ImplId; }
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

        #region IMethodInvoker Members

        public JsBuilder.JsSyntax.JsToken WriteMethod(IScriptMethodBase method, ScriptAst.ScriptMethodInvocationExpression methodExpression, IRootInvoker converter)
        {
            return helper.Invoker.WriteMethod(helper, methodExpression, converter);
        }

        public JsBuilder.JsSyntax.JsToken WriteMethodReference(IScriptMethodBase method)
        {
            return helper.Invoker.WriteMethodReference(helper);
        }

        #endregion

        #region IObjectCreationInvoker Members

        public JsBuilder.JsSyntax.JsToken WriteObjectCreation(IScriptConstructor ctor, ScriptAst.ScriptObjectCreationExpression creationExpression, IRootInvoker converter)
        {
            return helper.CreationInvoker.WriteObjectCreation(helper, creationExpression, converter);
        }

        #endregion
    }
}
