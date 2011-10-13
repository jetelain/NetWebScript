using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Debugger.Interop;
using System.Diagnostics;
using NetWebScript.Debug.Engine.Debug;
using NetWebScript.Debug.Engine.Script;
using NetWebScript.Debug.Engine.Events;

namespace NetWebScript.Debug.Engine
{
    [Guid(NWSEngine.ClsId)]
    [ComVisible(true)]
    public class NWSEngine : IDebugEngine2, IDebugEngineLaunch2, IDebugEngineProgram2
    {
        public const string ClsId = "0898B973-803F-4d43-82EC-A9044B873467";
        public const string Id = "88EBAE0F-1B09-4c57-9463-799D483E4097";
        public const string Name = "NetWebScript";
        public static readonly Guid EngineGuid = new Guid("{" + Id + "}");

        private Callback callback;

        private readonly List<PendingBreakpoint> pendings = new List<PendingBreakpoint>();
        private readonly List<ScriptProgram> programs = new List<ScriptProgram>();

        internal Callback Callback
        {
            get { return callback; }
        }

        internal IEnumerable<ScriptProgram> Programs
        {
            get { return programs; }
        }

        #region IDebugEngine2 Members

        // Attach the debug engine to a program. 
        int IDebugEngine2.Attach(IDebugProgram2[] rgpPrograms, IDebugProgramNode2[] rgpProgramNodes, uint celtPrograms, IDebugEventCallback2 dwCallback, enum_ATTACH_REASON dwReason)
        {
            Trace.TraceInformation("IDebugEngine2.Attach");

            if (celtPrograms != 1)
            {
                throw new ArgumentException("NWSEngine only expects to see one program in a process");
            }

            var program = rgpPrograms[0] as ScriptProgram;
            if (program == null)
            {
                throw new ArgumentException("NWSEngine only support NWS Transport");
            }

            Attach(dwCallback, program);

            return Constants.S_OK;
        }

        private void Attach(IDebugEventCallback2 dwCallback, ScriptProgram program)
        {
            if (callback == null)
            {
                callback = new Callback(this, dwCallback);
                callback.SendEngineCreateEvent();
            }

            programs.Add(program);
            program.Attach(this);

            callback.SendProgramCreateEvent(program);
        }



        // Requests that all programs being debugged by this DE stop execution the next time one of their threads attempts to run.
        // This is normally called in response to the user clicking on the pause button in the debugger.
        // When the break is complete, an AsyncBreakComplete event will be sent back to the debugger.
        int IDebugEngine2.CauseBreak()
        {
            foreach (ScriptProgram program in programs)
            {
                program.CauseBreak();
            }
            return Constants.S_OK;
        }

        // Called by the SDM to indicate that a synchronous debug event, previously sent by the DE to the SDM,
        // was received and processed. The only event the sample engine sends in this fashion is Program Destroy.
        // It responds to that event by shutting down the engine.
        int IDebugEngine2.ContinueFromSynchronousEvent(IDebugEvent2 eventObject)
        {
            // TODO
            Trace.TraceInformation("IDebugEngine2.ContinueFromSynchronousEvent");
            return Constants.S_OK;
        }

        // Creates a pending breakpoint in the engine. A pending breakpoint is contains all the information needed to bind a breakpoint to 
        // a location in the debuggee.
        int IDebugEngine2.CreatePendingBreakpoint(IDebugBreakpointRequest2 pBPRequest, out IDebugPendingBreakpoint2 ppPendingBP)
        {
            Trace.TraceInformation("IDebugEngine2.CreatePendingBreakpoint");

            PendingBreakpoint pending = new PendingBreakpoint(pBPRequest, this);
            lock (pendings)
            {
                this.pendings.Add(pending);
            }
            ppPendingBP = pending;
            return Constants.S_OK;
        }

