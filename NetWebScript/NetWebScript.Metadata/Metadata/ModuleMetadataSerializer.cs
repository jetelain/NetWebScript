using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Xml;

namespace NetWebScript.Metadata
{
    public static class ModuleMetadataSerializer
    {
        private static readonly XmlSerializer serializer = new XmlSerializer(typeof(ModuleMetadata));

        public static void Write(TextWriter writer, ModuleMetadata metadata)
        {
            serializer.Serialize(writer, metadata);
        }

        public static ModuleMetadata Read(TextReader reader)
        {
            var metadata = (ModuleMetadata)serializer.Deserialize(reader);
            Consolidate(metadata);
            return metadata;
        }

        public static ModuleMetadata Read(System.Xml.XmlDocument document)
        {
            var metadata = (ModuleMetadata)serializer.Deserialize(new XmlNodeReader(document));
            Consolidate(metadata);
            return metadata;
        }

        private static void Consolidate(ModuleMetadata metadata)
        {
            foreach (var type in metadata.Types)
            {
                type.Module = metadata;
                foreach (var method in type.Methods)
                {
                    method.Type = type;
                    foreach (var point in method.Points)
                    {
                        point.Method = method;
                    }
                }
            }
        }


    }
}
