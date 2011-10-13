using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Globalization;

namespace NetWebScript.Remoting.Serialization
{
    internal class EvaluableSerializer
    {
        private readonly SerializerCache cache;

        public EvaluableSerializer(SerializerCache cache)
        {
            this.cache = cache;
        }

        public void Serialize(TextWriter writer, object value)
        {
            if (value == null)
            {
                writer.Write("null");
                return;
            }
            var strValue = value as String;
            if (strValue != null)
            {
                writer.Write('"');
                writer.Write(strValue.Replace("\\", "\\\\").Replace("\n", "\\n").Replace("\r", "\\r").Replace("\"", "\\\"").Replace("'", "\\'"));
                writer.Write('"');
                return;
            }
            if (value is bool)
            {
                writer.Write(((bool)value)?"true":"false");
                return;
            }
            if (value is double || value is int)
            {
                writer.Write(Convert.ToDouble(value, CultureInfo.InvariantCulture).ToString(CultureInfo.InvariantCulture));
                return;
            }
            var type = value.GetType();
            if (type.IsArray)
            {
                Array array = (Array)value;
                writer.Write('[');
                for (int i = 0; i < array.Length; ++i)
                {
                    if (i > 0)
                    {
                        writer.Write(',');
                    }
                    Serialize(writer, array.GetValue(i));
                }
                writer.Write(']');
                return;
            }


            var serializer = cache.GetSerializer(type);
            serializer.WriteScriptStart(writer);
            var data = new SerializationInfo(type, cache.Converter);
            serializer.Serialize(value, data);
            writer.Write('{');
            bool first = true;
            foreach (SerializationEntry entry in data)
            {
                if (first)
                {
                    first = true;
                }
                else
                {
                    writer.Write(',');
                }
                writer.Write(entry.Name);
                writer.Write(':');
                Serialize(writer, entry.Value);
            }
            writer.Write('}');
            serializer.WriteScriptEnd(writer);
        }

    }
}
