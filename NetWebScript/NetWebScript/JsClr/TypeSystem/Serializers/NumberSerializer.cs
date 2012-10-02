using NetWebScript.JsClr.JsBuilder.JsSyntax;
using NetWebScript.JsClr.TypeSystem.Invoker;
using System;
using System.Globalization;

namespace NetWebScript.JsClr.TypeSystem.Serializers
{
    internal class NumberSerializer : IValueSerializer
    {
        internal static readonly NumberSerializer Instance = new NumberSerializer();

        public JsToken LiteralValue(IValueSerializer type, object value, IRootInvoker converter)
        {
            var str = Convert.ToString(value,CultureInfo.InvariantCulture);
            if (str.StartsWith("-", StringComparison.Ordinal))
            {
                return new JsToken(JsPrecedence.Other, str);
            }
            return JsToken.Name(str);
        }

    }
}
