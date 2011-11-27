using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Debugger.Interop;
using NetWebScript.Debug.Engine.Port;
using Microsoft.VisualStudio.OLE.Interop;
using NetWebScript.Debug.Server;
using NetWebScript.Debug.Engine.Debug;
using System.Diagnostics;

namespace NetWebScript.Debug.Engine.Script
{
    class ScriptProgram : IDebugProcess3, IDebugProgram3, IJSProgramCallback
    {
        private static readonly int windowsSessionId = System.Diagnostics.Process.GetCurrentProcess().SessionId;

        private readonly List<ScriptThread> threads = new List<ScriptThread>();
        private readonly List<ScriptModule> modules = new List<ScriptModule>();
        private readonly List<BoundBreakpoint> points = new List<BoundBreakpoint>();

        private readonly ScriptPort port;
        private readonly Guid processId;
        private readonly Guid programId;
        private readonly int processNum;
        private readonly DateTime started;
        private readonly IJSProgram program;
        private readonly String programName;

        private NWSEngine attached;

        public ScriptProgram(ScriptPort port,IJSProgram program)
        {
            this.port = port;
            this.processId = Guid.NewGuid();
            this.programId = Guid.NewGuid();
            this.processNum = program.Id;
            this.started = DateTime.Now;
            this.program = program;
            this.programName = program.Uri.LocalPath;
            program.RegisterCallback(this);

            foreach (ModuleInfo module in program.Modules)
            {
                modules.Add(new ScriptModule(module));
            }
        }

        ~ScriptProgram()
        {
            program.UnRegisterCallback(this);
        }

        internal IEnumerable<ScriptModule> Modules
        {
            get { return modules; }
        }

        internal ScriptPort Port
        {
            get { return port; }
        }

        #region IDebugProcess2 Members

        public int Attach(IDebugEventCallback2 pCallback, Guid[] rgguidSpecificEngines, uint celtSpecificEngines, int[] rghrEngineAttach)
        {
            Trace.TraceWarning("int Attach(IDebugEventCallback2 pCallback, Guid[] rgguidSpecificEngines, uint celtSpecificEngines, int[] rghrEngineAttach)");
            throw new NotImplementedException();
        }

        public int EnumPrograms(out IEnumDebugPrograms2 ppEnum)
        {
            ppEnum = new DebugPrograms(new IDebugProgram2[]{this});
            return Constants.S_OK;
        }

        [Obsolete]
        public int GetAttachedSessionName(out string pbstrSessionName)
        {
            // DEPRECATED, Never called by SDM
            pbstrSessionName = null;
            return Constants.E_NOTIMPL;
        }

        public int GetInfo(enum_PROCESS_INFO_FIELDS Fields, PROCESS_INFO[] pProcessInfo)
        {
            Trace.TraceInformation("IDebugProcess2.GetInfo "+Fields);
            /*enum_PROCESS_INFO_FIELDS.PIF_ATTACHED_SESSION_NAME is DEPRECATED*/
            if ((Fields & enum_PROCESS_INFO_FIELDS.PIF_BASE_NAME) != 0)
            {
                pProcessInfo[0].bstrBaseName = program.Uri.ToString();
                pProcessInfo[0].Fields |= enum_PROCESS_INFO_FIELDS.PIF_BASE_NAME;
            }
            if ((Fields & enum_PROCESS_INFO_FIELDS.PIF_FILE_NAME) != 0)
            {
                pProcessInfo[0].bstrFileName = program.Uri.ToString();
                pProcessInfo[0].Fields |= enum_PROCESS_INFO_FIELDS.PIF_FILE_NAME;
            }
            if ((Fields & enum_PROCESS_INFO_FIELDS.PIF_TITLE) != 0)
            {
                pProcessInfo[0].bstrTitle = program.Uri.LocalPath;
                pProcessInfo[0].Fields |= enum_PROCESS_INFO_FIELDS.PIF_TITLE;
            }
            if ((Fields & enum_PROCESS_INFO_FIELDS.PIF_PROCESS_ID) != 0)
            {
                pProcessInfo[0].ProcessId.ProcessIdType = (int)enum_AD_PROCESS_ID.AD_PROCESS_ID_SYSTEM;
                pProcessInfo[0].ProcessId.dwProcessId = ProcessNum;
                pProcessInfo[0].Fields |= enum_PROCESS_INFO_FIELDS.PIF_PROCESS_ID;
            }
            if ((Fields & enum_PROCESS_INFO_FIELDS.PIF_FLAGS) != 0)
            {
                if (attached != null)
                {
                    pProcessInfo[0].Flags = enum_PROCESS_INFO_FLAGS.PIFLAG_PROCESS_RUNNING | enum_PROCESS_INFO_FLAGS.PIFLAG_DEBUGGER_ATTACHED;
                    Trace.TraceInformation("=>PIFLAG_PROCESS_RUNNING|PIFLAG_DEBUGGER_ATTACHED");
                }
                else
                {
                    pProcessInfo[0].Flags = enum_PROCESS_INFO_FLAGS.PIFLAG_PROCESS_RUNNING;
                    Trace.TraceInformation("=>PIFLAG_PROCESS_RUNNING");
                }
                pProcessInfo[0].Fields |= enum_PROCESS_INFO_FIELDS.PIF_FLAGS;
            }
            if ((Fields & enum_PROCESS_INFO_FIELDS.PIF_CREATION_TIME) != 0)
            {
                ulong fileTime = (ulong)started.ToFileTime();
                pProcessInfo[0].CreationTime.dwHighDateTime = (uint)(fileTime >> 32);
                pProcessInfo[0].CreationTime.dwLowDateTime = (uint)(fileTime);
                pProcessInfo[0].Fields |= enum_PROCESS_INFO_FIELDS.PIF_CREATION_TIME;
            }
            if ((Fields & enum_PROCESS_INFO_FIELDS.PIF_SESSION_ID) != 0)
            {
                pProcessInfo[0].dwSessionId = (uint)windowsSessionId;
                pProcessInfo[0].Fields |= enum_PROCESS_INFO_FIELDS.PIF_SESSION_ID;
            }
            return Constants.S_OK;
        }

