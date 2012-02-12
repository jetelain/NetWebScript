using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.JsClr.TypeSystem.Imported;

namespace NetWebScript.JsClr.TypeSystem.Standard
{
    class ScriptExtenderType : ScriptType, IScriptTypeExtender
    {
        private readonly ImportedType extended;

        public ScriptExtenderType(ScriptSystem system, Type type, Type extendedType)
            : base(system, type)
        {
            extended = (ImportedType)system.GetScriptType(extendedType);
            extended.AddExtender(this);

            // TODO: transfert interfaces impl
        }

        public override IEnumerable<ScriptWriter.Declaration.IScriptMethodDeclaration> Methods
        {
            get
            {
                return base.Methods.Where(m => m.IsStatic);
            }
        }

        public override bool HaveCastInformation
        {
            get
            {
                return false;
            }
        }

        public IEnumerable<ScriptMethod> Extensions
        {
            get { return methods.Where(m => !m.Method.IsStatic).OfType<ScriptMethod>(); }
        }
    }
}
