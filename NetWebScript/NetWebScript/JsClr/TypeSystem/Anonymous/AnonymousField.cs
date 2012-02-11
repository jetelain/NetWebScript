using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using NetWebScript.JsClr.TypeSystem.Invoker;

namespace NetWebScript.JsClr.TypeSystem.Anonymous
{
    class AnonymousField : MappedField
    {
        private readonly string name;

        public AnonymousField(AnonymousType owner, FieldInfo field, CaseConvention convention)
            : base(owner, field)
        {
            this.name = Imported.ImportedType.Name(field.Name, convention);
        }

        public override string SlodId
        {
            get { return name; }
        }
    }
}
