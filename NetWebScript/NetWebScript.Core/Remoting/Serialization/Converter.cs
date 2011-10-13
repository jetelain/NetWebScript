using System;
using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Runtime.Serialization;
using NetWebScript;
using NetWebScript.Script;
using NetWebScript.Runtime;

namespace NetWebScript.Remoting.Serialization
{
    public sealed class Converter : IFormatterConverter
    {
        private readonly SerializerCache cache;

        internal Converter(SerializerCache cache)
        {
            this.cache = cache;
        }

        public object Ensure(object value, Type type)
        {
            if (value != null && !type.IsAssignableFrom(value.GetType()))
            {
                return Convert(value, type);
            }
            return value;
        }

        private object ConvertUnknownToType(JSObject source, Type type)
        {
            // Copy source content to a SerializationInfo object
            SerializationInfo sourceInfo = new SerializationInfo(type, this);
            SerializationHelper.Serialize(source, sourceInfo);
            // Simply use the appropriate serializer
            IObjectSerializer serializer = cache.GetSerializer(type);
            return serializer.CreateAndDeserialize(sourceInfo);
        }

        public object Convert(object value, Type type)
        {
            var unknown = value as JSObject;
            if (unknown != null)
            {
                return ConvertUnknownToType(unknown, type);
            }

            object[] array = value as object[];
            if (array != null)
            {
                if (type.IsArray)
                {
                    Type elementType = type.GetElementType();
                    Array correctArray = Array.CreateInstance(elementType, array.Length);
                    for (int i = 0; i < array.Length; ++i)
                    {
                        correctArray.SetValue(Ensure(array[i], elementType), i);
                    }
                    return correctArray;
                }
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(JSArray<>))
                {
                    Type elementType = type.GetGenericArguments()[0];
                    object correctArray = Activator.CreateInstance(type);
                    MethodInfo push = type.GetMethod("Push");
                    for (int i = 0; i < array.Length; ++i)
                    {
                        push.Invoke(correctArray, new[] { Ensure(array[i], elementType) });
                    }
                    return correctArray;
                }
                if (typeof(IList).IsAssignableFrom(type))
                {
                    IList list = (IList)Activator.CreateInstance(type);
                    for (int i = 0; i < array.Length; ++i)
                    {
                        list.Add(array[i]);
                    }
                    return list;
                }
            }

            return System.Convert.ChangeType(value, type, CultureInfo.InvariantCulture);
        }


        #region IFormatterConverter Members

        object IFormatterConverter.Convert(object value, TypeCode typeCode)
        {
            return System.Convert.ChangeType(value, typeCode, CultureInfo.InvariantCulture);
        }

        object IFormatterConverter.Convert(object value, Type type)
        {
            return Convert(value, type);
        }

        bool IFormatterConverter.ToBoolean(object value)
        {
            return System.Convert.ToBoolean(value, CultureInfo.InvariantCulture);
        }

        byte IFormatterConverter.ToByte(object value)
        {
            return System.Convert.ToByte(value, CultureInfo.InvariantCulture);
        }

        char IFormatterConverter.ToChar(object value)
        {
            return System.Convert.ToChar(value, CultureInfo.InvariantCulture);
        }

        DateTime IFormatterConverter.ToDateTime(object value)
        {
            return System.Convert.ToDateTime(value, CultureInfo.InvariantCulture);
        }

        decimal IFormatterConverter.ToDecimal(object value)
        {
            return System.Convert.ToDecimal(value, CultureInfo.InvariantCulture);
        }

        double IFormatterConverter.ToDouble(object value)
        {
            return System.Convert.ToDouble(value, CultureInfo.InvariantCulture);
        }

        short IFormatterConverter.ToInt16(object value)
        {
            return System.Convert.ToInt16(value, CultureInfo.InvariantCulture);
        }

        int IFormatterConverter.ToInt32(object value)
        {
            return System.Convert.ToInt32(value, CultureInfo.InvariantCulture);
        }

        long IFormatterConverter.ToInt64(object value)
        {
            return System.Convert.ToInt64(value, CultureInfo.InvariantCulture);
        }

        sbyte IFormatterConverter.ToSByte(object value)
        {
            return System.Convert.ToSByte(value, CultureInfo.InvariantCulture);
        }

        float IFormatterConverter.ToSingle(object value)
        {
            return System.Convert.ToSingle(value, CultureInfo.InvariantCulture);
        }

        string IFormatterConverter.ToString(object value)
        {
            return System.Convert.ToString(value, CultureInfo.InvariantCulture);
        }

        ushort IFormatterConverter.ToUInt16(object value)
        {
            return System.Convert.ToUInt16(value, CultureInfo.InvariantCulture);
        }

        uint IFormatterConverter.ToUInt32(object value)
        {
            return System.Convert.ToUInt32(value, CultureInfo.InvariantCulture);
        }

        ulong IFormatterConverter.ToUInt64(object value)
        {
            return System.Convert.ToUInt64(value, CultureInfo.InvariantCulture);
        }

        #endregion
    }
}
