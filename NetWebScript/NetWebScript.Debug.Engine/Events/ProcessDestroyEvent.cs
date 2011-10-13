using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Debugger.Interop;

namespace NetWebScript.Debug.Engine.Events
{
    class ProcessDestroyEvent : SynchronousEvent, IDebugProcessDestroyEvent2
    {
        public static readonly Guid IID = new Guid("{3E2A0832-17E1-4886-8C0E-204DA242995F}");
    }
}
