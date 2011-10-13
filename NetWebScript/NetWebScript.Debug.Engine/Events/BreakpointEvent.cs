using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Debugger.Interop;

namespace NetWebScript.Debug.Engine.Events
{
    class BreakpointEvent : StoppingEvent, IDebugBreakpointEvent2
    {
        public static readonly Guid IID = new Guid("{501C1E21-C557-48B8-BA30-A1EAB0BC4A74}");

        IEnumDebugBoundBreakpoints2 m_boundBreakpoints;

        public BreakpointEvent(IEnumDebugBoundBreakpoints2 boundBreakpoints)
        {
            m_boundBreakpoints = boundBreakpoints;
        }

        #region IDebugBreakpointEvent2 Members

        int IDebugBreakpointEvent2.EnumBreakpoints(out IEnumDebugBoundBreakpoints2 ppEnum)
        {
            ppEnum = m_boundBreakpoints;
            return Constants.S_OK;
        }

        #endregion
    }
}
