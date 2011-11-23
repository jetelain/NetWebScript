using System;

namespace NetWebScript.Script.HTML
{
    /// <summary>
    /// HTML Element<br />
    /// See also : <a href="https://developer.mozilla.org/en/DOM/element">MDN DOM element reference.</a>
    /// </summary>
    [Imported]
    public interface IHTMLElement
    {
        /// <summary>
        /// Removes keyboard focus from the current element.
        /// </summary>
        void Blur();

        /// <summary>
        /// Simulates a click on the current element.
        /// </summary>
        void Click();

        /// <summary>
        /// Clone a node.
        /// </summary>
        /// <returns></returns>
        IHTMLElement CloneNode();

        /// <summary>
        /// Clone a node, and optionally, all of its contents.
        /// </summary>
        /// <param name="deep"></param>
        /// <returns></returns>
        IHTMLElement CloneNode(bool deep);

        /// <summary>
        /// Gives keyboard focus to the current element.
        /// </summary>
        void Focus();

        /// <summary>
        /// Retrieve the value of the named attribute from the current node.
        /// </summary>
        /// <param name="name">Attribute name</param>
        /// <returns></returns>
        object GetAttribute(string name);

        /// <summary>
        /// Retrieve the node representation of the named attribute from the current node.
        /// </summary>
        /// <param name="name">Attribute name</param>
        /// <returns></returns>
        IHTMLAttribute GetAttributeNode(string name);

        /// <summary>
        /// Retrieve a set of all descendant elements, of a particular tag name, from the current element.
        /// </summary>
        /// <param name="tagName">Tag name</param>
        /// <returns></returns>
        IHTMLElementCollection GetElementsByTagName(string tagName);

        /// <summary>
        /// Check if the element has any child nodes, or not.
        /// </summary>
        /// <returns></returns>
        bool HasChildNodes();

        /// <summary>
        /// Remove the named attribute from the current node.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        bool RemoveAttribute(string name);

        /// <summary>
        /// Remove the node representation of the named attribute from the current node.
        /// </summary>
        /// <param name="attribute"></param>
        /// <returns></returns>
        IHTMLAttribute RemoveAttributeNode(IHTMLAttribute attribute);

        /// <summary>
        /// Removes a child node from the current element.
        /// </summary>
        /// <param name="child"></param>
        /// <returns></returns>
        IHTMLElement RemoveChild(IHTMLElement child);

        /// <summary>
        /// Replaces one child node in the current element with another.
        /// </summary>
        /// <param name="newChild"></param>
        /// <param name="oldChild"></param>
        /// <returns></returns>
        IHTMLElement ReplaceChild(IHTMLElement newChild, IHTMLElement oldChild);

        /// <summary>
        /// Scrolls the page until the element gets into the view.
        /// </summary>
        void ScrollIntoView();

        /// <summary>
        /// Scrolls the page until the element gets into the view.
        /// </summary>
        /// <param name="alignTop"></param>
        void ScrollIntoView(bool alignTop);

        /// <summary>
        /// Set the value of the named attribute from the current node.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        void SetAttribute(string name, object value);

        /// <summary>
        /// Set the node representation of the named attribute from the current node.
        /// </summary>
        /// <param name="attribute"></param>
        /// <returns></returns>
        IHTMLAttribute SetAttributeNode(IHTMLAttribute attribute);

        /// <summary>
        /// All attributes associated with an element.
        /// </summary>
        [IntrinsicProperty]
        IHTMLAttributeCollection Attributes { get; }

        /// <summary>
        /// All child nodes of an element.
        /// </summary>
        [IntrinsicProperty]
        IHTMLElementCollection ChildNodes { get; }

        /// <summary>
        /// A live list of the current child elements.
        /// </summary>
        [IntrinsicProperty]
        IHTMLElementCollection Children { get; }

        /// <summary>
        /// Class of the element.
        /// </summary>
        [IntrinsicProperty]
        string ClassName { get; set; }

        /// <summary>
        /// The inner height of an element.
        /// </summary>
        [IntrinsicProperty]
        int ClientHeight { get; }

        /// <summary>
        /// The width of the left border of an element.
        /// </summary>
        [IntrinsicProperty]
        int ClientLeft { get; }

