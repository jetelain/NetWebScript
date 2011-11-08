using System;

namespace NetWebScript.Script.Xml
{
    [Imported(IgnoreNamespace=true)]
    public interface IXmlAttribute : IXmlNode
    {
        [IntrinsicProperty]
        string Name { get; }

        [IntrinsicProperty]
        bool Specified { get; }

        [IntrinsicProperty]
        string Value { get; set; }
    }
}

