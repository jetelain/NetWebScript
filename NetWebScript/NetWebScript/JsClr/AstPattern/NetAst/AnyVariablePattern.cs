using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetWebScript.JsClr.AstPattern.NetAst
{
    class AnyVariablePattern : NetPatternBase
    {
        internal string Name { get; set; }

        internal Type Type { get; set; }

        public override PatternMatch Visit(Ast.VariableReferenceExpression variableReferenceExpression)
        {
            if (Type == null || variableReferenceExpression.Variable.LocalType == Type)
            {
                return new PatternMatch(Name, variableReferenceExpression.Variable);
            }
            return null;
        }
    }
}
