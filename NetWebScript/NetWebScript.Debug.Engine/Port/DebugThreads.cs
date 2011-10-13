using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Debugger.Interop;

namespace NetWebScript.Debug.Engine.Port
{
    class DebugThreads : BaseEnumerable<IDebugThread2, IEnumDebugThreads2>, IEnumDebugThreads2
    {
        internal DebugThreads(IDebugThread2[] data)
            : base(data)
        {

        }

        public int Next(uint celt, IDebugThread2[] rgelt, ref uint celtFetched)
        {
            return Next(celt, rgelt, out celtFetched);
        }
    }
}
