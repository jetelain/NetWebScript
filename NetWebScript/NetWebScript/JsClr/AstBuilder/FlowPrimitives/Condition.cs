using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.JsClr.AstBuilder.Cil;
using NetWebScript.JsClr.AstBuilder.FlowGraph;

namespace NetWebScript.JsClr.AstBuilder.Flow
{
    public sealed class Condition : SingleBlock
    {
        internal Condition(InstructionBlock block)
            : base(block)
        {

        }

        internal List<Sequence> NoJump;
        internal List<Sequence> Jump;

        internal int StackAfter;

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("If ( Block " + (Block.Index + 1)+ " )");
            if (Jump != null)
            {
                builder.AppendLine("Then {");
                foreach (Sequence seq in Jump)
                {
                    builder.AppendLine("\t" + seq.ToString().Replace("\n", "\n\t"));
                }
                builder.AppendLine("}");
            }
            if (NoJump != null)
            {
                builder.AppendLine("Else {");
                foreach (Sequence seq in NoJump)
                {
                    builder.AppendLine("\t" + seq.ToString().Replace("\n", "\n\t"));
                }
                builder.AppendLine("}");
            }
            builder.Remove(builder.Length - 2, 2);
            return builder.ToString();
        }
    }
}
