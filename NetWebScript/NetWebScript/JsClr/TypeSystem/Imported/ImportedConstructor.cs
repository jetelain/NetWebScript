using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using NetWebScript.JsClr.TypeSystem.Invoker;

namespace NetWebScript.JsClr.TypeSystem.Imported
{
    class ImportedConstructor : MappedMethodBase, IScriptConstructor
    {
        public ImportedConstructor(ImportedType type, ConstructorInfo ctor)
            : base(type, ctor)
        {

        }

        public IObjectCreationInvoker CreationInvoker
        {
            get { return ImportedConstructorInvoker.Instance; }
        }

        public override string ImplId
        {
            get { throw new NotSupportedException(); }
        }

        public override IMethodInvoker Invoker
        {
            get { return ImportedConstructorInvoker.Instance; }
        }

    }
}
