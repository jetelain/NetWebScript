using System;

namespace NetWebScript.Script.Xml
{
    [Imported(IgnoreNamespace = true)]
    public interface IXmlNamedNodeMap
    {
        IXmlNode GetNamedItem(string name);

        IXmlNode Item(int index);

        IXmlNode RemoveNamedItem(string name);

        IXmlNode SetNamedItem(IXmlNode node);

        [IntrinsicProperty]
        int Length { get; }
    }
}

