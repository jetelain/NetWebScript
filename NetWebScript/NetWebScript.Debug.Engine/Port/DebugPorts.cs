using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Debugger.Interop;

namespace NetWebScript.Debug.Engine.Port
{
    class DebugPorts : BaseEnumerable<IDebugPort2, IEnumDebugPorts2>, IEnumDebugPorts2
    {
        internal DebugPorts(IDebugPort2[] data)
            : base(data)
        {

        }

        public int Next(uint celt, IDebugPort2[] rgelt, ref uint celtFetched)
        {
            return Next(celt, rgelt, out celtFetched);
        }
    }
}
