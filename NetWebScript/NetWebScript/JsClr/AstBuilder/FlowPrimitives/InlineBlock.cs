using NetWebScript.JsClr.AstBuilder.FlowGraph;
using System.Diagnostics.Contracts;

namespace NetWebScript.JsClr.AstBuilder.Flow
{
    /// <summary>
    /// Block that must be transformed into a single expression
    /// </summary>
    internal class InlineBlock : SingleBlock
    {
        internal InlineBlock(InstructionBlock block)
            : base(block)
        {
            Contract.Requires(block.Successors.Length > 0, "An inline block must end with a branch instruction, and thus must have one or more sucessors");
        }
    }
}
