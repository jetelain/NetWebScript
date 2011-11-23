using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetWebScript.Script.HTML
{
    /// <summary>
    /// Interface of an IFrame element
    /// </summary>
    [Imported]
    public interface IIFrameElement : IHTMLElement
    {
        /// <summary>
        /// The window proxy for the nested browsing context.
        /// </summary>
        [IntrinsicProperty]
        IWindow ContentWindow { get; }

        /// <summary>
        /// The active document in the inline frame's nested browsing context.
        /// </summary>
        [IntrinsicProperty]
        IHTMLDocument ContentDocument { get; }

        /// <summary>
        /// Indicates whether to create borders between frames.
        /// </summary>
        [IntrinsicProperty]
        string FrameBorder {  get; set; }

        /// <summary>
        /// Indicates whether the browser should provide scrollbars for the frame.
        /// </summary>
        [IntrinsicProperty]
        string Scrolling { get; set; }

        /// <summary>
        /// Reflects the src HTML attribute, containing the address of the content to be embedded.
        /// </summary>
        [IntrinsicProperty]
        string Src { get; set; }
    }
}