        public int GetName(enum_GETNAME_TYPE gnType, out string pbstrName)
        {
            Trace.TraceInformation("IDebugProcess2.GetName");
            switch (gnType)
            {
                case enum_GETNAME_TYPE.GN_BASENAME:
                    pbstrName = program.Uri.ToString();
                    break;
                case enum_GETNAME_TYPE.GN_FILENAME:
                    pbstrName = program.Uri.ToString();
                    break;
                case enum_GETNAME_TYPE.GN_MONIKERNAME:
                    pbstrName = "NWS";
                    break;
                case enum_GETNAME_TYPE.GN_NAME:
                    pbstrName = program.Uri.LocalPath;
                    break;
                case enum_GETNAME_TYPE.GN_STARTPAGEURL:
                    pbstrName = program.Uri.ToString();
                    break;
                case enum_GETNAME_TYPE.GN_TITLE:
                    pbstrName = program.Uri.LocalPath;
                    break;
                case enum_GETNAME_TYPE.GN_URL:
                    pbstrName = program.Uri.ToString();
                    break;
                default:
                    pbstrName = null;
                    return Constants.E_UNEXPECTED;
            }
            return Constants.S_OK;
        }

        public int GetPhysicalProcessId(AD_PROCESS_ID[] pProcessId)
        {
            Trace.TraceInformation("IDebugProcess2.GetPhysicalProcessId");
            pProcessId[0].dwProcessId = ProcessNum;
            pProcessId[0].ProcessIdType = (int)enum_AD_PROCESS_ID.AD_PROCESS_ID_SYSTEM;
            return Constants.S_OK;
        }

        public int GetPort(out IDebugPort2 ppPort)
        {
            ppPort = port;
            return Constants.S_OK;
        }

        public int GetProcessId(out Guid pguidProcessId)
        {
            Trace.TraceInformation("IDebugProcess2.GetProcessId");
            pguidProcessId = processId;
            return Constants.S_OK;
        }

        public int GetServer(out IDebugCoreServer2 ppServer)
        {
            Trace.TraceWarning("int GetServer(out IDebugCoreServer2 ppServer)");
            throw new NotImplementedException();
        }

        #endregion

        #region IDebugProgram2 Members

        public int Attach(IDebugEventCallback2 pCallback)
        {
            Trace.TraceWarning("int Attach(IDebugEventCallback2 pCallback)");
            throw new NotImplementedException();
        }

        public int CanDetach()
        {
            Trace.TraceInformation("IDebugProgram2.CanDetach");
            if (attached == null)
            {
                return Constants.S_FALSE;
            }
            return Constants.S_OK;
        }

        public int CauseBreak()
        {
            Trace.TraceWarning("int CauseBreak()");
            throw new NotImplementedException();
        }

