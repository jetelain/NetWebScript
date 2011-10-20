using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace NetWebScript.Metadata
{
    [XmlRoot(Namespace="http://codeplex.com/nws", ElementName="Module")]
    public class ModuleMetadata
    {
        public ModuleMetadata()
        {
            Assemblies = new List<string>();
            Types = new List<TypeMetadata>();
            Documents = new List<DocumentReference>();
            Equivalents = new List<EquivalentMetadata>();
        }

        [XmlAttribute]
        public string Name { get; set; }

        [XmlElement(ElementName="Assembly")]
        public List<string> Assemblies { get; set; }

        [XmlElement(ElementName = "Type")]
        public List<TypeMetadata> Types { get; set; }

        [XmlElement(ElementName = "Document")]
        public List<DocumentReference> Documents { get; set; }

        [XmlElement(ElementName = "Equivalent")]
        public List<EquivalentMetadata> Equivalents { get; set; }

        public DocumentReference GetDocumentReference(String path)
        {
            var doc = Documents.FirstOrDefault(d => d.Filename == path);
            if (doc == null)
            {
                doc = new DocumentReference();
                doc.Id = String.Format("d{0}", Documents.Count + 1);
                doc.Filename = path;
                Documents.Add(doc);
            }
            return doc;
        }
    }
}
