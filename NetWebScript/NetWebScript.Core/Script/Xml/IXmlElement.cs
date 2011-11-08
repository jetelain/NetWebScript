﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetWebScript.Script.Xml
{
    [Imported(IgnoreNamespace = true)]
    public interface IXmlElement : IXmlNode
    {
        void SetAttribute(string name, string value);
    }
}
