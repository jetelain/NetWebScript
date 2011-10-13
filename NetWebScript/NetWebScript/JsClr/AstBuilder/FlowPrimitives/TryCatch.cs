using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetWebScript.JsClr.AstBuilder.Flow
{
    class CatchFlow
    {
        public Type Type;
        public List<Sequence> Body;
    }

    class TryCatch : Sequence
    {
        public List<Sequence> Body;
        public List<CatchFlow> CatchList;
        public List<Sequence> Finally;

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("Try");
            builder.AppendLine("{");
            foreach (Sequence seq in Body)
            {
                builder.AppendLine("\t" + seq.ToString().Replace("\n", "\n\t"));
            }
            builder.AppendLine("}");
            if (CatchList != null)
            {
                foreach (CatchFlow catchflow in CatchList)
                {
                    builder.AppendLine("Catch (" + catchflow.Type + ") {");
                    foreach (Sequence seq in catchflow.Body)
                    {
                        builder.AppendLine("\t" + seq.ToString().Replace("\n", "\n\t"));
                    }
                    builder.AppendLine("}");
                }
            }
            if (Finally != null)
            {
                builder.AppendLine("Finally {");
                foreach (Sequence seq in Finally)
                {
                    builder.AppendLine("\t" + seq.ToString().Replace("\n", "\n\t"));
                }
                builder.AppendLine("}");
            }
            builder.Remove(builder.Length - 1, 1);
            return builder.ToString();
        }

    }
}
