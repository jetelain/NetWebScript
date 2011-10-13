using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using NetWebScript.JsClr.TypeSystem.Invoker;

namespace NetWebScript.JsClr.TypeSystem.Imported
{
    class ImportedConstructor : IScriptConstructor
    {
        private readonly ConstructorInfo ctor;
        private readonly ImportedType owner;

        public ImportedConstructor(ImportedType type, ConstructorInfo ctor)
        {
            this.owner = type;
            this.ctor = ctor;
        }

        public IObjectCreationInvoker CreationInvoker
        {
            get { return ImportedConstructorInvoker.Instance; }
        }

        public string ImplId
        {
            get { throw new NotSupportedException(); }
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
            get { return ImportedConstructorInvoker.Instance; }
        }

      
    }
}
