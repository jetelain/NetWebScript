using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetWebScript.JsClr.AstPattern
{
    class SubSequencePattern<TStatement> : SequencePattern<TStatement> where TStatement : class
    {
        internal SequencePattern<TStatement> Pattern { get; set; }

        internal string Name { get; set; }

        internal override bool Visit(Navigator<TStatement> statements, PatternContext context)
        {
            List<PatternContext> matching = new List<PatternContext>();
            while (!statements.IsEnd)
            {
                PatternContext subContext = new PatternContext();
                if (Pattern.Visit(statements, subContext))
                {
                    matching.Add(subContext);
                }
                else
                {
                    statements.MoveNext();
                }
            }
            context.Merge(new PatternMatch(Name, matching));
            return matching.Count > 0;
        }
    }
}
