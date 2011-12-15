using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetWebScript.JsClr.AstBuilder.FlowGraph
{
    internal class CatchHandlerData
    {
        public readonly Type Type;
        public readonly InstructionBlock Block;
        public readonly bool IsFault;

        public CatchHandlerData(Type type, InstructionBlock block, bool isFault)
        {
            Type = type;
            Block = block;
            IsFault = isFault;
        }
    }
}
