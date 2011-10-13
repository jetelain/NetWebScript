using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace NetWebScript.Metadata
{
    public class MethodBaseMetadata
    {
        public MethodBaseMetadata()
        {
            Points = new List<DebugPointMetadata>();
            Variables = new List<VariableMetadata>();
        }

        [XmlAttribute]
        public string Name { get; set; }

        //[XmlAttribute]
        //public string Slots { get; set; }

        [XmlAttribute]
        public string CRef { get; set; }

        [XmlIgnore]
        public TypeMetadata Type { get; set; }

        [XmlElement(ElementName="Point")]
        public List<DebugPointMetadata> Points { get; set; }

        [XmlElement(ElementName = "Variable")]
        public List<VariableMetadata> Variables { get; set; }


        [XmlIgnore]
        public string Id { get { return Type.Id + "-" + Name; } }

    }
}
