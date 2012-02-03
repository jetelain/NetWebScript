using System.Collections.Generic;

namespace NetWebScript.JsClr.ScriptAst
{
    public sealed class ScriptCase
    {
        public ScriptCase(ScriptLiteralExpression value, List<ScriptStatement> statements)
        {
            Value = value;
            Statements = statements;
        }

        public static readonly ScriptLiteralExpression DefaultCase = new ScriptLiteralExpression(null,null,null);

        public ScriptLiteralExpression Value { get; internal set; }

        public List<ScriptStatement> Statements { get; internal set; }
    }
}
