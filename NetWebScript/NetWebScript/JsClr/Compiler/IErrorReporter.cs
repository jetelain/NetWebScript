using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetWebScript.JsClr.Compiler
{
    public interface IErrorReporter
    {
        void Error(string p);

        void Warning(string p);

        void Info(string p);

        void Report(CompilerMessage message);
    }
}
