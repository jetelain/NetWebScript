using System;

namespace NetWebScript.Script.HTML
{
    /// <summary>
    /// Browser window<br />
    /// See also : <a href="https://developer.mozilla.org/en/DOM/window">MDN Window reference.</a>
    /// </summary>
    [Imported]
    public interface IWindow
    {
        /// <summary>
        /// Closes the current window.
        /// </summary>
        void Close();

        /// <summary>
        /// Provides a secure means for one window to send a string of data to another window, which need not be within the same domain as the first, in a secure manner.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="targetOrigin"></param>
        void PostMessage(string message, string targetOrigin);

        /// <summary>
        /// Opens the Print Dialog to print the current document.
        /// </summary>
        void Print();

        /// <summary>
        /// Scrolls the window to a particular place in the document.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        void Scroll(int x, int y);

        /// <summary>
        /// Scrolls the document in the window by the given amount.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        void ScrollBy(int x, int y);

        /// <summary>
        /// Scrolls to a particular set of coordinates in the document.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        void ScrollTo(int x, int y);

        /// <summary>
        /// This property indicates whether the current window is closed or not.
        /// </summary>
        [IntrinsicProperty]
        bool Closed { get; }

        /// <summary>
        /// Gets/sets the status bar text for the given window.
        /// </summary>
        [IntrinsicProperty]
        string DefaultStatus { get; set; }

        /// <summary>
        /// Returns a reference to the document that the window contains.
        /// </summary>
        [IntrinsicProperty]
        IHTMLDocument Document { get; }

        /// <summary>
        /// Returns the element in which the window is embedded, or null if the window is not embedded.
        /// </summary>
        [IntrinsicProperty]
        IIFrameElement FrameElement { get; }

        /// <summary>
        /// Returns an array of the subframes in the current window.
        /// </summary>
        [IntrinsicProperty]
        IWindow[] Frames { get; }

        /// <summary>
        /// Gets/sets the location, or current URL, of the window object.
        /// </summary>
        [IntrinsicProperty]
        ILocation Location { get; }

        /// <summary>
        /// Returns a reference to the window that opened this current window.
        /// </summary>
        [IntrinsicProperty]
        IWindow Opener { get; }

        /// <summary>
        /// Returns a reference to the parent of the current window or subframe.
        /// </summary>
        [IntrinsicProperty]
        IWindow Parent { get; }

        /// <summary>
        /// Returns an object reference to the window object itself.
        /// </summary>
        [IntrinsicProperty]
        IWindow Self { get; }

        /// <summary>
        /// Gets/sets the text in the statusbar at the bottom of the browser.
        /// </summary>
        [IntrinsicProperty]
        string Status { get; set; }

        /// <summary>
        /// Returns a reference to the topmost window in the window hierarchy. 
        /// </summary>
        [IntrinsicProperty]
        IWindow Top { get; }
    }
}
