using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Debugger.Interop;
using NetWebScript.Debug.Engine.Events;
using NetWebScript.Debug.Engine.Script;
using NetWebScript.Debug.Engine.Debug;
using System.Diagnostics;

namespace NetWebScript.Debug.Engine
{
    /// <summary>
    /// Notification channel from Debug Engine to SDM (Visual Studio)
    /// </summary>
    internal class Callback
    {
        private readonly IDebugEventCallback2 callback;
        private readonly NWSEngine engine;

        internal Callback(NWSEngine engine, IDebugEventCallback2 callback)
        {
            this.callback = callback;
            this.engine = engine;
        }

        private void Send(IDebugEvent2 eventObject, Guid iidEvent, ScriptProgram program, ScriptThread thread)
        {
            uint attributes; 
            EngineUtils.RequireOk(eventObject.GetAttributes(out attributes));
            EngineUtils.RequireOk(callback.Event(engine, program, program, thread, eventObject, ref iidEvent, attributes));
        }

        /// <summary>
        /// Notifies SDM (Visual Studio) that a program has been attached to Debug Engine
        /// </summary>
        /// <param name="program"></param>
        internal void SendProgramCreateEvent(ScriptProgram program)
        {
            Trace.TraceInformation("Callback.SendProgramCreateEvent");
            ProgramCreateEvent eventObject = new ProgramCreateEvent();
            Send(eventObject, ProgramCreateEvent.IID, program, null);
        }

        /// <summary>
        /// Notifies SDM (Visual Studio) the instance of Debug Engine
        /// </summary>
        internal void SendEngineCreateEvent()
        {
            Trace.TraceInformation("Callback.SendEngineCreateEvent");
            EngineCreateEvent eventObject = new EngineCreateEvent(engine);
            Send(eventObject, EngineCreateEvent.IID, null, null);
        }

        /// <summary>
        /// Notifies SDM (Visual Studio) that a thread has started
        /// </summary>
        /// <param name="program">Program</param>
        /// <param name="thread">Thread</param>
        internal void SendThreadStart(ScriptProgram program, ScriptThread thread)
        {
            Trace.TraceInformation("Callback.SendThreadStart");
            ThreadStartEvent eventObject = new ThreadStartEvent();
            Send(eventObject, ThreadStartEvent.IID, program, thread);
        }

        /// <summary>
        /// Notifies SDM (Visual Studio) that a break point has been reached (and debugged thread has been paused)
        /// </summary>
        /// <param name="scriptProgram">Program</param>
        /// <param name="scriptThread">Thread</param>
        /// <param name="bp">Reached break point</param>
        internal void SendBreakpoint(ScriptProgram scriptProgram, ScriptThread scriptThread, BoundBreakpoint bp)
        {
            Trace.TraceInformation("Callback.SendBreakpoint");
            BreakpointEvent eventObject = new BreakpointEvent(new BoundBreakpoints(new IDebugBoundBreakpoint2[] { bp }));
            Send(eventObject, BreakpointEvent.IID, scriptProgram, scriptThread);
        }

        /// <summary>
        /// Notifies SDM (Visual Studio) that previously requested step operation has been done.
        /// </summary>
        /// <param name="scriptProgram">Program</param>
        /// <param name="scriptThread">Thread</param>
        internal void SendStepComplete(ScriptProgram scriptProgram, ScriptThread scriptThread)
        {
            Trace.TraceInformation("Callback.SendStepComplete");
            StepCompleteEvent eventObject = new StepCompleteEvent();
            Send(eventObject, StepCompleteEvent.IID, scriptProgram, scriptThread);
        }

        /// <summary>
        /// Notifies SDM (Visual Studio) that program has stopped
        /// </summary>
        /// <param name="scriptProgram">Program</param>
        /// <param name="code">Exit code of program</param>
        internal void SendProgramDestroyEvent(ScriptProgram scriptProgram, int code)
        {
            Trace.TraceInformation("Callback.SendProgramDestroyEvent");
            Send(new ProgramDestroyEvent(scriptProgram, (uint)code), ProgramDestroyEvent.IID, scriptProgram, null);
        }

        /// <summary>
        /// Notifies SDM (Visual Studio) that a new module has been loaded in program.
        /// </summary>
        /// <param name="scriptProgram">Program</param>
        /// <param name="module">New module</param>
        internal void SendModuleLoaded(ScriptProgram scriptProgram, ScriptModule module)
        {
            Trace.TraceInformation("Callback.SendModuleLoaded");
            ModuleEventLoad eventObject = new ModuleEventLoad(module, true);
            Send(eventObject, ModuleEventLoad.IID, scriptProgram, null);
        }

    }
}
