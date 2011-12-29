using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Reflection;

namespace NetWebScript.Remoting.Serialization
{
    internal class EquivalentSerializer : IObjectSerializer
    {
        private readonly IObjectSerializer equivalentSerializer;
        private readonly MethodInfo toEquivalent;
        private readonly MethodInfo fromEquivalent;
        private readonly Type type;

        internal EquivalentSerializer(Type type, Type equivalentType, IObjectSerializer equivalentSerializer)
        {
            this.type = type;
            this.equivalentSerializer = equivalentSerializer;

            toEquivalent = equivalentType.GetMethod("op_Implicit", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, new[] { type }, null);
            fromEquivalent = equivalentType.GetMethod("op_Implicit", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, new[] { equivalentType }, null);
        }

        public void Serialize(object source, System.Runtime.Serialization.SerializationInfo target)
        {
            equivalentSerializer.Serialize(toEquivalent.Invoke(null, new[] { source }), target);
        }

        public object CreateAndDeserialize(System.Runtime.Serialization.SerializationInfo source)
        {
            return fromEquivalent.Invoke(null, new [] { equivalentSerializer.CreateAndDeserialize(source) });
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
