using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetWebScript.JsClr.AstPattern
{
    class ExactSequencePattern<TStatement> : SequencePattern<TStatement>, IEnumerable<PatternBase<TStatement>> where TStatement : class
    {
        internal List<PatternBase<TStatement>> Patterns { get; set; }

        public void Add (PatternBase<TStatement> statement)
        {
            if (Patterns == null) Patterns = new List<PatternBase<TStatement>>();
            Patterns.Add(statement);
        }

        public IEnumerator<PatternBase<TStatement>> GetEnumerator()
        {
            return Patterns.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return Patterns.GetEnumerator();
        }

        internal override bool Visit(Navigator<TStatement> statements, PatternContext context)
        {
            var patternsEnum = Patterns.GetEnumerator();
            while (patternsEnum.MoveNext() && statements.MoveNext())
            {
                if (!patternsEnum.Current.Test(statements.Current, context))
                {
                    return false;
                }
            }
            if (patternsEnum.MoveNext())
            {
                return false;
            }
            return true;
        }
    }
}
