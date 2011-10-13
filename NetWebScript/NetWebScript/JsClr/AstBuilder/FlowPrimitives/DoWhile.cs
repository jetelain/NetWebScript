using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetWebScript.JsClr.AstBuilder.Flow
{
    public class DoWhile : Sequence
    {
        internal List<Sequence> Body;
        internal LoopBody Jump;

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("Do");
            builder.AppendLine("{");
            foreach (Sequence seq in Body)
            {
                builder.AppendLine("\t" + seq.ToString().Replace("\n", "\n\t"));
            }
            builder.Append("} While "+Jump);
            return builder.ToString();
        }
    }
}
