using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.JsClr.AstBuilder.Cil;
using NetWebScript.JsClr.AstBuilder.FlowGraph;

namespace NetWebScript.JsClr.AstBuilder.Flow
{
    public class SingleBlock : Sequence
    {
        internal SingleBlock(InstructionBlock block)
        {
            this.Block = block;

        }

        internal readonly InstructionBlock Block;

        public override string ToString()
        {
            return "Block "+(Block.Index+1);
        }
    }
}
