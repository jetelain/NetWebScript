using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.JsClr.ScriptWriter.Declaration;
using NetWebScript.JsClr.ScriptAst;
using NetWebScript.JsClr.TypeSystem.Invoker;

namespace NetWebScript.JsClr.TypeSystem.Remoting
{
    class TransparentField : IScriptFieldDeclaration, IInvocableField
    {
        private readonly TransparentType owner;

        public TransparentField(TransparentType owner)
        {
            this.owner = owner;
        }

        public string SlodId
        {
            get { return "$p"; }
        }

        public ScriptLiteralExpression InitialValue
        {
            get { return ScriptLiteralExpression.NullValue; }
        }

        public string PrettyName
        {
            get { return "realProxy"; }
        }

        public bool IsStatic
        {
            get { return false; }
        }

        public IInvocableType DeclaringType
        {
            get { return owner; }
        }

        public IFieldInvoker Invoker
        {
            get { return StandardInvoker.Instance; }
        }

        public string DisplayName
        {
            get { return "realProxy"; }
        }
    }
}
