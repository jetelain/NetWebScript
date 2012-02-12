using System;
using System.Collections.Generic;
using System.Reflection;
using NetWebScript.JsClr.JsBuilder.JsSyntax;
using NetWebScript.JsClr.ScriptAst;
using NetWebScript.JsClr.TypeSystem.Invoker;
using NetWebScript.Remoting;

namespace NetWebScript.JsClr.TypeSystem.Remoting
{
    class ServerSideConstructor : MappedMethodBase, IScriptConstructor, IObjectCreationInvoker
    {
        private readonly IInvocableConstructor transparentCtor;
        private readonly IInvocableConstructor remoteInvokerCtor;

        public ServerSideConstructor(ScriptSystem system, ServerSideType owner, ConstructorInfo ctor)
            : base(owner, ctor)
        {
            transparentCtor = owner.TransparentProxy.Constructor;
            remoteInvokerCtor = system.GetScriptConstructor(typeof(RemoteInvoker).GetConstructor(Type.EmptyTypes));
        }

        public override string ImplId
        {
            get { return null; }
        }

        public IObjectCreationInvoker CreationInvoker
        {
            get { return this; }
        }

        public JsToken WriteObjectCreation(IInvocableConstructor ctor, ScriptObjectCreationExpression creationExpression, IRootInvoker converter)
        {
            return
                new ScriptObjectCreationExpression(creationExpression.IlOffset, transparentCtor, new List<ScriptExpression>(){
                    new ScriptObjectCreationExpression(creationExpression.IlOffset, remoteInvokerCtor, new List<ScriptExpression>())
                }).Accept(converter);
        }
    }
}
