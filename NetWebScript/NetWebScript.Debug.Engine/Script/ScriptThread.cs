using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Debugger.Interop;
using NetWebScript.Debug.Server;
using System.Diagnostics;
using NetWebScript.Debug.Engine.Debug;
using System.Xml;
using NetWebScript.Metadata;

namespace NetWebScript.Debug.Engine.Script
{
    class ScriptThread : IDebugThread2, IJSThreadCallback//, IDebugThread3
    {
        private readonly ScriptProgram program;
        private readonly IJSThread thread;
        private readonly int threadNum;

        private String name;
        private DocumentContext currentPoint;
        private String currentStackXml;
        private List<Frame> frames;

        public ScriptThread(ScriptProgram program, IJSThread thread)
        {
            this.program = program;
            this.threadNum = thread.Id;
            this.thread = thread;
            name = "Test Thread";
            thread.RegisterCallback(this);
        }

        ~ScriptThread()
        {
            thread.UnRegisterCallback(this);
        }


        #region IDebugThread2 Members

        public int CanSetNextStatement(IDebugStackFrame2 pStackFrame, IDebugCodeContext2 pCodeContext)
        {
            // We are not able to set next statement
            return Constants.S_FALSE;
        }

        public int EnumFrameInfo(enum_FRAMEINFO_FLAGS dwFieldSpec, uint nRadix, out IEnumDebugFrameInfo2 ppEnum)
        {
            if (frames == null)
            {
                frames = new List<Frame>();
                XmlDocument document = new XmlDocument();
                document.LoadXml(currentStackXml);
                foreach (XmlElement element in document.DocumentElement.SelectNodes("Frame"))
                {
                    String name = element.GetAttribute("Name");
                    String pointId = element.GetAttribute("Point");
                    DocumentContext point = null;
                    XmlElement p = (XmlElement)element.SelectSingleNode("P");
                    if (p != null)
                    {
                        point = currentPoint;
                    }
                    else
                    {
                        point = ResolveByPointId(pointId);
                    }
                    var metadata = ResolveMethodMetadata(name);

                    Frame frame = new Frame(program, this, point, name, metadata);
                    if (p != null)
                    {
                        frame.Property = new Property(frame, p);
                    }
                    frames.Add(frame);
                }
                if (frames.Count == 0)
                {
                    frames.Add(new Frame(program, this, currentPoint, "unknown()", null));
                }
                else
                {
                    frames.Reverse();
                }

            }
            // TODO: Be able to retreive full stack trace
            FRAMEINFO[] frameInfoArray = new FRAMEINFO[frames.Count];
            for (int i = 0; i < frames.Count; ++i)
            {
                frameInfoArray[i] = frames[i].GetFrameInfo(dwFieldSpec);
            }
            ppEnum = new FrameInfos(frameInfoArray);
            return Constants.S_OK;
        }

        private MethodBaseMetadata ResolveMethodMetadata(string name)
        {
            foreach (var module in program.Modules)
            {
                var method = module.ResolveMethod(name);
                if (method != null)
                {
                    return method;
                }
            }
            return null;
        }

        internal TypeMetadata ResolveTypeMetadata(string name)
        {
            foreach (var module in program.Modules)
            {
                var method = module.ResolveType(name);
                if (method != null)
                {
                    return method;
                }
            }
            return null;
        }

        internal DocumentContext ResolveByPointId(string name)
        {
            foreach (var module in program.Modules)
            {
                var point = module.ResolvePointById(name);
                if (point != null)
                {
                    return point;
                }
            }
            return null;
        }

        public int GetLogicalThread(IDebugStackFrame2 pStackFrame, out IDebugLogicalThread2 ppLogicalThread)
        {
            Trace.TraceWarning("int GetLogicalThread(IDebugStackFrame2 pStackFrame, out IDebugLogicalThread2 ppLogicalThread)");
            throw new NotImplementedException();
        }

        public int GetName(out string pbstrName)
        {
            pbstrName = name;
            return Constants.S_OK;
        }

        public int GetProgram(out IDebugProgram2 ppProgram)
        {
            ppProgram = program;
            return Constants.S_OK;
        }

        public int GetThreadId(out uint pdwThreadId)
        {
            pdwThreadId = (uint)threadNum;
            return Constants.S_OK;
        }

