using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using NetWebScript.JsClr.TypeSystem.Invoker;

namespace NetWebScript.JsClr.TypeSystem.Imported
{
    class ImportedMethod : IScriptMethod
    {
        private readonly MethodInfo method;
        private readonly string name;
        private readonly ImportedType owner;

        public ImportedMethod(ImportedType owner, MethodInfo method)
        {
            this.owner = owner;
            this.method = method;
            this.name = owner.Name(method);
        }

        public string SlodId
        {
            get { return name; }
        }

        public string ImplId
        {
            get { return name; }
        }

        public MethodBase Method
        {
            get { return method; }
        }

        public IScriptType Owner
        {
            get { return owner; }
        }

        public IMethodInvoker Invoker
        {
            // Methods works in a standard way
            get { return StandardInvoker.Instance; }
        }

    }
}