        public int Continue(IDebugThread2 pThread)
        {
            ScriptThread thread = (ScriptThread)pThread;
            if (thread.IsBreaked())
            {
                thread.Continue();
                return Constants.S_OK;
            }
            return Constants.E_UNEXPECTED;
        }

        public int Detach()
        {
            Trace.TraceInformation("IDebugProgram2.Detach");
            if (attached == null)
            {
                return Constants.E_UNEXPECTED;
            }

            Callback callback = attached.Callback;

            program.DetachAll();
            points.Clear();

            attached.Detached(this);
            attached = null;

            callback.SendProgramDestroyEvent(this, 0);

            return Constants.S_OK;
        }

        public int EnumCodeContexts(IDebugDocumentPosition2 pDocPos, out IEnumDebugCodeContexts2 ppEnum)
        {
            Trace.TraceWarning("int EnumCodeContexts(IDebugDocumentPosition2 pDocPos, out IEnumDebugCodeContexts2 ppEnum)");
            throw new NotImplementedException();
        }

        public int EnumCodePaths(string pszHint, IDebugCodeContext2 pStart, IDebugStackFrame2 pFrame, int fSource, out IEnumCodePaths2 ppEnum, out IDebugCodeContext2 ppSafety)
        {
            Trace.TraceWarning("int EnumCodePaths(string pszHint, IDebugCodeContext2 pStart, IDebugStackFrame2 pFrame, int fSource, out IEnumCodePaths2 ppEnum, out IDebugCodeContext2 ppSafety)");
            throw new NotImplementedException();
        }

        public int EnumModules(out IEnumDebugModules2 ppEnum)
        {
            ppEnum = new DebugModules(modules.ToArray());
            return Constants.S_OK;
        }

        public int EnumThreads(out IEnumDebugThreads2 ppEnum)
        {
            lock (threads)
            {
                ppEnum = new DebugThreads(threads.ToArray());
            }
            return Constants.S_OK;
        }

        [Obsolete("Use Execute(IDebugThread2)")]
        public int Execute()
        {
            Trace.TraceWarning("int Execute()");
            throw new NotImplementedException();
        }

        public int GetDebugProperty(out IDebugProperty2 ppProperty)
        {
            Trace.TraceWarning("Program: int GetDebugProperty(out IDebugProperty2 ppProperty)");
            throw new NotImplementedException();
        }

        public int GetDisassemblyStream(enum_DISASSEMBLY_STREAM_SCOPE dwScope, IDebugCodeContext2 pCodeContext, out IDebugDisassemblyStream2 ppDisassemblyStream)
        {
            Trace.TraceWarning("int GetDisassemblyStream(enum_DISASSEMBLY_STREAM_SCOPE dwScope, IDebugCodeContext2 pCodeContext, out IDebugDisassemblyStream2 ppDisassemblyStream)");
            throw new NotImplementedException();
        }

        public int GetENCUpdate(out object ppUpdate)
        {
            Trace.TraceWarning("int GetENCUpdate(out object ppUpdate)");
            throw new NotImplementedException();
        }

        public int GetEngineInfo(out string pbstrEngine, out Guid pguidEngine)
        {
            pbstrEngine = NWSEngine.Name;
            pguidEngine = NWSEngine.EngineGuid;
            return Constants.S_OK;
        }

        public int GetMemoryBytes(out IDebugMemoryBytes2 ppMemoryBytes)
        {
            Trace.TraceWarning("int GetMemoryBytes(out IDebugMemoryBytes2 ppMemoryBytes)");
            throw new NotImplementedException();
        }

        public int GetName(out string pbstrName)
        {
            Trace.TraceInformation("IDebugProgram2.GetName");
            pbstrName = programName;
            return Constants.S_OK;
        }

        public int GetProcess(out IDebugProcess2 ppProcess)
        {
            ppProcess = this;
            return Constants.S_OK;
        }

        public int GetProgramId(out Guid pguidProgramId)
        {
            Trace.TraceInformation("IDebugProgram2.GetProgramId");
            pguidProgramId = programId;
            return Constants.S_OK;
        }

        public int Step(IDebugThread2 pThread, enum_STEPKIND kind, enum_STEPUNIT unit)
        {
            ScriptThread thread = (ScriptThread)pThread;
            return thread.Step(kind, unit);
        }

        public int Terminate()
        {
            return Constants.S_OK;
        }

