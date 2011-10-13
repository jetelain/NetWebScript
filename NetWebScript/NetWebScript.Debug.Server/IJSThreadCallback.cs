using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetWebScript.Debug.Server
{
    public interface IJSThreadCallback
    {
        void OnBreakpoint(String id, String stackXml);

        void OnStepDone(String id, String stackXml);

        void OnStopped();
    }
}
