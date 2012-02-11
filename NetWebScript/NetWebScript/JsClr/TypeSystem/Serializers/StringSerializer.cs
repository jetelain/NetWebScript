using System;
using NetWebScript.JsClr.JsBuilder.JsSyntax;
using NetWebScript.JsClr.TypeSystem.Invoker;

namespace NetWebScript.JsClr.TypeSystem.Serializers
{
    internal class StringSerializer : IValueSerializer
    {
        internal static readonly StringSerializer Instance = new StringSerializer();

        public JsToken LiteralValue(IValueSerializer type, object value, IRootInvoker converter)
        {
            var strvalue = (String)value;
            if (strvalue != null)
            {
                return JsToken.LiteralString(strvalue);
            }
            return JsToken.Name("null");
        }
    }
}
