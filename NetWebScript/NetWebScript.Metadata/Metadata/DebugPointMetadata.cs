using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace NetWebScript.Metadata
{
    public class DebugPointMetadata
    {
        [XmlIgnore]
        public MethodBaseMetadata Method { get; set; }

        [XmlIgnore]
        public String Id { get { return Method.Id + "-" + Name; } }

        [XmlAttribute]
        public String Name { get; set; }

        [XmlAttribute]
        public String DocumentId { get; set; }

        [XmlAttribute]
        public int Offset { get; set; }

        [XmlAttribute]
        public int StartCol { get; set; }

        [XmlAttribute]
        public int StartRow { get; set; }

        [XmlAttribute]
        public int EndCol { get; set; }

        [XmlAttribute]
        public int EndRow { get; set; }
    }
}
