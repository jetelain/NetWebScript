using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using NetWebScript.JsClr.TypeSystem.Invoker;

namespace NetWebScript.JsClr.TypeSystem.Imported
{
    class ImportedField : MappedField
    {
        private readonly string name;
        
        internal ImportedField(ImportedType owner, FieldInfo field)
            : base(owner, field)
        {
            this.name = owner.Name(field);
        }

        public override string SlodId
        {
            get { return name; }
        }
    }
}
