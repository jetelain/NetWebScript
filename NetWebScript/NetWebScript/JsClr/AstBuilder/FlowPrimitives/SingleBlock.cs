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

        public override int LowestStack
        {
            get
            {
                return Block.LowestStack;
            }
        }

        public override System.Collections.Generic.IEnumerable<Sequence> Children
        {
            get { yield break; }
        }
    }
}
