using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript;

namespace NetWebScript.Remoting.Serialization
{
    class AnonymousSerializer : SerializerBase
    {
        internal AnonymousSerializer(IObjectSerializer parent, AnonymousObjectAttribute attribute, Type type)
            :base(parent,type)
        {
            foreach (var field in type.GetFields(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.DeclaredOnly))
            {
                string name = CaseToolkit.GetMemberName(attribute.Convention, field.Name);
                AddEntry(field, name);
            }
        }

        public override void WriteScriptStart(System.IO.TextWriter writer)
        {
        }

        public override void WriteScriptEnd(System.IO.TextWriter writer)
        {
        }
    }
}
