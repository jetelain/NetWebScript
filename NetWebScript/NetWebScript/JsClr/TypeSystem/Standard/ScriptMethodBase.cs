using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using NetWebScript.JsClr.TypeSystem.Invoker;
using NetWebScript.JsClr.AstBuilder;

namespace NetWebScript.JsClr.TypeSystem.Standard
{
    public abstract class ScriptMethodBase : IScriptMethodBase
    {
        private readonly MethodBase method;
        private readonly string impl;
        private readonly ScriptType owner;
        private readonly string body;

        protected ScriptMethodBase(ScriptSystem system, ScriptType owner, MethodBase method, string body)
        {
            this.owner = owner;
            this.method = method;
            this.impl = system.CreateImplementationId();
            this.body = body;
        }

        /// <summary>
        /// Identifier of implementation (name to use for an explicit call of method)
        /// </summary>
        public string ImplId
        {
            get { return impl; }
        }

        public IScriptType Owner
        {
            get { return owner; }
        }

        public MethodBase Method
        {
            get { return method; }
        }

        public IMethodInvoker Invoker 
        { 
            get 
            {
                if (owner.IsGlobals)
                {
                    return GlobalsInvoker.Instance;
                }
                return StandardInvoker.Instance; 
            } 
        }

        public MethodAst Ast
        {
            get;
            set;
        }

        /// <summary>
        /// If <see cref="HasNativeBody"/>, the method script body in the form "function(...){...}".
        /// </summary>
        internal string NativeBody
        {
            get { return body; }
        }

        /// <summary>
        /// Does method have a native implementation.
        /// In that case, we will not generate script from CIL for this method, 
        /// and simply use the provided body script.
        /// </summary>
        internal bool HasNativeBody
        {
            get { return body != null; }
        }
    }
}