        /// <summary>
        /// The width of the top border of an element
        /// </summary>
        [IntrinsicProperty]
        int ClientTop { get; }

        /// <summary>
        /// The inner width of an element.
        /// </summary>
        [IntrinsicProperty]
        int ClientWidth { get; }

        /// <summary>
        /// The first direct child node of an element, or null if this element has no child nodes.
        /// </summary>
        [IntrinsicProperty]
        IHTMLElement FirstChild { get; }

        /// <summary>
        /// id of the element
        /// </summary>
        [IntrinsicProperty]
        string ID { get; set; }

        /// <summary>
        /// Markup and content of the element.
        /// </summary>
        [IntrinsicProperty]
        string InnerHTML { get; set; }

        /// <summary>
        /// The last direct child node of an element, or null if this element has no child nodes.
        /// </summary>
        [IntrinsicProperty]
        IHTMLElement LastChild { get; }

        /// <summary>
        /// The node immediately following the given one in the tree, or null if there is no sibling node.
        /// </summary>
        [IntrinsicProperty]
        IHTMLElement NextSibling { get; }

        /// <summary>
        /// The name of the node.
        /// </summary>
        [IntrinsicProperty]
        string NodeName { get; }

        /// <summary>
        /// A number representing the type of the node. Is always equal to 1 for DOM elements.
        /// </summary>
        [IntrinsicProperty]
        NetWebScript.Script.Xml.XmlNodeType NodeType { get; }

        /// <summary>
        /// The value of the node. Is always equal to null for DOM elements.
        /// </summary>
        [IntrinsicProperty]
        string NodeValue { get; }

        /// <summary>
        /// The height of an element, relative to the layout.
        /// </summary>
        [IntrinsicProperty]
        int OffsetHeight { get; }

        /// <summary>
        /// The distance from this element's left border to its offsetParent's left border.
        /// </summary>
        [IntrinsicProperty]
        int OffsetLeft { get; }

        /// <summary>
        /// The element from which all offset calculations are currently computed.
        /// </summary>
        [IntrinsicProperty]
        IHTMLElement OffsetParent { get; }

        /// <summary>
        /// The distance from this element's top border to its offsetParent's top border.
        /// </summary>
        [IntrinsicProperty]
        int OffsetTop { get; }

        /// <summary>
        /// The width of an element, relative to the layout.
        /// </summary>
        [IntrinsicProperty]
        int OffsetWidth { get; }

        /// <summary>
        /// The document that this node is in, or null if the node is not inside of one.
        /// </summary>
        [IntrinsicProperty]
        IHTMLDocument OwnerDocument { get; }

        /// <summary>
        /// The parent element of this node, or null if the node is not inside of a DOM Document.
        /// </summary>
        [IntrinsicProperty]
        IHTMLElement ParentNode { get; }

        /// <summary>
        /// The node immediately preceding the given one in the tree, or null if there is no sibling node.
        /// </summary>
        [IntrinsicProperty]
        IHTMLElement PreviousSibling { get; }

        /// <summary>
        /// The scroll view height of an element.
        /// </summary>
        [IntrinsicProperty]
        int ScrollHeight { get; }

        /// <summary>
        /// The left scroll offset of an element.
        /// </summary>
        [IntrinsicProperty]
        int ScrollLeft { get; set; }

        /// <summary>
        /// The top scroll offset of an element.
        /// </summary>
        [IntrinsicProperty]
        int ScrollTop { get; set; }

        /// <summary>
        /// The scroll view width of an element.
        /// </summary>
        [IntrinsicProperty]
        int ScrollWidth { get; }

        /// <summary>
        /// An object representing the declarations of an element's style attributes.
        /// </summary>
        [IntrinsicProperty]
        ICSSStyle Style { get; }

        /// <summary>
        /// The position of the element in the tabbing order.
        /// </summary>
        [IntrinsicProperty]
        int TabIndex { get; set; }

        /// <summary>
        /// The name of the tag for the given element.
        /// </summary>
        [IntrinsicProperty]
        string TagName { get; }

        /// <summary>
        /// A string that appears in a popup box when mouse is over the element.
        /// </summary>
        [IntrinsicProperty]
        string Title { get; set; }
    }
}

