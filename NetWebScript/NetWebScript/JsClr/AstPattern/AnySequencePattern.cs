using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetWebScript.JsClr.AstPattern
{
    class AnySequencePattern<TStatement> : SequencePattern<TStatement> where TStatement : class
    {
        internal string Name { get; set; }
  
        internal override bool Test(IList<TStatement> statements, PatternContext context)
        {
            if ( Name != null )
            {
                context.Merge(new PatternMatch(Name, statements));
            }
            return true;
        }

        internal override bool Visit(Navigator<TStatement> statements, PatternContext context)
        {
            if (Name == null)
            {
                while (statements.MoveNext());
            }
            else
            {
                List<TStatement> matching = new List<TStatement>();
                while (statements.MoveNext())
                {
                    matching.Add(statements.Current);
                }
                context.Merge(new PatternMatch(Name, matching));
            }
            return true;
        }
    }
}
