using NetWebScript.JsClr.JsBuilder.JsSyntax;
using NetWebScript.JsClr.TypeSystem.Invoker;
using System;
using System.Globalization;

namespace NetWebScript.JsClr.TypeSystem.Serializers
{
    internal class NumberSerializer : IValueSerializer
    {
        internal static readonly NumberSerializer Instance = new NumberSerializer();

        public JsToken LiteralValue(IScriptType type, object value, IRootInvoker converter)
        {
            return JsToken.Name(Convert.ToString(value,CultureInfo.InvariantCulture));
        }

    }
}
