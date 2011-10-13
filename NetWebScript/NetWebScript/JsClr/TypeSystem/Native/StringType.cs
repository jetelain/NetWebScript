using System;
using System.Diagnostics.Contracts;
using NetWebScript.JsClr.JsBuilder.JsSyntax;
using NetWebScript.JsClr.TypeSystem.Helped;
using NetWebScript.JsClr.TypeSystem.Invoker;

namespace NetWebScript.JsClr.TypeSystem.Native
{
    class StringType : ScriptTypeHelped, IValueSerializer
    {
        public StringType(ScriptSystem system)
            : base(system, typeof(String), typeof(Equivalents.StringHelper))
        {
        }

        public override IValueSerializer Serializer
        {
            get
            {
                return this;
            }
        }

        public override string TypeId
        {
            get
            {
                return "String";
            }
        }

        public JsToken LiteralValue(IScriptType type, object value, IRootInvoker converter)
        {
            Contract.Assert(type == this);
            var strvalue = (String)value;
            if (strvalue != null)
            {
                return JsToken.LiteralString(strvalue);
            }
            return JsToken.Name("null");
        }
    }
}
