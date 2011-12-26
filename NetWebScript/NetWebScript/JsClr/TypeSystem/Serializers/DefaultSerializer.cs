using System;
using System.Reflection;
using NetWebScript.JsClr.TypeSystem.Invoker;

namespace NetWebScript.JsClr.TypeSystem.Serializers
{
    internal static class DefaultSerializer
    {
        internal static IValueSerializer GetSerializer(ScriptSystem system, Type type)
        {
            if (type.IsValueType)
            {
                if (type == typeof(char))
                {
                    return CharSerializer.Instance;
                }
                if (type == typeof(bool))
                {
                    return BooleanSerializer.Instance;
                }
                if (ScriptSystem.IsNumberType(type))
                {
                    return NumberSerializer.Instance;
                }
            }
            else
            {
                if (type == typeof(string))
                {
                    return StringSerializer.Instance;
                }

                if (typeof(MethodInfo).IsAssignableFrom(type))
                {
                    return new MethodBaseSerializer(system);
                }
            }
            return null;
        }
    }
}
