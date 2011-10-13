using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Debugger.Interop;
using NetWebScript.Debug.Engine.Script;

namespace NetWebScript.Debug.Engine.Events
{
    class ProgramDestroyEvent : SynchronousEvent, IDebugProgramDestroyEvent2
    {
        public static readonly Guid IID = new Guid("{E147E9E3-6440-4073-A7B7-A65592C714B5}");

        private readonly uint m_exitCode;
        private readonly ScriptProgram program;

        public ProgramDestroyEvent(ScriptProgram program, uint exitCode)
        {
            this.program = program;
            m_exitCode = exitCode;
        }

        internal ScriptProgram Program
        {
            get { return program; }
        }

        #region IDebugProgramDestroyEvent2 Members

        int IDebugProgramDestroyEvent2.GetExitCode(out uint exitCode)
        {
            exitCode = m_exitCode;
            return Constants.S_OK;
        }

        #endregion
    }
}
