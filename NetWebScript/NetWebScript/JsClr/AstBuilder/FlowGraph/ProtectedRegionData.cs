using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetWebScript.JsClr.AstBuilder.FlowGraph
{
    internal class ProtectedRegionData
    {
        public readonly int TryOffset;
        public readonly int TryLength;

        public readonly InstructionBlock Try;
        public readonly HashSet<InstructionBlock> TryEnd = new HashSet<InstructionBlock>();

        public readonly List<CatchHandlerData> Catches = new List<CatchHandlerData>();

        public InstructionBlock Finally;

        public InstructionBlock Fault;

        public ProtectedRegionData(int TryOffset, int TryLength, InstructionBlock Try)
        {
            this.TryOffset = TryOffset;
            this.TryLength = TryLength;
            this.Try = Try;
        }

        public int TryEndOffset
        {
            get { return TryOffset + TryLength; }
        }
    }
}
