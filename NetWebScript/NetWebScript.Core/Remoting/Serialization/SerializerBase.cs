using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Runtime.Serialization;
using System.IO;

namespace NetWebScript.Remoting.Serialization
{
    internal abstract class SerializerBase : IObjectSerializer
    {
        private readonly SerializerBase parent;
        private readonly Type type;
        private readonly List<Entry> entries = new List<Entry>();

        private class Entry
        {
            protected readonly FieldInfo field;
            protected readonly string name;

            public Entry(FieldInfo field, string name)
            {
                this.field = field;
                this.name = name;
            }
            
            public virtual void Serialize(object source, SerializationInfo target)
            {
                target.AddValue(name,field.GetValue(source));
            }
            public virtual void Deserialize(object target, SerializationInfo source)
            {
                field.SetValue(target, source.GetValue(name, field.FieldType));
            }
        }

        private class EntryString : Entry
        {
            public EntryString(FieldInfo field, string name) : base(field, name)
            {
            }
            public override void Serialize(object source, SerializationInfo target)
            {
                target.AddValue(name, (string)field.GetValue(source));
            }
            public override void Deserialize(object target, SerializationInfo source)
            {
                field.SetValue(target, source.GetString(name));
            }
        }

        public SerializerBase(IObjectSerializer parent, Type type)
        {
            this.parent = (SerializerBase)parent;
            this.type = type;
        }

        protected void AddEntry(FieldInfo field, string scriptName)
        {
            var fieldType = field.FieldType;
            if (fieldType == typeof(string))
            {
                entries.Add(new EntryString(field, scriptName));
            }
            else
            {
                entries.Add(new Entry(field, scriptName));
            }
        }

        #region IObjectSerializer Members

        public void Serialize(object source, SerializationInfo target)
        {
            if (parent != null)
            {
                parent.Serialize(source, target);
            }
            foreach (var entry in entries)
            {
                entry.Serialize(source, target);
            }
        }

        public object CreateAndDeserialize(SerializationInfo source)
        {
            var target = Activator.CreateInstance(type);
            Deserialize(target, source);
            return target;
        }

        internal void Deserialize(object target, SerializationInfo source)
        {
            if (parent != null)
            {
                parent.Deserialize(target, source);
            }
            foreach (var entry in entries)
            {
                entry.Deserialize(target, source);
            }
        }

        #endregion


        public abstract void WriteScriptStart(TextWriter writer);

        public abstract void WriteScriptEnd(TextWriter writer);
    }
}
