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
    class BoundBreakpoint : IDebugBoundBreakpoint2, IDebugBreakpointResolution2
    {
        private readonly PendingBreakpoint parent;
        private readonly ScriptProgram program;
        private readonly JSDebugPoint point;
        private readonly DocumentContext doc;
        private bool enabled = true;
        private bool deleted = false;

        public BoundBreakpoint(PendingBreakpoint parent, ScriptProgram program, JSDebugPoint point)
        {
            this.program = program;
            this.point = point;
            this.doc = new DocumentContext(point);
            this.parent = parent;

            program.AddBreakPoint(point);
            program.RegisterBreakPoint(this);
        }

        internal JSDebugPoint Point
        {
            get { return point; }
        }

        internal PendingBreakpoint Parent
        {
            get { return parent; }
        }

        internal DocumentContext Context
        {
            get { return doc; }
        }

        #region IDebugBoundBreakpoint2 Members

        public int Delete()
        {
            if (!deleted)
            {
                deleted = true;
                if (enabled)
                {
                    program.RemoveBreakPoint(point);
                }
                parent.OnBoundBreakpointDeleted(this);
                program.UnRegisterBreakPoint(this);
            }
            return Constants.S_OK;
        }

        public int Enable(int fEnable)
        {
            bool newEnabled = fEnable == 0 ? false : true;
            if (enabled != newEnabled)
            {
                enabled = newEnabled;
                if (enabled)
                {
                    program.AddBreakPoint(point);
                }
                else
                {
                    program.RemoveBreakPoint(point);
                }
            }
            return Constants.S_OK;
        }

        public int GetBreakpointResolution(out IDebugBreakpointResolution2 ppBPResolution)
        {
            ppBPResolution = this;
            return Constants.S_OK;
        }

        public int GetHitCount(out uint pdwHitCount)
        {
            Trace.TraceWarning("int GetHitCount(out uint pdwHitCount)");
            throw new NotImplementedException();
        }

        public int GetPendingBreakpoint(out IDebugPendingBreakpoint2 ppPendingBreakpoint)
        {
            ppPendingBreakpoint = parent;
            return Constants.S_OK;
        }

        public int GetState(enum_BP_STATE[] pState)
        {
            if (deleted)
            {
                pState[0] = enum_BP_STATE.BPS_DELETED;
            }
            else if (enabled)
            {
                pState[0] = enum_BP_STATE.BPS_ENABLED;
            }
            else
            {
                pState[0] = enum_BP_STATE.BPS_DISABLED;
            }
            return Constants.S_OK;
        }

        public int SetCondition(BP_CONDITION bpCondition)
        {
            Trace.TraceWarning("int SetCondition(BP_CONDITION bpCondition)");
            throw new NotImplementedException();
        }

        public int SetHitCount(uint dwHitCount)
        {
            Trace.TraceWarning("int SetHitCount(uint dwHitCount)");
            throw new NotImplementedException();
        }

        public int SetPassCount(BP_PASSCOUNT bpPassCount)
        {
            Trace.TraceWarning("int SetPassCount(BP_PASSCOUNT bpPassCount)");
            throw new NotImplementedException();
        }

        #endregion

        #region IDebugBreakpointResolution2 Members

        public int GetBreakpointType(enum_BP_TYPE[] pBPType)
        {
            pBPType[0] = enum_BP_TYPE.BPT_CODE;
            return Constants.S_OK;
        }

        public int GetResolutionInfo(enum_BPRESI_FIELDS dwFields, BP_RESOLUTION_INFO[] pBPResolutionInfo)
        {
            //TODO: if ((dwFields & enum_BPRESI_FIELDS.BPRESI_BPRESLOCATION) != 0) 

            if ((dwFields & enum_BPRESI_FIELDS.BPRESI_PROGRAM) != 0)
            {
                pBPResolutionInfo[0].pProgram = (IDebugProgram2)program;
                pBPResolutionInfo[0].dwFields |= enum_BPRESI_FIELDS.BPRESI_PROGRAM;
            }
            return Constants.S_OK;
        }

        #endregion

        internal ScriptProgram Program { get{return program;} }
    }
}
