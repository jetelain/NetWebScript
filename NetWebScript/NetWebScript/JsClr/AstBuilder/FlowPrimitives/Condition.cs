using System.Collections.Generic;
using System.Text;
using NetWebScript.JsClr.AstBuilder.FlowGraph;
using System;
using System.Linq;

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


        public int ContentLowestStack
        {
            get
            {
                var min = int.MaxValue;
                if (Jump != null)
                {
                    min = Math.Min(min, Jump.Min(b => b.LowestStack));
                }
                if (NoJump != null)
                {
                    min = Math.Min(min, NoJump.Min(b => b.LowestStack));
                }
                return min;
            }
        }

        public override int LowestStack
        {
            get
            {
                var min = Block.LowestStack;
                if (min != 0)
                {
                    min = Math.Min(min, ContentLowestStack);
                }
                return min;
            }
        }

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

        public override IEnumerable<Sequence> Children
        {
            get
            {
                if (NoJump != null)
                {
                    foreach (var item in NoJump)
                    {
                        yield return item;
                    }
                }
                if (Jump != null)
                {
                    foreach (var item in Jump)
                    {
                        yield return item;
                    }
                }
            }
        }
    }
}
