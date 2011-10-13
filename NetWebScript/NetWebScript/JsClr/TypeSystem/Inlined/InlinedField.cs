using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.JsClr.TypeSystem.Invoker;
using System.Reflection;
using NetWebScript.JsClr.JsBuilder.Pattern;
using NetWebScript.JsClr.JsBuilder.JsSyntax;

namespace NetWebScript.JsClr.TypeSystem.Inlined
{
    class InlinedField : IScriptField, IFieldInvoker
    {
        private readonly FieldInfo field;
        private readonly IScriptType owner;
        private readonly InlineFragment pattern;

        public InlinedField(IScriptType owner, FieldInfo field, string patternString)
        {
            this.owner = owner;
            this.field = field;
            this.pattern = new InlineFragment(patternString);
        }


        #region IScriptField Members

        public string SlodId
        {
            get { return null; }
        }

        public System.Reflection.FieldInfo Field
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

        public JsBuilder.JsSyntax.JsToken WriteField(IScriptField field, Ast.FieldReferenceExpression fieldExpression, IRootInvoker converter)
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
