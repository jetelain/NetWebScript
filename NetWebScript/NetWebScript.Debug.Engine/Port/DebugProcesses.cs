using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Debugger.Interop;

namespace NetWebScript.Debug.Engine.Port
{
    class DebugProcesses : BaseEnumerable<IDebugProcess2, IEnumDebugProcesses2>, IEnumDebugProcesses2
    {
        internal DebugProcesses(IDebugProcess2[] data)
            : base(data)
        {

        }

        public int Next(uint celt, IDebugProcess2[] rgelt, ref uint celtFetched)
        {
            return Next(celt, rgelt, out celtFetched);
        }
    }
}
