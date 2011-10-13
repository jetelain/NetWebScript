using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace NetWebScript.Metadata
{
    public class TypeMetadata
    {
        public TypeMetadata()
        {
            Methods = new List<MethodBaseMetadata>();
            Fields = new List<FieldMetadata>();
        }

        [XmlIgnore]
        public ModuleMetadata Module { get; set; }

        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public string BaseTypeName { get; set; }

        [XmlAttribute]
        public string CRef { get; set; }

        [XmlElement(ElementName="Method")]
        public List<MethodBaseMetadata> Methods { get; set; }

        [XmlElement(ElementName = "Field")]
        public List<FieldMetadata> Fields { get; set; }


        [XmlIgnore]
        public string Id { get { return Name; } }

         
    }
}
