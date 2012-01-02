using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.Debug.Server;

namespace NetWebScript.Debug.App.ViewModel
{
    public class ThreadModel : IJSThreadCallback, IDisposable
    {
        private readonly IJSThread thread;
        private readonly ProgramModel program;

        public ThreadModel(ProgramModel program, IJSThread thread)
        {
            this.thread = thread;
            this.program = program;
            thread.RegisterCallback(this);
        }

        public void OnBreakpoint(string id, string stackXml, JSDebugPoint point, JSStack stack)
        {
            program.SetCurrentPointAsync(this, point, stack);
        }

        public void OnStepDone(string id, string stackXml, JSDebugPoint point, JSStack stack)
        {
            program.SetCurrentPointAsync(this, point, stack);
        }

        public void OnStopped()
        {
            program.Stopped(this);
        }

        public void Dispose()
        {
            thread.UnRegisterCallback(this);
        }

        internal void Continue()
        {
            thread.Continue();
        }

        internal void StepInto()
        {
            thread.StepInto();
        }

        internal void StepOut()
        {
            thread.StepOut();
        }

        internal void StepOver()
        {
            thread.StepOver();
        }
    }
}
