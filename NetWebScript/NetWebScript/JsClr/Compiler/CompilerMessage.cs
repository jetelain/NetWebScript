using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetWebScript.JsClr.Compiler
{
    public class CompilerMessage
    {
        public string Message { get; set; }
        public MessageSeverity Severity { get; set; }
        public string Filename { get; set; }

        public int LineNumber { get; set; }
        public int LinePosition { get; set; }
        public int EndLineNumber { get; set; }
        public int EndLinePosition { get; set; }

        public override string ToString()
        {
            if (!string.IsNullOrEmpty(Filename))
            {
                return string.Format("{0}({1},{2}): {3}: {4}", Filename, LineNumber, LinePosition, Severity, Message);
            }
            else
            {
                return string.Format("{0}: {1}", Severity, Message);
            }
        }


    }
}
