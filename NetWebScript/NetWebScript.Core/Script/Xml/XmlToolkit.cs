using NetWebScript.Script.Xml.Impl;

namespace NetWebScript.Script.Xml
{
    /// <summary>
    /// Utilities functions to manipulate a <see cref="IXmlDocument"/> object.
    /// </summary>
    [ScriptAvailable]
    public static class XmlToolkit
    {
        /// <summary>
        /// Create a new <see cref="IXmlDocument"/> with a root element (with empty XML namespace).
        /// </summary>
        /// <param name="rootElementName">Root element name.</param>
        /// <returns>A new instance of IXmlDocument.</returns>
        [ScriptBody(Body= @"function(n) {
if (window.ActiveXObject) {
var d = new ActiveXObject('Microsoft.XMLDOM');
d.appendChild(d.createElement(n));
return d;
}
return document.implementation.createDocument('', n, null);
}")]
        public static IXmlDocument CreateDocument(string rootElementName)
        {
            var doc = new NXmlDocument();
            doc.AppendChild(doc.CreateElement(rootElementName));
            return doc;
        }

        /// <summary>
        /// Serialize a <see cref="IXmlDocument"/> as a string.
        /// </summary>
        /// <param name="document">Document to serialize.</param>
        /// <returns>Textual XML representation of document.</returns>
        [ScriptBody(Body = @"function(d) {
if (typeof XMLSerializer !== 'undefined') {
return (new XMLSerializer()).serializeToString(d);
}
return d.xml;
}")]
        public static string ToXml(IXmlDocument document)
        {
            return document.Xml;
        }
    }
}
