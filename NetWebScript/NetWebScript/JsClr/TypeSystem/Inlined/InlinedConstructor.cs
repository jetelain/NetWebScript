using System;
using System.Collections.Generic;
using System.Reflection;
using NetWebScript.JsClr.JsBuilder.JsSyntax;
using NetWebScript.JsClr.JsBuilder.Pattern;
using NetWebScript.JsClr.ScriptAst;
using NetWebScript.JsClr.TypeSystem.Invoker;

namespace NetWebScript.JsClr.TypeSystem.Inlined
{
    internal sealed class InlinedConstructor : MappedMethodBase, IScriptConstructor, IObjectCreationInvoker, IMethodInvoker
    {
        private readonly ScriptTypeBase owner;
        private readonly InlineFragment pattern;

        internal InlinedConstructor(IScriptType owner, ConstructorInfo method, string patternString)
            : base(owner, method)
        {
            this.pattern = new InlineFragment(patternString);
        }

        public string SlodId
        {
            get { return null; }
        }

        public override string ImplId
        {
            get { return null; }
        }

        public override IMethodInvoker Invoker
        {
            get { return this; }
        }

        public IInvocableType Owner
        {
            get { return owner; }
        }

        public IObjectCreationInvoker CreationInvoker
        {
            get { return this; }
        }

        public JsToken WriteObjectCreation(ScriptAst.IInvocableConstructor ctor, ScriptObjectCreationExpression creationExpression, IRootInvoker converter)
        {
            var locals = new Dictionary<string, JsToken>();
            var args = creationExpression.Arguments;
            if (args != null && args.Count > 0)
            {
                var argsDef = this.method.GetParameters();
                for (int i = 0; i < argsDef.Length; ++i)
                {
                    locals.Add(argsDef[i].Name, args[i].Accept(converter));
                }
            }
            return pattern.Execute(locals);
        }

        public JsToken WriteMethod(IInvocableMethodBase method, ScriptMethodInvocationExpression methodExpression, IRootInvoker converter)
        {
            throw new NotSupportedException();
        }

        public JsToken WriteMethodReference(IInvocableMethodBase method)
        {
            throw new NotSupportedException();
        }
    }
}
