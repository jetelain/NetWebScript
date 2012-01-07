using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Debugger.Interop;
using NetWebScript.Debug.Engine.Script;
using System.Diagnostics;
using NetWebScript.Debug.Server;

namespace NetWebScript.Debug.Engine.Debug
{
    class DocumentContext : IDebugDocumentContext2
    {
        private readonly JSDebugPoint point;
        private readonly CodeContext codeContext;

        public DocumentContext(JSDebugPoint point)
        {
            this.point = point;
            this.codeContext = new CodeContext(this);
        }

        public String FileName
        {
            get
            {
                return point.FileName;
            }
        }

        public int StartCol
        {
            get
            {
                return point.StartCol;
            }
        }

        public int StartRow
        {
            get
            {
                return point.StartRow;
            }
        }

        public CodeContext CodeContext
        {
            get { return codeContext; }
        }

        #region IDebugDocumentContext2 Members

        // Compares this document context to a given array of document contexts.
        int IDebugDocumentContext2.Compare(enum_DOCCONTEXT_COMPARE Compare, IDebugDocumentContext2[] rgpDocContextSet, uint dwDocContextSetLen, out uint pdwDocContext)
        {
            dwDocContextSetLen = 0;
            pdwDocContext = 0;
            return Constants.E_NOTIMPL;
        }

        // Retrieves a list of all code contexts associated with this document context.
        // The engine sample only supports one code context per document context and 
        // the code contexts are always memory addresses.
        int IDebugDocumentContext2.EnumCodeContexts(out IEnumDebugCodeContexts2 ppEnumCodeCxts)
        {
            ppEnumCodeCxts = new CodeContextInfos(new[] { codeContext });
            return Constants.S_OK;
        }

        // Gets the document that contains this document context.
        // This method is for those debug engines that supply documents directly to the IDE. Since the sample engine
        // does not do this, this method returns E_NOTIMPL.
        int IDebugDocumentContext2.GetDocument(out IDebugDocument2 ppDocument)
        {
            ppDocument = null;
            return Constants.E_FAIL;
        }

        // Gets the language associated with this document context.
        // The language for this sample is always C++
        public int GetLanguageInfo(ref string pbstrLanguage, ref Guid pguidLanguage)
        {
            pbstrLanguage = "C# NetWebScript";
            pguidLanguage = Constants.CSharp;
            return Constants.S_OK;
        }

        // Gets the displayable name of the document that contains this document context.
        int IDebugDocumentContext2.GetName(enum_GETNAME_TYPE gnType, out string pbstrFileName)
        {
            pbstrFileName = point.FileName;
            return Constants.S_OK;
        }

        // Gets the source code range of this document context.
        // A source range is the entire range of source code, from the current statement back to just after the previous s
        // statement that contributed code. The source range is typically used for mixing source statements, including 
        // comments, with code in the disassembly window.
        // Sincethis engine does not support the disassembly window, this is not implemented.
        int IDebugDocumentContext2.GetSourceRange(TEXT_POSITION[] pBegPosition, TEXT_POSITION[] pEndPosition)
        {
            Trace.TraceWarning("int IDebugDocumentContext2.GetSourceRange(TEXT_POSITION[] pBegPosition, TEXT_POSITION[] pEndPosition)");
            throw new NotImplementedException();
        }

        // Gets the file statement range of the document context.
        // A statement range is the range of the lines that contributed the code to which this document context refers.
        int IDebugDocumentContext2.GetStatementRange(TEXT_POSITION[] pBegPosition, TEXT_POSITION[] pEndPosition)
        {
            pBegPosition[0].dwColumn = (uint)point.StartCol - 1;
            pBegPosition[0].dwLine = (uint)point.StartRow - 1;
            pEndPosition[0].dwColumn = (uint)point.EndCol - 1;
            pEndPosition[0].dwLine = (uint)point.EndRow - 1;
            return Constants.S_OK;
        }

        // Moves the document context by a given number of statements or lines.
        // This is used primarily to support the Autos window in discovering the proximity statements around 
        // this document context. 
        int IDebugDocumentContext2.Seek(int nCount, out IDebugDocumentContext2 ppDocContext)
        {
            ppDocContext = null;
            return Constants.E_NOTIMPL;
        }

        #endregion
    }
}
