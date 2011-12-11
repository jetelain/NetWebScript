using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.JsClr.TypeSystem.Imported;

namespace NetWebScript.JsClr.TypeSystem.Standard
{
    public class ScriptExtenderType : ScriptType
    {
        public ScriptExtenderType(ScriptSystem system, Type type, Type extendedType)
            : base(system, type)
        {
            var extended = (ImportedType)system.GetScriptType(extendedType);
            extended.AddExtensions(Methods.Where(m => !m.Method.IsStatic).OfType<ScriptMethod>());

            // TODO: transfert interfaces impl
        }

        internal override IEnumerable<ScriptMethodBase> MethodsToWrite
        {
            get
            {
                return Methods.Where(m => m.Method.IsStatic);
            }
        }
    }
}