        public int GetThreadProperties(enum_THREADPROPERTY_FIELDS dwFields, THREADPROPERTIES[] ptp)
        {
            if ((dwFields & enum_THREADPROPERTY_FIELDS.TPF_ID) != 0)
            {
                ptp[0].dwThreadId = (uint)threadNum;
                ptp[0].dwFields |= enum_THREADPROPERTY_FIELDS.TPF_ID;
            }
            if ((dwFields & enum_THREADPROPERTY_FIELDS.TPF_SUSPENDCOUNT) != 0)
            {
                // sample debug engine doesn't support suspending threads
                ptp[0].dwSuspendCount = 0;
                ptp[0].dwFields |= enum_THREADPROPERTY_FIELDS.TPF_SUSPENDCOUNT;
            }
            if ((dwFields & enum_THREADPROPERTY_FIELDS.TPF_STATE) != 0)
            {
                ptp[0].dwThreadState = (uint)enum_THREADSTATE.THREADSTATE_RUNNING;
                ptp[0].dwFields |= enum_THREADPROPERTY_FIELDS.TPF_STATE;
            }
            if ((dwFields & enum_THREADPROPERTY_FIELDS.TPF_PRIORITY) != 0)
            {
                ptp[0].bstrPriority = "Normal";
                ptp[0].dwFields |= enum_THREADPROPERTY_FIELDS.TPF_PRIORITY;
            }
            if ((dwFields & enum_THREADPROPERTY_FIELDS.TPF_NAME) != 0)
            {
                ptp[0].bstrName = name;
                ptp[0].dwFields |= enum_THREADPROPERTY_FIELDS.TPF_NAME;
            }
            if ((dwFields & enum_THREADPROPERTY_FIELDS.TPF_LOCATION) != 0)
            {
                ptp[0].bstrLocation = "(unknown)";//GetCurrentLocation(true);
                ptp[0].dwFields |= enum_THREADPROPERTY_FIELDS.TPF_LOCATION;
            }

            return Constants.S_OK;
        }

        public int Resume(out uint pdwSuspendCount)
        {
            Trace.TraceWarning("int Resume(out uint pdwSuspendCount)");
            throw new NotImplementedException();
        }

        public int SetNextStatement(IDebugStackFrame2 pStackFrame, IDebugCodeContext2 pCodeContext)
        {
            Trace.TraceWarning("int SetNextStatement(IDebugStackFrame2 pStackFrame, IDebugCodeContext2 pCodeContext)");
            throw new NotImplementedException();
        }

        public int SetThreadName(string pszName)
        {
            this.name = pszName;
            return Constants.S_OK;
        }

        public int Suspend(out uint pdwSuspendCount)
        {
            Trace.TraceWarning("int Suspend(out uint pdwSuspendCount)");
            throw new NotImplementedException();
        }

        #endregion

        /*#region IDebugThread3 Members

        public int CanRemapLeafFrame()
        {
            Trace.TraceWarning("int CanRemapLeafFrame()");
            throw new NotImplementedException();
        }

        public int IsCurrentException()
        {
            Trace.TraceWarning("int IsCurrentException()");
            throw new NotImplementedException();
        }

        public int RemapLeafFrame()
        {
            Trace.TraceWarning("int RemapLeafFrame()");
            throw new NotImplementedException();
        }

        #endregion*/

        internal bool IsBreaked()
        {
            return thread.State == JSThreadState.Breaked;
        }

        internal void Continue()
        {
            if ( thread.State == JSThreadState.Breaked )
            {
                ClearCurrentPoint();
                thread.Continue();
            }
        }

        internal int Step(enum_STEPKIND kind, enum_STEPUNIT unit)
        {
            if (!IsBreaked())
            {
                return Constants.E_UNEXPECTED;
            }
            switch (kind)
            {
                case enum_STEPKIND.STEP_INTO:
                    thread.StepInto();
                    break;
                case enum_STEPKIND.STEP_OUT:
                    thread.StepOut();
                    break;
                case enum_STEPKIND.STEP_OVER:
                    thread.StepOver();
                    break;
                case enum_STEPKIND.STEP_BACKWARDS:
                default:
                    return Constants.E_NOTIMPL;
            }
            ClearCurrentPoint();
            return Constants.S_OK;
        }

        private void ClearCurrentPoint()
        {
            currentPoint = null;
        }

        private void SetCurrentPoint(DocumentContext point, String stackXml)
        {
            currentPoint = point;
            currentStackXml = stackXml;
            frames = null;
        }


        #region IJSThreadCallback Members

        public void OnBreakpoint(string id, String stackXml)
        {
            BoundBreakpoint bp = program.GetPoint(id);
            if (bp == null || program.Attached == null)
            {
                ClearCurrentPoint();
                Continue();
            }
            else
            {
                SetCurrentPoint(bp.Context, stackXml);
                program.Attached.Callback.SendBreakpoint(program, this, bp);
            }
        }

        public void OnStepDone(string id, String stackXml)
        {
            DocumentContext point = program.GetContextOfPoint(id);
            if (point == null || program.Attached == null)
            {
                Trace.TraceError("Point unknown: " + id);
                ClearCurrentPoint();
                Continue();
            }
            else
            {
                SetCurrentPoint(point, stackXml);
                program.Attached.Callback.SendStepComplete(program, this);
            }
        }

        public void OnStopped()
        {
            // FIXME: do relevant operations
        }

        #endregion
    }
}
