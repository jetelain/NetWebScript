using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    }
}
