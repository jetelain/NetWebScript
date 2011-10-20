using System.Xml.Serialization;

namespace NetWebScript.Metadata
{
    public class EquivalentMetadata
    {
        [XmlAttribute]
        public string EquivalentCRef { get; set; }

        [XmlAttribute]
        public string CRef { get; set; }
    }
}
