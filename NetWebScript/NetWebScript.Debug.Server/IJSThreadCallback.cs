using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetWebScript.Debug.Server
{
    public interface IJSThreadCallback
    {
        void OnBreakpoint(string id, string stackXml, JSDebugPoint point, JSStack stack);

        void OnStepDone(string id, string stackXml, JSDebugPoint point, JSStack stack);

        void OnStopped();

        void OnContinueDone();
    }
}