        public int WriteDump(enum_DUMPTYPE DUMPTYPE, string pszDumpUrl)
        {
            Trace.TraceWarning("int WriteDump(enum_DUMPTYPE DUMPTYPE, string pszDumpUrl)");
            throw new NotImplementedException();
        }

        #endregion

        #region IDebugProgram3 Members


        public int ExecuteOnThread(IDebugThread2 pThread)
        {
            ScriptThread thread = (ScriptThread)pThread;
            thread.Continue();
            return Constants.S_OK;
        }

        #endregion

        #region IDebugProcess3 Members


        public int DisableENC(EncUnavailableReason reason)
        {
            Trace.TraceWarning("int DisableENC(EncUnavailableReason reason)");
            throw new NotImplementedException();
        }

        public int Execute(IDebugThread2 pThread)
        {
            Trace.TraceWarning("int Execute(IDebugThread2 pThread)");
            throw new NotImplementedException();
        }

        public int GetDebugReason(enum_DEBUG_REASON[] pReason)
        {
            Trace.TraceWarning("int GetDebugReason(enum_DEBUG_REASON[] pReason)");
            throw new NotImplementedException();
        }

        public int GetENCAvailableState(EncUnavailableReason[] pReason)
        {
            Trace.TraceWarning("int GetENCAvailableState(EncUnavailableReason[] pReason)");
            throw new NotImplementedException();
        }

        public int GetEngineFilter(GUID_ARRAY[] pEngineArray)
        {
            Trace.TraceWarning("int GetEngineFilter(GUID_ARRAY[] pEngineArray)");
            throw new NotImplementedException();
        }

        public int GetHostingProcessLanguage(out Guid pguidLang)
        {
            Trace.TraceWarning("int GetHostingProcessLanguage(out Guid pguidLang)");
            throw new NotImplementedException();
        }

        public int SetHostingProcessLanguage(ref Guid guidLang)
        {
            Trace.TraceWarning("int SetHostingProcessLanguage(ref Guid guidLang)");
            throw new NotImplementedException();
        }

        #endregion

        internal void Attach(NWSEngine engine)
        {
            this.attached = engine;
        }

        internal void RegisterBreakPoint(BoundBreakpoint point)
        {
            lock (points)
            {
                points.Add(point);
            }
        }
        
        internal void UnRegisterBreakPoint(BoundBreakpoint point)
        {
            lock (points)
            {
                points.Remove(point);
            }
        }

        internal BoundBreakpoint GetPoint(String id)
        {
            return points.FirstOrDefault(p => p.Id == id);
        }

        internal DocumentContext GetContextOfPoint(String id)
        {
            DocumentContext point;
            foreach ( ScriptModule module in modules )
            {
                point = module.GetPoint(id);
                if (point != null)
                {
                    return point;
                }
            }
            return null;
        }

        internal void AddBreakPoint(string id)
        {
            program.AddBreakPoint(id);
        }

        internal void RemoveBreakPoint(string id)
        {
            program.RemoveBreakPoint(id);
        }

        internal NWSEngine Attached
        {
            get { return attached; }
        }

        internal uint ProcessNum
        {
            get { return (uint)processNum; }
        }

        internal IJSProgram JSProgram
        {
            get { return program; }
        }

        #region IJSProgramCallback Members

        public void OnNewThread(IJSThread obj)
        {
            ScriptThread thread = new ScriptThread(this, obj);
            lock (threads)
            {
                threads.Add(thread);
            }
            if (attached != null)
            {
                attached.Callback.SendThreadStart(this, thread);
            }
        }

        public void OnNewModule(ModuleInfo module)
        {
            var scriptModule = new ScriptModule(module);

            modules.Add(scriptModule);

            if (attached != null)
            {
                // If attached, notifies the Debug Engine,
                // that will do all required operations
                attached.OnNewModule(this, scriptModule);
            }
        }

        #endregion

        internal void SuspendedResume()
        {
        
        
        }

        public void OnModuleUpdate(ModuleInfo module)
        {
            // TODO: This ModuleUpdate approach is not really thread safe, it's a temporary solution to avoid VS restart 
            // after each module compilation...

            var scriptModule = modules.FirstOrDefault(m => m.Uri == module.Uri);
            if (scriptModule == null)
            {
                OnNewModule(module);
            }
            else
            {
                scriptModule.UpdateMetadata(module);
                if (attached != null)
                {
                    // If attached, notifies the Debug Engine,
                    // that will do all required operations
                    attached.OnModuleUpdate(this, scriptModule);
                }
            }
        }
    }
}
