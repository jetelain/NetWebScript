using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.JsClr.Ast;

namespace NetWebScript.JsClr.AstPattern
{
    class PatternContext
    {
        internal Dictionary<string, object> backReferences = new Dictionary<string, object>();

        internal object GetCapture(string name)
        {
            return backReferences[name];
        }
        internal object TryGetCapture(string name, out object value)
        {
            return backReferences.TryGetValue(name,out value);
        }

        internal bool Merge ( PatternContext match )
        {
            foreach (var pair in match.backReferences)
            {
                object previousValue;
                if (backReferences.TryGetValue(pair.Key, out previousValue))
                {
                    if (!object.Equals(previousValue, pair.Value))
                    {
                        return false;
                    }
                }
                else
                {
                    backReferences.Add(pair.Key, pair.Value);
                }
            }
            return true;
        }
    }

    internal class PatternMatch : PatternContext
    {
        internal PatternMatch()
        {

        }

        internal PatternMatch(string name, object value)
        {
            backReferences.Add(name, value);
        }

        internal PatternMatch(string name, object value, string name2, object value2)
        {
            backReferences.Add(name, value);
            backReferences.Add(name2, value2);
        }
    }

    abstract class PatternBase<TStatement> where TStatement : class
    {
        internal string StatementName { get; set; }

        internal virtual bool Test(TStatement statement, PatternContext context)
        {
            if (statement == null)
            {
                return false;
            }
            var result = Visit(statement);
            if (result != null)
            {
                if (context.Merge(result))
                {
                    if (StatementName != null)
                    {
                        context.backReferences.Add(StatementName, statement);
                    }
                    return true;
                }
            }
            return false;
        }

        protected abstract PatternMatch Visit(TStatement statement);

    }
}