        // Informs a DE that the program specified has been atypically terminated and that the DE should 
        // clean up all references to the program and send a program destroy event.
        int IDebugEngine2.DestroyProgram(IDebugProgram2 pProgram)
        {
            Trace.TraceInformation("IDebugEngine2.DestroyProgram");
            // Tell the SDM that the engine knows that the program is exiting, and that the
            // engine will send a program destroy. We do this because the Win32 debug api will always
            // tell us that the process exited, and otherwise we have a race condition.
            return (Constants.E_PROGRAM_DESTROY_PENDING);
        }

        // Gets the GUID of the DE.
        int IDebugEngine2.GetEngineId(out Guid guidEngine)
        {
            guidEngine = EngineGuid;
            return Constants.S_OK;
        }

        // Removes the list of exceptions the IDE has set for a particular run-time architecture or language.
        // The sample engine does not support exceptions in the debuggee so this method is not actually implemented.
        int IDebugEngine2.RemoveAllSetExceptions(ref Guid guidType)
        {
            return Constants.S_OK;
        }

        // Removes the specified exception so it is no longer handled by the debug engine.
        // The sample engine does not support exceptions in the debuggee so this method is not actually implemented.       
        int IDebugEngine2.RemoveSetException(EXCEPTION_INFO[] pException)
        {
            // The sample engine will always stop on all exceptions.
            return Constants.S_OK;
        }

        // Specifies how the DE should handle a given exception.
        // The sample engine does not support exceptions in the debuggee so this method is not actually implemented.
        int IDebugEngine2.SetException(EXCEPTION_INFO[] pException)
        {
            return Constants.S_OK;
        }

        // Sets the locale of the DE.
        // This method is called by the session debug manager (SDM) to propagate the locale settings of the IDE so that
        // strings returned by the DE are properly localized. The sample engine is not localized so this is not implemented.
        int IDebugEngine2.SetLocale(ushort wLangID)
        {
            return Constants.S_OK;
        }

        // A metric is a registry value used to change a debug engine's behavior or to advertise supported functionality. 
        // This method can forward the call to the appropriate form of the Debugging SDK Helpers function, SetMetric.
        int IDebugEngine2.SetMetric(string pszMetric, object varValue)
        {
            // The sample engine does not need to understand any metric settings.
            return Constants.S_OK;
        }

        // Sets the registry root currently in use by the DE. Different installations of Visual Studio can change where their registry information is stored
        // This allows the debugger to tell the engine where that location is.
        int IDebugEngine2.SetRegistryRoot(string pszRegistryRoot)
        {
            // The sample engine does not read settings from the registry.
            return Constants.S_OK;
        }

        #endregion

        #region IDebugEngineLaunch2 Members

        // Determines if a process can be terminated.
        int IDebugEngineLaunch2.CanTerminateProcess(IDebugProcess2 process)
        {
            return Constants.S_FALSE;
        }

        // Launches a process by means of the debug engine.
        // Normally, Visual Studio launches a program using the IDebugPortEx2::LaunchSuspended method and then attaches the debugger 
        // to the suspended program. However, there are circumstances in which the debug engine may need to launch a program 
        // (for example, if the debug engine is part of an interpreter and the program being debugged is an interpreted language), 
        // in which case Visual Studio uses the IDebugEngineLaunch2::LaunchSuspended method
        // The IDebugEngineLaunch2::ResumeProcess method is called to start the process after the process has been successfully launched in a suspended state.
        int IDebugEngineLaunch2.LaunchSuspended(string pszServer, IDebugPort2 port, string exe, string args, string dir, string env, string options, enum_LAUNCH_FLAGS launchFlags, uint hStdInput, uint hStdOutput, uint hStdError, IDebugEventCallback2 ad7Callback, out IDebugProcess2 process)
        {
            Trace.TraceInformation("IDebugEngineLaunch2.LaunchSuspended");
            
            var scriptPort = port as ScriptPort;
            if (scriptPort == null)
            {
                process = null;
                return Constants.E_FAIL;
            }

            // The exe is the root URI of program 
            var uri = new Uri(exe);

            var program = scriptPort.LaunchSuspended(uri);

            process = program;
            return Constants.S_OK;
        }

