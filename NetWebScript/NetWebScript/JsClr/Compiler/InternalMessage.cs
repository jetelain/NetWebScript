using System.Reflection;

namespace NetWebScript.JsClr.Compiler
{
    /// <summary>
    /// Output message from compiler, but using internal representation (Method+Offset)
    /// </summary>
    internal class InternalMessage
    {
        public MessageSeverity Severity { get; set; }

        public string Message { get; set; }

        public MethodBase Method { get; set; }

        public int? IlOffset { get; set; }
    }
}
