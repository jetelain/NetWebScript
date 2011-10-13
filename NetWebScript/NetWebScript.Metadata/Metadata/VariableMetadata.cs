using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace NetWebScript.Metadata
{
    public sealed class VariableMetadata
    {
        [XmlAttribute]
        public string CName { get; set; }

        [XmlAttribute]
        public string Name { get; set; }
    }
}
