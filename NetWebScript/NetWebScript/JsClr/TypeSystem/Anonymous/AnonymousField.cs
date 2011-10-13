using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using NetWebScript.JsClr.TypeSystem.Invoker;

namespace NetWebScript.JsClr.TypeSystem.Anonymous
{
    class AnonymousField : IScriptField
    {
        private readonly FieldInfo field;
        private readonly AnonymousType owner;
        private readonly string name;

        public AnonymousField(AnonymousType owner, FieldInfo field, CaseConvention convention)
        {
            this.owner = owner;
            this.field = field;
            this.name = Imported.ImportedType.Name(field.Name, convention);
        }


        #region IScriptField Members

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
            get { return StandardInvoker.Instance; }
        }

        #endregion
    }
}
