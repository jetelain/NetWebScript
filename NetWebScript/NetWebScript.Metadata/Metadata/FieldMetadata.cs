using System.Xml.Serialization;

namespace NetWebScript.Metadata
{
    public class FieldMetadata
    {
        public FieldMetadata()
        {
        }

        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public string CRef { get; set; }

        [XmlAttribute(AttributeName="CGen")]
        public bool CompilerGenerated { get; set; }
    }
}
