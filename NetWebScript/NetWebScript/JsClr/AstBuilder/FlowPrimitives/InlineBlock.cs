using NetWebScript.JsClr.AstBuilder.FlowGraph;

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
        }
    }
}
