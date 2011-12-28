using System;

namespace NetWebScript.JsClr.AstPattern.NetAst
{
    class AnyMakeByRefVariablePattern : NetPatternBase
    {
        internal string Name { get; set; }
        internal Type Type { get; set; }

        public override PatternMatch Visit(Ast.MakeByRefVariableExpression variableReferenceExpression)
        {
            if (Type == null || variableReferenceExpression.Variable.LocalType == Type)
            {
                return new PatternMatch(Name, variableReferenceExpression.Variable);
            }
            return null;
        }
    }
}
