using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Debugger.Interop;

namespace NetWebScript.Debug.Engine.Debug
{
    class CodeContextInfos : BaseEnumerable<IDebugCodeContext2, IEnumDebugCodeContexts2>, IEnumDebugCodeContexts2
    {
        public CodeContextInfos(IDebugCodeContext2[] data)
            : base(data)
        {
        }

        public int Next(uint celt, IDebugCodeContext2[] rgelt, ref uint celtFetched)
        {
            return Next(celt, rgelt, out celtFetched);
        }
   
    }
}
