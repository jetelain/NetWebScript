using System;

namespace NetWebScript.Script.Xml
{
    [Imported(IgnoreNamespace = true)]
    public interface IXmlTextNode : IXmlNode
    {
        IXmlTextNode SplitText(int offset);

        [IntrinsicProperty]
        string Data { get; set; }

        [IntrinsicProperty]
        int Length { get; }
    }
}

