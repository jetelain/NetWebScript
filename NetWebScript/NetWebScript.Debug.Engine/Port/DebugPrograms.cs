using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Debugger.Interop;

namespace NetWebScript.Debug.Engine.Port
{
    class DebugPrograms : BaseEnumerable<IDebugProgram2, IEnumDebugPrograms2>, IEnumDebugPrograms2
    {
        internal DebugPrograms(IDebugProgram2[] data)
            : base(data)
        {

        }

        public int Next(uint celt, IDebugProgram2[] rgelt, ref uint celtFetched)
        {
            return Next(celt, rgelt, out celtFetched);
        }
    }
}
