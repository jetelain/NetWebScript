using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using NetWebScript.JsClr.TypeSystem.Invoker;

namespace NetWebScript.JsClr.TypeSystem.Imported
{
    class ImportedField : IScriptField
    {
        private readonly FieldInfo field;
        private readonly string name;
        private readonly ImportedType owner;
        
        internal ImportedField(ImportedType owner, FieldInfo field)
        {
            this.owner = owner;
            this.field = field;
            this.name = owner.Name(field);
        }

        public string SlodId
        {
            get { return name; }
        }

        public FieldInfo Field
        {
            get { return field; }
        }

        public IScriptType Owner
        {
            get { return owner; }
        }

        public IFieldInvoker Invoker
        {
            // Fields works in a standard way
            get { return StandardInvoker.Instance; }
        }
    }
}
