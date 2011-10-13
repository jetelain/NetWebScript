using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace NetWebScript.Script.Xml.Impl
{
    internal class NXmlNodeList : IXmlNodeList
    {
        private readonly XmlNodeList list;

        internal NXmlNodeList(XmlNodeList list)
        {
            this.list = list;
        }

        #region IXmlNodeList Members

        public IXmlNode this[int index]
        {
            get { return (IXmlNode)list[index]; }
        }

        public int Length
        {
            get { return list.Count; }
        }

        #endregion
    }
}
