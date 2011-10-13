using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.JsClr.TypeSystem.Invoker;
using System.Reflection;
using NetWebScript.JsClr.JsBuilder.JsSyntax;

namespace NetWebScript.JsClr.TypeSystem.Anonymous
{
    class AnonymousConstructor : IScriptConstructor, IObjectCreationInvoker
    {
        private readonly ConstructorInfo ctor;
        private readonly AnonymousType owner;

        public AnonymousConstructor(AnonymousType owner, ConstructorInfo ctor)
        {
            this.owner = owner;
            this.ctor = ctor;
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
            get { return null; }
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
            get { return null; }
        }

        #endregion

        #region IObjectCreationInvoker Members

        public JsToken WriteObjectCreation(IScriptConstructor ctor, Ast.ObjectCreationExpression creationExpression, IRootInvoker converter)
        {
            return JsToken.Name("{}");
        }

        #endregion
    }
}
