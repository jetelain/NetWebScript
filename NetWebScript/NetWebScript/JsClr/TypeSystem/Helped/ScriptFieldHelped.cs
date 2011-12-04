using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using NetWebScript.JsClr.TypeSystem.Invoker;
using NetWebScript.JsClr.JsBuilder.JsSyntax;

namespace NetWebScript.JsClr.TypeSystem.Helped
{
    class ScriptFieldHelped : IScriptField, IFieldInvoker
    {
        private readonly IScriptType owner;
        private readonly IScriptField helper;
        private readonly FieldInfo field;

        public ScriptFieldHelped(IScriptType owner, FieldInfo field, IScriptField helper)
        {
            this.owner = owner;
            this.field = field;
            this.helper = helper;
        }

        #region IScriptField Members

        public string SlodId
        {
            get { return helper.SlodId; }
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
            get { return this; }
        }

        #endregion

        #region IFieldInvoker Members

        public JsToken WriteField(IScriptField field, ScriptAst.ScriptFieldReferenceExpression fieldExpression, IRootInvoker converter)
        {
            return helper.Invoker.WriteField(helper, fieldExpression, converter);
        }

        #endregion
    }
}
