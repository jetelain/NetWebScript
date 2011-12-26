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
            if (type.Type == typeof(int))
            {
                int intValue = (int)value;
                if (intValue > 0x1000)
                {
                    return JsToken.Name("0x"+intValue.ToString("X", CultureInfo.InvariantCulture));
                }
            }
            return JsToken.Name(Convert.ToString(value,CultureInfo.InvariantCulture));
        }

    }
}
