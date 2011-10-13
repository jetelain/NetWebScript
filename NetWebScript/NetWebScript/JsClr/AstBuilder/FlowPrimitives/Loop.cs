﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.JsClr.AstBuilder.Cil;
using NetWebScript.JsClr.AstBuilder.FlowGraph;

namespace NetWebScript.JsClr.AstBuilder.Flow
{
    public enum LoopBody
    {
        Jump,
        NoJump
    }


    public sealed class Loop : SingleBlock
    {
        internal Loop(InstructionBlock block) : base(block)
        {

        }

        internal List<Sequence> Body;
        internal LoopBody Jump;
        
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("While " + Jump + " ( Block " + (Block.Index + 1)+" )");
            builder.AppendLine("{");
            foreach (Sequence seq in Body)
            {
                builder.AppendLine("\t" + seq.ToString().Replace("\n", "\n\t"));
            }
            builder.Append("}");
            return builder.ToString();
        }
    }
}
