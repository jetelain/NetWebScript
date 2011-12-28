using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.JsClr.Ast;

namespace NetWebScript.JsClr.AstPattern
{

    class Navigator<T> where T : class
    {
        int pos = -1;
        private readonly IList<T> data;

        internal Navigator(IList<T> data)
        {
            this.data = data;
        }

        internal T Current { get; private set; }

        internal bool MoveNext()
        {
            pos++;
            if (pos < data.Count)
            {
                Current = data[pos];
                return true;
            }
            Current = null;
            return false;
        }

        internal bool MoveBack()
        {
            pos--;
            if (pos >= 0)
            {
                Current = data[pos];
                return true;
            }
            Current = null;
            return false;
        }
    }




    abstract class SequencePattern<TStatement> where TStatement : class
    {
        internal virtual bool Test(IList<TStatement> statements, PatternContext context)
        {
            var enumerator = new Navigator<TStatement>(statements);
            
                if (!Visit(enumerator, context))
                {
                    return false;
                }

                if (enumerator.MoveNext())
                {
                    return false;
                }
            
            return true;
        }

        internal abstract bool Visit(Navigator<TStatement> statements, PatternContext context);
    }
}
