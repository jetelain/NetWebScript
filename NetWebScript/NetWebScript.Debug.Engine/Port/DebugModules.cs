using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Debugger.Interop;

namespace NetWebScript.Debug.Engine.Port
{
    class DebugModules : BaseEnumerable<IDebugModule2, IEnumDebugModules2>, IEnumDebugModules2
    {
        internal DebugModules(IDebugModule2[] data)
            : base(data)
        {

        }

        public int Next(uint celt, IDebugModule2[] rgelt, ref uint celtFetched)
        {
            return Next(celt, rgelt, out celtFetched);
        }
    }
}
