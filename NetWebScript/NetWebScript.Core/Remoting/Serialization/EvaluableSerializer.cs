using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Globalization;
using NetWebScript.Metadata;
using NetWebScript.Script;

namespace NetWebScript.Remoting.Serialization
{
    public class EvaluableSerializer
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

            if (type.IsEnum)
            {
                writer.Write(Convert.ToInt32(value, CultureInfo.InvariantCulture).ToString(CultureInfo.InvariantCulture));
                return;
            }

            var valueType = value as Type;
            if (valueType != null)
            {
                var key = CRefToolkit.GetCRef(valueType);
                var scriptType = cache.ScriptModules.SelectMany(m => m.Types.Where(t => t.CRef == key)).FirstOrDefault();
                writer.Write(scriptType.Name);
                return;
            }

            var func = value as JSFunction;
            if (func != null)
            {
                SerializeMethodReference(writer, func.Method);
                return;
            }

            var deleg = value as Delegate;
            if (deleg != null)
            {
                if (deleg.Target != null)
                {
                    SerializeMethodReference(writer, new Func<object, JSFunction, Delegate>(RuntimeHelper.CreateDelegate).Method);
                    writer.Write('(');
                    Serialize(writer, deleg.Target);
                    writer.Write(',');
                    SerializeMethodReference(writer, deleg.Method);
                    writer.Write(')');
                }
                else
                {
                    SerializeMethodReference(writer, deleg.Method);
                }
                return;
            }

            var serializer = cache.GetSerializer(type);
            if (serializer == null)
            {
                throw new Exception(string.Format("Unable to serialize a value of type '{0}'", type.FullName));
            }
            serializer.WriteScriptStart(writer);
            var data = new SerializationInfo(type, cache.Converter);
            serializer.Serialize(value, data);
            writer.Write('{');
            bool first = true;
            foreach (SerializationEntry entry in data)
            {
                if (first)
                {
                    first = false;
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

        private void SerializeMethodReference(TextWriter writer, System.Reflection.MethodBase method)
        {
            var key = CRefToolkit.GetCRef(method.DeclaringType);
            var scriptType = cache.ScriptModules.SelectMany(m => m.Types.Where(t => t.CRef == key)).FirstOrDefault();

            key = CRefToolkit.GetCRef(method);
            var scriptMethod = scriptType.Methods.FirstOrDefault(m => m.CRef == key);
            writer.Write(scriptType.Name);
            writer.Write('.');
            if (!method.IsStatic)
            {
                writer.Write("prototype.");
            }
            writer.Write(scriptMethod.Name);
        }

    }
}
