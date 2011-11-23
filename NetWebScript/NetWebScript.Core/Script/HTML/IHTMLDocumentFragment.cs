
namespace NetWebScript.Script.HTML
{
    [Imported]
    public interface IHTMLDocumentFragment
    {

        IHTMLElement AppendChild(IHTMLElement child);

        IHTMLDocumentFragment CloneNode();

        IHTMLElement CloneNode(bool deep);

        bool Contains(IHTMLElement element);

        IHTMLElement GetElementById(string id);

        IHTMLElementCollection GetElementsByTagName(string tagName);

        bool HasAttributes();

        bool HasChildNodes();

        IHTMLElement InsertBefore(IHTMLElement newChild);

        IHTMLElement InsertBefore(IHTMLElement newChild, IHTMLElement referenceChild);

        IHTMLElement RemoveChild(IHTMLElement child);

        IHTMLElement ReplaceChild(IHTMLElement newChild, IHTMLElement oldChild);

        [IntrinsicProperty]
        IHTMLAttributeCollection Attributes { get; }

        [IntrinsicProperty]
        IHTMLElementCollection ChildNodes { get; }

        [IntrinsicProperty]
        IHTMLElement FirstChild { get; }

        [IntrinsicProperty]
        IHTMLElement LastChild { get; }

        [IntrinsicProperty]
        IHTMLElement NextSibling { get; }

        [IntrinsicProperty]
        string NodeName { get; }

        [IntrinsicProperty]
        int NodeType { get; }

        [IntrinsicProperty]
        string NodeValue { get; }

        [IntrinsicProperty]
        IHTMLDocument OwnerDocument { get; }

        [IntrinsicProperty]
        IHTMLElement ParentNode { get; }

        [IntrinsicProperty]
        IHTMLElement PreviousSibling { get; }
    }
}

