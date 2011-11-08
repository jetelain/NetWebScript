using System;

namespace NetWebScript.Script.Xml
{
    [Imported(IgnoreNamespace = true)]
    public interface IXmlNodeList
    {
        [IntrinsicProperty]
        IXmlNode this[int index] { get; }

        [IntrinsicProperty]
        int Length { get; }
    }
}

