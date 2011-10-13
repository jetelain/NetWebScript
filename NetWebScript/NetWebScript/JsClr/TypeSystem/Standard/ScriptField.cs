using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using NetWebScript.JsClr.TypeSystem.Invoker;

namespace NetWebScript.JsClr.TypeSystem.Standard
{
    class ScriptField : IScriptField
    {
        private readonly FieldInfo field;
        private readonly string slotId;
        private readonly ScriptType owner;

        internal ScriptField(ScriptSystem system, ScriptType owner, FieldInfo field)
        {
            this.owner = owner;
            this.field = field;
            this.slotId = system.CreateSplotId();
        }

        public string SlodId
        {
            get { return slotId; }
        }

        public IScriptType Owner
        {
            get { return owner; }
        }

        public FieldInfo Field
        {
            get { return field; }
        }

        public IFieldInvoker Invoker
        {
            get
            {
                if (owner.IsGlobals)
                {
                    return GlobalsInvoker.Instance;
                }
                return StandardInvoker.Instance;
            } 
        }
    }
}
