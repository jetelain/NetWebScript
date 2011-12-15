using System.Collections.Generic;
using System.Text;
using NetWebScript.JsClr.AstBuilder.FlowGraph;

namespace NetWebScript.JsClr.AstBuilder.Flow
{
    public sealed class Switch : SingleBlock
    {
        internal Switch(InstructionBlock block)
            : base(block)
        {

        }

        internal Dictionary<int, List<Sequence>> Cases = new Dictionary<int, List<Sequence>>();

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("Switch");
            builder.AppendLine("\tBlock " + (Block.Index + 1));
            foreach( KeyValuePair<int, List<Sequence>> pair in Cases)
            {
                builder.AppendLine("\t" + (pair.Key == -1 ? "default" : "case "+pair.Key.ToString()));
                builder.AppendLine("\t{");
                foreach (Sequence seq in pair.Value)
                {
                    builder.AppendLine("\t\t" + seq.ToString().Replace("\n", "\n\t\t"));
                }
                builder.AppendLine("\t}");
            }
            builder.Remove(builder.Length - 1, 1);
            return builder.ToString();
        }
    }
}
