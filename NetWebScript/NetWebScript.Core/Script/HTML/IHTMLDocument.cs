using System;

namespace NetWebScript.Script.HTML
{
    /// <summary>
    /// HTML Document<br />
    /// See also : <a href="https://developer.mozilla.org/en/DOM/document">MDN Document reference.</a>
    /// </summary>
    [Imported]
    public interface IHTMLDocument
    {

        /// <summary>
        /// Closes a document stream for writing.
        /// </summary>
        void Close();

        /// <summary>
        /// Creates a new attribute node and returns it.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IHTMLAttribute CreateAttribute(string name);

        /// <summary>
        /// Creates a new document fragment.
        /// </summary>
        /// <returns></returns>
        IHTMLDocumentFragment CreateDocumentFragment();

        /// <summary>
        /// Creates a new element with the given tag name.
        /// </summary>
        /// <param name="tagName"></param>
        /// <returns></returns>
        IHTMLElement CreateElement(string tagName);

        /// <summary>
        /// Creates a text node.
        /// </summary>
        /// <param name="tagName"></param>
        /// <returns></returns>
        IHTMLElement CreateTextNode(string tagName);

        /// <summary>
        /// Executes a <a href="https://developer.mozilla.org/en/Midas">Midas</a> command.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="displayUserInterface"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        bool ExecCommand(string command, bool displayUserInterface, object value);

        /// <summary>
        /// Returns an object reference to the identified element.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IHTMLElement GetElementById(string id);

        /// <summary>
        /// Returns a list of elements with the given name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IHTMLElementCollection GetElementsByName(string name);

        /// <summary>
        /// Returns a list of elements with the given tag name.
        /// </summary>
        /// <param name="tagName"></param>
        /// <returns></returns>
        IHTMLElementCollection GetElementsByTagName(string tagName);

        /// <summary>
        /// Returns true if the focus is currently located anywhere inside the specified document.
        /// </summary>
        /// <returns></returns>
        bool HasFocus();

        /// <summary>
        /// Opens a document stream for writing.
        /// </summary>
        void Open();

        /// <summary>
        /// Returns true if the Midas command can be executed on the current range.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        bool QueryCommandEnabled(string command);

        /// <summary>
        /// Returns true if the Midas command is in a indeterminate state on the current range.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        bool QueryCommandIndeterm(string command);

        /// <summary>
        /// Returns true if the Midas command has been executed on the current range.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        bool QueryCommandState(string command);

        /// <summary>
        /// Reports whether or not the specified editor query command is supported by the browser.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        bool QueryCommandSupported(string command);

        /// <summary>
        /// Returns the current value of the current range for Midas command. As of Firefox 2.0.0.2, queryCommandValue will return an empty string when a command value has not been explicitly set.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        object QueryCommandValue(string command);

        /// <summary>
        /// Writes text to a document.
        /// </summary>
        /// <param name="text"></param>
        void Write(string text);

        /// <summary>
        /// Write a line of text to a document.
        /// </summary>
        /// <param name="text"></param>
        void Writeln(string text);

        /// <summary>
        /// Returns the currently focused element
        /// </summary>
        [IntrinsicProperty]
        IHTMLElement ActiveElement { get; }

        /// <summary>
        /// Returns the BODY node of the current document.
        /// </summary>
        [IntrinsicProperty]
        IHTMLElement Body { get; }

        /// <summary>
        /// Returns a semicolon-separated list of the cookies for that document or sets a single cookie.
        /// </summary>
        [IntrinsicProperty]
        string Cookie { get; set; }

        /// <summary>
        /// Gets/sets WYSYWIG editing capability of Midas. 
        /// </summary>
        [IntrinsicProperty]
        string DesignMode { get; set; }

        /// <summary>
        /// Returns the Document Type Definition (DTD) of the current document.
        /// </summary>
        [IntrinsicProperty]
        string Doctype { get; }

        /// <summary>
        /// Returns the Element that is a direct child of document. For HTML documents, this is normally the HTML element.
        /// </summary>
        [IntrinsicProperty]
        IHTMLElement DocumentElement { get; }

        /// <summary>
        /// Returns the domain of the current document.
        /// </summary>
        [IntrinsicProperty]
        string Domain { get; set; }

        /// <summary>
        /// Returns the DOM implementation associated with the current document.
        /// </summary>
        [IntrinsicProperty]
        IHTMLImplementation Implementation { get; }

        /// <summary>
        /// Returns the URI of the page that linked to this page.
        /// </summary>
        [IntrinsicProperty]
        string Referrer { get; }

        /// <summary>
        /// Returns the title of the current document.
        /// </summary>
        [IntrinsicProperty]
        string Title { get; set; }

        /// <summary>
        /// Returns a string containing the URL of the current document.
        /// </summary>
        [IntrinsicProperty]
        string URL { get; }
    }
}

