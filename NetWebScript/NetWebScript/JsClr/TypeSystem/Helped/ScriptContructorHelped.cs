using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using NetWebScript.JsClr.TypeSystem.Invoker;
using NetWebScript.JsClr.ScriptAst;

namespace NetWebScript.JsClr.TypeSystem.Helped
{
    class ScriptContructorHelped : MappedMethodBase, IScriptConstructor, IMethodInvoker, IObjectCreationInvoker
    {
        private readonly IScriptConstructor helper;

        public ScriptContructorHelped(IScriptType owner, ConstructorInfo ctor, IScriptConstructor helper)
            : base(owner, ctor)
        {

            this.helper = helper;
        }

        public IObjectCreationInvoker CreationInvoker
        {
            get { return this; }
        }

        public override string ImplId
        {
            get { return helper.ImplId; }
        }

        public override IMethodInvoker Invoker
        {
            get { return this; }
        }

        public JsBuilder.JsSyntax.JsToken WriteMethod(IInvocableMethodBase method, ScriptAst.ScriptMethodInvocationExpression methodExpression, IRootInvoker converter)
        {
            return helper.Invoker.WriteMethod(helper, methodExpression, converter);
        }

        public JsBuilder.JsSyntax.JsToken WriteMethodReference(IInvocableMethodBase method)
        {
            return helper.Invoker.WriteMethodReference(helper);
        }

        public JsBuilder.JsSyntax.JsToken WriteObjectCreation(IInvocableConstructor ctor, ScriptAst.ScriptObjectCreationExpression creationExpression, IRootInvoker converter)
        {
            return helper.CreationInvoker.WriteObjectCreation(helper, creationExpression, converter);
        }

    }
}
