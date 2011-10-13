using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Debugger.Interop;

namespace NetWebScript.Debug.Engine.Events
{
    sealed class ThreadStartEvent : AsynchronousEvent, IDebugThreadCreateEvent2
    {
        public static readonly Guid IID = new Guid("{2090CCFC-70C5-491D-A5E8-BAD2DD9EE3EA}");
    }
}
