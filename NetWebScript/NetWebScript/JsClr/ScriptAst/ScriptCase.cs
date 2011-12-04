using System.Collections.Generic;

namespace NetWebScript.JsClr.ScriptAst
{
    public sealed class ScriptCase
    {
        public ScriptCase ( object value, List<ScriptStatement> statements )
        {
            Value = value;
            Statements = statements;
        }

        public static readonly object DefaultCase = NetWebScript.JsClr.Ast.Case.DefaultCase;

        public object Value { get; internal set; }

        public List<ScriptStatement> Statements { get; internal set; }
    }
}
