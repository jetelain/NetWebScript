using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Debugger.Interop;
using NetWebScript.Debug.Engine.Script;
using System.Diagnostics;
using NetWebScript.Metadata;

namespace NetWebScript.Debug.Engine.Debug
{
    class Frame : IDebugStackFrame2, IDebugExpressionContext2
    {
        private readonly ScriptProgram program;
        private readonly ScriptThread thread;
        private readonly DocumentContext point;
        private readonly String name;
        private readonly MethodBaseMetadata metadata;

        public Frame(ScriptProgram program, ScriptThread thread, DocumentContext point, String name, MethodBaseMetadata metadata)
        {
            this.program = program;
            this.thread = thread;
            this.point = point;
            this.name = name;
            this.metadata = metadata;
        }

        internal ScriptThread Thread
        {
            get { return thread; }
        }

        internal MethodBaseMetadata MethodMetadata
        {
            get { return metadata; }
        }

        internal Property Property { get; set; }

        private string DisplayName
        {
            get 
            {
                if (metadata != null)
                {
                    return metadata.CRef;
                }
                return name;
            }
        }

        #region IDebugStackFrame2 Members

        public int EnumProperties(enum_DEBUGPROP_INFO_FLAGS dwFields, uint nRadix, ref Guid guidFilter, uint dwTimeout, out uint elementsReturned, out IEnumDebugPropertyInfo2 ppEnum)
        {
            var result = Property.EnumChildren(dwFields, nRadix, ref guidFilter, 0, null, dwTimeout, out ppEnum);
            if (ppEnum != null)
            {
                ppEnum.GetCount(out elementsReturned);
            }
            else
            {
                elementsReturned = 0;
            }
            return result;
        }

        public int GetCodeContext(out IDebugCodeContext2 ppCodeCxt)
        {
            ppCodeCxt = point.CodeContext;
            return Constants.S_OK;
        }

        public int GetDebugProperty(out IDebugProperty2 ppProperty)
        {
            ppProperty = Property;
            return Constants.S_OK;
        }

        public int GetDocumentContext(out IDebugDocumentContext2 ppCxt)
        {
            ppCxt = point;
            return Constants.S_OK;
        }

        public int GetExpressionContext(out IDebugExpressionContext2 ppExprCxt)
        {
            ppExprCxt = this;
            return Constants.S_OK;
        }

        public int GetInfo(enum_FRAMEINFO_FLAGS dwFieldSpec, uint nRadix, FRAMEINFO[] pFrameInfo)
        {
            pFrameInfo[0] = GetFrameInfo(dwFieldSpec);
            return Constants.S_OK;
        }

        public int GetLanguageInfo(ref string pbstrLanguage, ref Guid pguidLanguage)
        {
            pbstrLanguage = "C# NetWebScript";
            pguidLanguage = Constants.CSharp;
            return Constants.S_OK;
        }

        public int GetName(out string pbstrName)
        {
            pbstrName = DisplayName;
            return Constants.S_OK;
        }

        public int GetPhysicalStackRange(out ulong paddrMin, out ulong paddrMax)
        {
            paddrMin = 0;
            paddrMax = 0;
            return Constants.S_OK;
        }

        public int GetThread(out IDebugThread2 ppThread)
        {
            ppThread = thread;
            return Constants.S_OK;
        }

        #endregion

        // Construct a FRAMEINFO for this stack frame with the requested information.
        public FRAMEINFO GetFrameInfo(enum_FRAMEINFO_FLAGS dwFieldSpec)
        {
            FRAMEINFO frameInfo = new FRAMEINFO();

            // The debugger is asking for the formatted name of the function which is displayed in the callstack window.
            // There are several optional parts to this name including the module, argument types and values, and line numbers.
            // The optional information is requested by setting flags in the dwFieldSpec parameter.
            if ((dwFieldSpec & enum_FRAMEINFO_FLAGS.FIF_FUNCNAME) != 0)
            {
                frameInfo.m_bstrFuncName = DisplayName;
                frameInfo.m_dwValidFields |= enum_FRAMEINFO_FLAGS.FIF_FUNCNAME;
            }

            // The debugger is requesting the name of the module for this stack frame.
            if ((dwFieldSpec & enum_FRAMEINFO_FLAGS.FIF_MODULE) != 0)
            {
                frameInfo.m_bstrModule = "unknown.js";
                frameInfo.m_dwValidFields |= enum_FRAMEINFO_FLAGS.FIF_MODULE;
            }

            // The debugger is requesting the range of memory addresses for this frame.
            // For the sample engine, this is the contents of the frame pointer.
            if ((dwFieldSpec & enum_FRAMEINFO_FLAGS.FIF_STACKRANGE) != 0)
            {
                frameInfo.m_addrMin = 0;
                frameInfo.m_addrMax = 0;
                frameInfo.m_dwValidFields |= enum_FRAMEINFO_FLAGS.FIF_STACKRANGE;
            }

            // The debugger is requesting the IDebugStackFrame2 value for this frame info.
            if ((dwFieldSpec & enum_FRAMEINFO_FLAGS.FIF_FRAME) != 0)
            {
                frameInfo.m_pFrame = this;
                frameInfo.m_dwValidFields |= enum_FRAMEINFO_FLAGS.FIF_FRAME;
            }

            // Does this stack frame of symbols loaded?
            if ((dwFieldSpec & enum_FRAMEINFO_FLAGS.FIF_DEBUGINFO) != 0)
            {
                frameInfo.m_fHasDebugInfo = point != null ? 1 : 0;
                frameInfo.m_dwValidFields |= enum_FRAMEINFO_FLAGS.FIF_DEBUGINFO;
            }

            // Is this frame stale?
            if ((dwFieldSpec & enum_FRAMEINFO_FLAGS.FIF_STALECODE) != 0)
            {
                frameInfo.m_fStaleCode = 0;
                frameInfo.m_dwValidFields |= enum_FRAMEINFO_FLAGS.FIF_STALECODE;
            }

            // The debugger would like a pointer to the IDebugModule2 that contains this stack frame.
            if ((dwFieldSpec & enum_FRAMEINFO_FLAGS.FIF_DEBUG_MODULEP) != 0)
            {
                if (point != null)
                {
                    frameInfo.m_pModule = point.Module;
                    frameInfo.m_dwValidFields |= enum_FRAMEINFO_FLAGS.FIF_DEBUG_MODULEP;
                }
            }
            return frameInfo;
        }

        #region IDebugExpressionContext2 Members

        public int ParseText(string pszCode, enum_PARSEFLAGS dwFlags, uint nRadix, out IDebugExpression2 ppExpr, out string pbstrError, out uint pichError)
        {
            pbstrError = "";
            pichError = 0;
            ppExpr = null;

            if (Property != null)
            {
                // Variables, paramètres
                Property p = Property.Children.FirstOrDefault(c => c.DisplayName == pszCode);
                if (p != null)
                {
                    ppExpr = new TrivialExpression(p);
                    return Constants.S_OK;
                }

            }
            pbstrError = "Unsupported Expression";
            pichError = (uint)pbstrError.Length;
            return Constants.S_FALSE;
        }

        #endregion
    }
}
