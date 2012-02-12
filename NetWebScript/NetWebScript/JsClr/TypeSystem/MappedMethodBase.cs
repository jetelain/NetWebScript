using System;
using System.Reflection;
using NetWebScript.JsClr.ScriptAst;
using NetWebScript.JsClr.TypeSystem.Invoker;

namespace NetWebScript.JsClr.TypeSystem
{
    public abstract class MappedMethodBase : IScriptMethodBase
    {
        protected readonly MethodBase method;
        protected readonly IScriptType owner;

        protected MappedMethodBase(IScriptType owner, MethodBase method)
        {
            this.owner = owner;
            this.method = method;
        }

        public MethodBase Method
        {
            get { return method; }
        }

        public IScriptType OwnerScriptType
        {
            get { return owner; }
        }

        public abstract string ImplId
        {
            get;
        }

        public bool IsStatic
        {
            get { return method.IsStatic; }
        }

        IInvocableType IInvocableMethodBase.Owner
        {
            get { return owner; }
        }

        public virtual Invoker.IMethodInvoker Invoker
        {
            get { return StandardInvoker.Instance; }
        }

        public string DisplayName
        {
            get { return method.Name; }
        }

        public bool IsVirtual
        {
            get { return method.IsVirtual; }
        }
    }
}
