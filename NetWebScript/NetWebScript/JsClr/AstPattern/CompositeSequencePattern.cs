using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetWebScript.JsClr.AstPattern
{
    class CompositeSequencePattern<TStatement> : SequencePattern<TStatement>, IEnumerable<SequencePattern<TStatement>> where TStatement : class
    {
        internal List<SequencePattern<TStatement>> Patterns { get; set; }

        public void Add(SequencePattern<TStatement> statement)
        {
            if (Patterns == null) Patterns = new List<SequencePattern<TStatement>>();
            Patterns.Add(statement);
        }

        public IEnumerator<SequencePattern<TStatement>> GetEnumerator()
        {
            return Patterns.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return Patterns.GetEnumerator();
        }

        internal override bool Visit(Navigator<TStatement> statements, PatternContext context)
        {
            foreach (var pattern in Patterns)
            {
                if (!pattern.Visit(statements, context))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
