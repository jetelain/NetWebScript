using NetWebScript.JsClr.JsBuilder.JsSyntax;
using NetWebScript.JsClr.TypeSystem.Invoker;
using System;
using System.Globalization;

namespace NetWebScript.JsClr.TypeSystem.Serializers
{
    internal class CharSerializer : IValueSerializer
    {
        internal static readonly CharSerializer Instance = new CharSerializer();

        public JsToken LiteralValue(IScriptType type, object value, IRootInvoker converter)
        {
            return JsToken.Name(Convert.ToString((int)((char)value),CultureInfo.InvariantCulture));
        }

    }
}
