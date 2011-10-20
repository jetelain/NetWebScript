using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace NetWebScript.Remoting.Serialization
{
    internal class EquivalentSerializer : IObjectSerializer
    {
        private readonly IObjectSerializer equivalentSerializer;
        private readonly TypeConverter converter;
        private readonly Type type;

        internal EquivalentSerializer(Type type, Type equivalentType, IObjectSerializer equivalentSerializer)
        {
            this.type = type;
            this.converter = TypeDescriptor.GetConverter(equivalentType);
            this.equivalentSerializer = equivalentSerializer;
        }

        public void Serialize(object source, System.Runtime.Serialization.SerializationInfo target)
        {
            equivalentSerializer.Serialize(converter.ConvertFrom(source), target);
        }

        public object CreateAndDeserialize(System.Runtime.Serialization.SerializationInfo source)
        {
            return converter.ConvertTo(equivalentSerializer.CreateAndDeserialize(source), type);
        }

        public void WriteScriptStart(System.IO.TextWriter writer)
        {
            equivalentSerializer.WriteScriptStart(writer);
        }

        public void WriteScriptEnd(System.IO.TextWriter writer)
        {
            equivalentSerializer.WriteScriptEnd(writer);
        }
    }
}
