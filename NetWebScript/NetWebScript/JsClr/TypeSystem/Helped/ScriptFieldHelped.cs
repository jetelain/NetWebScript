using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using NetWebScript.JsClr.TypeSystem.Invoker;
using NetWebScript.JsClr.JsBuilder.JsSyntax;
using NetWebScript.JsClr.ScriptAst;

namespace NetWebScript.JsClr.TypeSystem.Helped
{
    class ScriptFieldHelped : MappedField, IFieldInvoker
    {
        private readonly IScriptField helper;

        public ScriptFieldHelped(IScriptType owner, FieldInfo field, IScriptField helper)
            : base(owner, field)
        {
            this.helper = helper;
        }

        #region IScriptField Members

        public override string SlodId
        {
            get { return helper.SlodId; }
        }

        public override IFieldInvoker Invoker
        {
            get { return this; }
        }

        #endregion

        #region IFieldInvoker Members

        public JsToken WriteField(IInvocableField field, ScriptAst.ScriptFieldReferenceExpression fieldExpression, IRootInvoker converter)
        {
            return helper.Invoker.WriteField(helper, fieldExpression, converter);
        }

        #endregion

    }
}
