using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Debugger.Interop;

namespace NetWebScript.Debug.Engine.Events
{
    class StepCompleteEvent : StoppingEvent, IDebugStepCompleteEvent2
    {
        public static readonly Guid IID = new Guid("{0F7F24C1-74D9-4EA6-A3EA-7EDB2D81441D}");
    }
}
