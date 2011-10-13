using System;

namespace NetWebScript.Script.Xml
{
    [Imported, IgnoreNamespace]
    public interface IXmlNode
    {
        IXmlNode AppendChild(IXmlNode child);

        IXmlNode CloneNode(bool deepClone);

        bool HasChildNodes();

        IXmlNode InsertBefore(IXmlNode child, IXmlNode refChild);

        IXmlNode RemoveChild(IXmlNode child);

        IXmlNode ReplaceChild(IXmlNode child, IXmlNode oldChild);

        IXmlNodeList SelectNodes(string xpath);

        IXmlNode SelectSingleNode(string xpath);

        string TransformNode(IXmlDocument stylesheet);

        [IntrinsicProperty]
        IXmlNamedNodeMap Attributes { get; }

        [IntrinsicProperty]
        string BaseName { get; }

        [IntrinsicProperty]
        IXmlNode[] ChildNodes { get; }

        [IntrinsicProperty]
        IXmlNode FirstChild { get; }

        [IntrinsicProperty]
        IXmlNode LastChild { get; }

        [IntrinsicProperty]
        IXmlNode NextSibling { get; }

        [IntrinsicProperty]
        string NodeName { get; }

        [IntrinsicProperty]
        XmlNodeType NodeType { get; }

        [IntrinsicProperty]
        string NodeValue { get; set; }

        [IntrinsicProperty]
        object OwnerDocument { get; }

        [IntrinsicProperty]
        IXmlNode ParentNode { get; }

        [IntrinsicProperty]
        string Prefix { get; }

        [IntrinsicProperty]
        IXmlNode PreviousSibling { get; }

        [IntrinsicProperty]
        string Text { get; }

        [IntrinsicProperty]
        string Xml { get; }
    }
}

