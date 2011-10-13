using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace NetWebScript.Script.Xml.Impl
{
    class NXmlDocument : XmlDocument, IXmlDocument
    {
        public override XmlElement CreateElement(string prefix, string localName, string namespaceURI)
        {
            return new NXmlElement(prefix, localName, namespaceURI, this);
        }

        public override XmlAttribute CreateAttribute(string prefix, string localName, string namespaceURI)
        {
            return new NXmlAttribute(prefix, localName, namespaceURI, this);
        }

        public override XmlText CreateTextNode(string text)
        {
            return new NXmlTextNode(text, this);
        }

        #region IXmlNode Members

        public IXmlNode AppendChild(IXmlNode child)
        {
            return (IXmlNode)base.AppendChild((XmlNode)child);
        }

        public new IXmlNode CloneNode(bool deepClone)
        {
            return (IXmlNode)base.CloneNode(deepClone);
        }

        public new bool HasChildNodes()
        {
            return base.HasChildNodes;
        }

        public IXmlNode InsertBefore(IXmlNode child, IXmlNode refChild)
        {
            return (IXmlNode)InsertBefore((XmlNode)child, (XmlNode)refChild);
        }

        public IXmlNode RemoveChild(IXmlNode child)
        {
            return (IXmlNode)RemoveChild((XmlNode)child);
        }

        public IXmlNode ReplaceChild(IXmlNode child, IXmlNode oldChild)
        {
            return (IXmlNode)ReplaceChild((XmlNode)child, (XmlNode)oldChild);
        }

        public new IXmlNodeList SelectNodes(string xpath)
        {
            return new NXmlNodeList(base.SelectNodes(xpath));
        }

        public new IXmlNode SelectSingleNode(string xpath)
        {
            return (IXmlNode)base.SelectSingleNode(xpath);
        }

        public string TransformNode(IXmlDocument stylesheet)
        {
            throw new NotImplementedException();
        }

        public new IXmlNamedNodeMap Attributes
        {
            get { return new NXmlNamedNodeMap(base.Attributes); }
        }

        public string BaseName
        {
            get { throw new NotImplementedException(); }
        }

        public new IXmlNode[] ChildNodes
        {
            get { return ChildNodes.Select(n => (IXmlNode)n).ToArray(); }
        }

        public new IXmlNode FirstChild
        {
            get { return (IXmlNode)base.FirstChild; }
        }

        public new IXmlNode LastChild
        {
            get { return (IXmlNode)base.LastChild; }
        }

        public new IXmlNode NextSibling
        {
            get { return (IXmlNode)base.NextSibling; }
        }

        public string NodeName
        {
            get { return base.Name; }
        }

        public new XmlNodeType NodeType
        {
            get { return XmlNodeType.Document; }
        }

        public string NodeValue
        {
            get
            {
                return base.Value;
            }
            set
            {
                base.Value = value;
            }
        }

        public new object OwnerDocument
        {
            get { return base.OwnerDocument; }
        }

        public new IXmlNode ParentNode
        {
            get { return (IXmlNode)base.ParentNode; }
        }

        public new IXmlNode PreviousSibling
        {
            get { return (IXmlNode)base.PreviousSibling; }
        }

        public string Text
        {
            get { return base.InnerText; }
        }

        public string Xml
        {
            get { return base.OuterXml; }
        }

        #endregion

        #region IXmlDocument Members

        public new IXmlAttribute CreateAttribute(string name)
        {
            return (IXmlAttribute)base.CreateAttribute(name);
        }

        public IXmlAttribute CreateAttributeNS(string namespaceUri, string name)
        {
            return (IXmlAttribute)base.CreateAttribute(name, namespaceUri);
        }

        public IXmlNode CreateCDATASection(string data)
        {
            throw new NotImplementedException();
        }

        public new IXmlNode CreateComment(string text)
        {
            throw new NotImplementedException();
        }

        public new IXmlElement CreateElement(string tagName)
        {
            return (IXmlElement)base.CreateElement(tagName);
        }

        public IXmlElement CreateElementNS(string namespaceUri, string tagName)
        {
            return (IXmlElement)base.CreateElement(tagName, namespaceUri);
        }

        public new IXmlNode CreateEntityReference(string name)
        {
            throw new NotImplementedException();
        }

        public new IXmlNode CreateProcessingInstruction(string target, string data)
        {
            throw new NotImplementedException();
        }

        IXmlTextNode IXmlDocument.CreateTextNode(string text)
        {
            return (IXmlTextNode)CreateTextNode(text);
        }

        public new IXmlNodeList GetElementsByTagName(string tagName)
        {
            return new NXmlNodeList(base.GetElementsByTagName(tagName));
        }

        public string Doctype
        {
            get { throw new NotImplementedException(); }
        }

        public new IXmlElement DocumentElement
        {
            get { return (IXmlElement)base.DocumentElement; }
        }

        #endregion


    }
}
