using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetWebScript.JsClr.AstPattern
{
    class RepeatSequencePattern<TStatement> : SequencePattern<TStatement> where TStatement : class
    {
        internal PatternBase<TStatement> Pattern { get; set; }
        internal string Name { get; set; }


        internal override bool Visit(Navigator<TStatement> statements, PatternContext context)
        {
            List<TStatement> matching = new List<TStatement>();

            while (statements.MoveNext())
            {
                var current = statements.Current;
                if (Pattern.Test(current, context))
                {
                    matching.Add(current);
                }
                else
                {
                    statements.MoveBack();
                    break;
                }
            }
            context.Merge(new PatternMatch(Name, matching));
            return true;
        }
    }
}
