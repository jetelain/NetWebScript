using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace NetWebScript.Metadata
{
    public class DocumentReference
    {
        [XmlAttribute]
        public string Id { get; set; }

        [XmlAttribute]
        public string Filename { get; set; }
    }
}