        // Resume a process launched by IDebugEngineLaunch2.LaunchSuspended
        int IDebugEngineLaunch2.ResumeProcess(IDebugProcess2 process)
        {
            Trace.TraceInformation("IDebugEngineLaunch2.ResumeProcess");
            
            var scriptProgram = process as ScriptProgram;
            if (scriptProgram == null)
            {
                return Constants.E_FAIL;
            }
            var port = scriptProgram.Port;

            port.SendProgramCreateEvent(scriptProgram);

            scriptProgram.SuspendedResume();

            return Constants.S_OK;
        }

        // This function is used to terminate a process that the SampleEngine launched
        // The debugger will call IDebugEngineLaunch2::CanTerminateProcess before calling this method.
        int IDebugEngineLaunch2.TerminateProcess(IDebugProcess2 process)
        {
            Trace.TraceInformation("IDebugEngineLaunch2.TerminateProcess");
            // TODO
            return Constants.S_OK;
        }

        #endregion

        #region IDebugEngineProgram2 Members

        // Stops all threads running in this program.
        // This method is called when this program is being debugged in a multi-program environment. When a stopping event from some other program 
        // is received, this method is called on this program. The implementation of this method should be asynchronous; 
        // that is, not all threads should be required to be stopped before this method returns. The implementation of this method may be 
        // as simple as calling the IDebugProgram2::CauseBreak method on this program.
        //
        // The sample engine only supports debugging native applications and therefore only has one program per-process
        public int Stop()
        {
            Trace.TraceInformation("IDebugEngineProgram2.Stop");
            return Constants.S_OK;
        }

        // WatchForExpressionEvaluationOnThread is used to cooperate between two different engines debugging 
        // the same process. The sample engine doesn't cooperate with other engines, so it has nothing
        // to do here.
        public int WatchForExpressionEvaluationOnThread(IDebugProgram2 pOriginatingProgram, uint dwTid, uint dwEvalFlags, IDebugEventCallback2 pExprCallback, int fWatch)
        {
            return Constants.S_OK;
        }

        // WatchForThreadStep is used to cooperate between two different engines debugging the same process.
        // The sample engine doesn't cooperate with other engines, so it has nothing to do here.
        public int WatchForThreadStep(IDebugProgram2 pOriginatingProgram, uint dwTid, int fWatch, uint dwFrame)
        {
            return Constants.S_OK;
        }

        #endregion

        #region Deprecated interface methods
        // These methods are not called by the Visual Studio debugger, so they don't need to be implemented

        int IDebugEngine2.EnumPrograms(out IEnumDebugPrograms2 programs)
        {
            Trace.Fail("This function is not called by the debugger");
            programs = null;
            return Constants.E_NOTIMPL;
        }

        public int Attach(IDebugEventCallback2 pCallback)
        {
            Trace.Fail("This function is not called by the debugger");
            return Constants.E_NOTIMPL;
        }

        public int GetProcess(out IDebugProcess2 process)
        {
            Trace.Fail("This function is not called by the debugger");
            process = null;
            return Constants.E_NOTIMPL;
        }

        public int Execute()
        {
            Trace.Fail("This function is not called by the debugger.");
            return Constants.E_NOTIMPL;
        }

        #endregion

        internal void Detached(ScriptProgram scriptProgram)
        {
            Trace.TraceInformation("NWSEngine.Detached");
            programs.Remove(scriptProgram);
        }

        internal void OnNewModule(ScriptProgram scriptProgram, ScriptModule scriptModule)
        {
            Trace.TraceInformation("NWSEngine.OnNewModule");
            callback.SendModuleLoaded(scriptProgram, scriptModule);
            lock (pendings)
            {
                foreach (var bp in pendings)
                {
                    bp.OnNewModule(scriptProgram, scriptModule);
                }
            }
        }
    }
}
