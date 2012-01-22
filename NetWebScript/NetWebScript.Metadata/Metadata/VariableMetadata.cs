using System.Xml.Serialization;

namespace NetWebScript.Metadata
{
    public sealed class VariableMetadata
    {
        [XmlAttribute]
        public string CName { get; set; }

        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute(AttributeName = "CGen")]
        public bool CompilerGenerated { get; set; }
    }
}
