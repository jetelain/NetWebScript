using NetWebScript.JsClr.JsBuilder.JsSyntax;
using NetWebScript.JsClr.TypeSystem.Invoker;
using System;
using System.Globalization;

namespace NetWebScript.JsClr.TypeSystem.Serializers
{
    internal class BooleanSerializer : IValueSerializer
    {
        internal static readonly BooleanSerializer Instance = new BooleanSerializer();

        public JsToken LiteralValue(IValueSerializer type, object value, IRootInvoker converter)
        {
            return JsToken.Name(((bool)value) ? "true" : "false");
        }

    }
}
