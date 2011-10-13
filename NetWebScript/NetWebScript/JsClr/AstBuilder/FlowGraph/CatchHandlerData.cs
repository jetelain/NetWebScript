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

        public CatchHandlerData(Type type, InstructionBlock block)
        {
            Type = type;
            Block = block;
        }
    }
}
