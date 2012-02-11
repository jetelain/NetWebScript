using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.JsClr.TypeSystem.Invoker;
using System.Reflection;
using NetWebScript.JsClr.JsBuilder.JsSyntax;
using NetWebScript.JsClr.ScriptAst;

namespace NetWebScript.JsClr.TypeSystem.Anonymous
{
    class AnonymousConstructor : MappedMethodBase, IScriptConstructor, IObjectCreationInvoker
    {
        public AnonymousConstructor(AnonymousType owner, ConstructorInfo ctor)
            : base(owner, ctor)
        {

        }

        public IObjectCreationInvoker CreationInvoker
        {
            get { return this; }
        }

        public override string ImplId
        {
            get { return null; }
        }

        public override IMethodInvoker Invoker
        {
            get { return null; }
        }

        public JsToken WriteObjectCreation(IInvocableConstructor ctor, ScriptAst.ScriptObjectCreationExpression creationExpression, IRootInvoker converter)
        {
            return JsToken.Name("{}");
        }

    }
}
