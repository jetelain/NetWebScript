using System;

namespace NetWebScript.Script.Xml
{
    [IgnoreNamespace, Imported]
    public interface IXmlTextNode : IXmlNode
    {
        IXmlTextNode SplitText(int offset);

        [IntrinsicProperty]
        string Data { get; set; }

        [IntrinsicProperty]
        int Length { get; }
    }
}

