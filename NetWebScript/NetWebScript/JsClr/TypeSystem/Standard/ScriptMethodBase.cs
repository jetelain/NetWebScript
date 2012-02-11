using System.Reflection;
using NetWebScript.JsClr.ScriptAst;
using NetWebScript.JsClr.ScriptWriter.Declaration;
using NetWebScript.JsClr.TypeSystem.Invoker;
using NetWebScript.Metadata;

namespace NetWebScript.JsClr.TypeSystem.Standard
{
    public abstract class ScriptMethodBase : MappedMethodBase, IScriptMethodBaseDeclaration
    {
        private readonly string impl;
        private readonly string body;
        private readonly IMethodInvoker invoker;
        private readonly MethodBaseMetadata metadata;

        protected ScriptMethodBase(ScriptSystem system, IScriptType owner, MethodBase method, string body, bool isGlobal)
            : base(owner, method)
        {
            this.impl = system.CreateImplementationId();
            this.body = body;
            this.invoker = isGlobal ? (IMethodInvoker)GlobalsInvoker.Instance : (IMethodInvoker)StandardInvoker.Instance;
            metadata = CreateMetadata();
        }

        private MethodBaseMetadata CreateMetadata()
        {
            if (owner.Metadata == null)
            {
                return null;
            }
            var meta = new MethodBaseMetadata();
            meta.Type = owner.Metadata;
            meta.Name = ImplId;
            meta.CRef = CRefToolkit.GetCRef(Method);
            owner.Metadata.Methods.Add(meta);
            return meta;
        }


        /// <summary>
        /// Identifier of implementation (name to use for an explicit call of method)
        /// </summary>
        public override string ImplId
        {
            get { return impl; }
        }

        public override IMethodInvoker Invoker 
        { 
            get { return invoker; } 
        }

        public MethodScriptAst Ast
        {
            get;
            set;
        }

        /// <summary>
        /// If <see cref="HasNativeBody"/>, the method script body in the form "function(...){...}".
        /// </summary>
        public string NativeBody
        {
            get { return body; }
        }

        /// <summary>
        /// Does method have a native implementation.
        /// In that case, we will not generate script from CIL for this method, 
        /// and simply use the provided body script.
        /// </summary>
        public bool HasNativeBody
        {
            get { return body != null; }
        }

        public MethodBaseMetadata Metadata
        {
            get { return metadata; }
        }

        public string PrettyName
        {
            get { return method.ToString(); }
        }

    }
}
