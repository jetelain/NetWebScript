using System;

namespace NetWebScript.Script.Xml
{
    [IgnoreNamespace, Imported]
    public interface IXmlDocument : IXmlNode
    {
        IXmlAttribute CreateAttribute(string name);

        IXmlAttribute CreateAttributeNS(string namespaceUri, string name);

        IXmlNode CreateCDATASection(string data);

        IXmlNode CreateComment(string text);

        IXmlElement CreateElement(string tagName);

        IXmlElement CreateElementNS(string namespaceUri, string tagName);

        IXmlNode CreateEntityReference(string name);

        IXmlNode CreateProcessingInstruction(string target, string data);

        IXmlTextNode CreateTextNode(string text);

        IXmlNodeList GetElementsByTagName(string tagName);

        [IntrinsicProperty]
        string Doctype { get; }

        [IntrinsicProperty]
        IXmlElement DocumentElement { get; }
    }
}

