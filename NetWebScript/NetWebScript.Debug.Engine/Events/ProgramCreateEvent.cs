using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Debugger.Interop;

namespace NetWebScript.Debug.Engine.Events
{
    // This interface is sent by the debug engine (DE) to the session debug manager (SDM) when a program is attached to.
    sealed class ProgramCreateEvent : AsynchronousEvent, IDebugProgramCreateEvent2
    {
        public static readonly Guid IID = new Guid("{96CD11EE-ECD4-4E89-957E-B5D496FC4139}");
    }
}
