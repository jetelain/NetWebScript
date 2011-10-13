using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Debugger.Interop;

namespace NetWebScript.Debug.Engine.Debug
{
    class CodeContext : IDebugCodeContext2
    {
        // TODO: implements something

        private readonly DocumentContext documentContext;

        internal CodeContext(DocumentContext documentContext)
        {
            this.documentContext = documentContext;
        }

        #region IDebugCodeContext2 Members

        public int Add(ulong dwCount, out IDebugMemoryContext2 ppMemCxt)
        {
            ppMemCxt = null;
            return Constants.E_NOTIMPL;
        }

        public int Compare(enum_CONTEXT_COMPARE Compare, IDebugMemoryContext2[] rgpMemoryContextSet, uint dwMemoryContextSetLen, out uint pdwMemoryContext)
        {
            pdwMemoryContext = 0;
            return Constants.E_NOTIMPL;
        }

        public int GetDocumentContext(out IDebugDocumentContext2 ppSrcCxt)
        {
            ppSrcCxt = documentContext;
            return Constants.S_OK;
        }

        public int GetInfo(enum_CONTEXT_INFO_FIELDS dwFields, CONTEXT_INFO[] pinfo)
        {
            return Constants.E_NOTIMPL;
        }

        public int GetLanguageInfo(ref string pbstrLanguage, ref Guid pguidLanguage)
        {
            return documentContext.GetLanguageInfo(ref pbstrLanguage, ref pguidLanguage);
        }

        public int GetName(out string pbstrName)
        {
            pbstrName = null;
            return Constants.E_NOTIMPL;
        }

        public int Subtract(ulong dwCount, out IDebugMemoryContext2 ppMemCxt)
        {
            ppMemCxt = null;
            return Constants.E_NOTIMPL;
        }

        #endregion
    }
}
