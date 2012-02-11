using System.Collections.Generic;
using System.Reflection;
using NetWebScript.JsClr.JsBuilder.JsSyntax;
using NetWebScript.JsClr.JsBuilder.Pattern;
using NetWebScript.JsClr.TypeSystem.Invoker;
using NetWebScript.JsClr.ScriptAst;

namespace NetWebScript.JsClr.TypeSystem.Inlined
{
    class InlinedField : MappedField, IFieldInvoker
    {
        private readonly InlineFragment pattern;

        public InlinedField(IScriptType owner, FieldInfo field, string patternString)
            : base(owner, field)
        {
            this.pattern = new InlineFragment(patternString);
        }

        #region IScriptField Members

        public override string SlodId
        {
            get { return null; }
        }

        public override IFieldInvoker Invoker
        {
            get { return this; }
        }

        #endregion

        #region IFieldInvoker Members

        public JsBuilder.JsSyntax.JsToken WriteField(IInvocableField field, ScriptAst.ScriptFieldReferenceExpression fieldExpression, IRootInvoker converter)
        {
            var locals = new Dictionary<string, JsToken>();
            if (fieldExpression.Target != null)
            {
                locals.Add("this", fieldExpression.Target.Accept(converter));
            }
            return pattern.Execute(locals);
        }

        #endregion
    }
}
