using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Runtime.Serialization;
using NetWebScript.Metadata;

namespace NetWebScript.Remoting.Serialization
{
    class ScriptTypeSerializer : SerializerBase
    {
        private readonly string copyMethod;
        private readonly TypeMetadata typeMetadata;
        //private readonly MethodBaseMetadata emptyCtor;

        public ScriptTypeSerializer(string copyMethod, IObjectSerializer parent, TypeMetadata scriptType, Type type)
            : base(parent,type)
        {
            this.copyMethod = copyMethod;
            this.typeMetadata = scriptType;

            //var ctorKey = CRefToolkit.GetCRef(type.GetConstructor(Type.EmptyTypes));
            //emptyCtor = scriptType.Methods.FirstOrDefault(c => c.CRef == ctorKey);

            foreach (var field in type.GetFields(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic))
            {
                var key = CRefToolkit.GetCRef(field);
                var meta = scriptType.Fields.FirstOrDefault(f => f.CRef == key);
                if ( meta != null )
                {
                    AddEntry(field, meta.Name);
                }
            }
        }

        public override void WriteScriptStart(System.IO.TextWriter writer)
        {
            writer.Write(copyMethod);
            writer.Write("(new ");
            writer.Write(typeMetadata.Name);
            writer.Write("(),");
        }

        public override void WriteScriptEnd(System.IO.TextWriter writer)
        {
            writer.Write(")");
        }
    }
}
