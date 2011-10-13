using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Debugger.Interop;

namespace NetWebScript.Debug.Engine.Events
{
    // The debug engine (DE) sends this interface to the session debug manager (SDM) when an instance of the DE is created.
    sealed class EngineCreateEvent : AsynchronousEvent, IDebugEngineCreateEvent2
    {
        public readonly static Guid IID = new Guid("{FE5B734C-759D-4E59-AB04-F103343BDD06}");
        private IDebugEngine2 m_engine;

        internal EngineCreateEvent(NWSEngine engine)
        {
            m_engine = engine;
        }

        int IDebugEngineCreateEvent2.GetEngine(out IDebugEngine2 engine)
        {
            engine = m_engine;
            return Constants.S_OK;
        }
    }
}
