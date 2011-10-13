using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace NetWebScript.Script.Xml.Impl
{
    class NXmlNamedNodeMap : IXmlNamedNodeMap
    {
        private readonly XmlAttributeCollection collection;

        internal NXmlNamedNodeMap(XmlAttributeCollection collection)
        {
            this.collection = collection;
        }

        #region IXmlNamedNodeMap Members

        public IXmlNode GetNamedItem(string name)
        {
            return (IXmlNode)collection.GetNamedItem(name);
        }

        public IXmlNode Item(int index)
        {
            return (IXmlNode)collection[index];
        }

        public IXmlNode RemoveNamedItem(string name)
        {
            return (IXmlNode)collection.RemoveNamedItem(name);
        }

        public IXmlNode SetNamedItem(IXmlNode node)
        {
            return (IXmlNode)collection.SetNamedItem((XmlNode)node);
        }

        public int Length
        {
            get { return collection.Count; }
        }

        #endregion
    }
}
