using System;
using NetWebScript.JsClr.Ast;

namespace NetWebScript.JsClr.AstPattern.NetAst
{
    class AnyVariableOrAssignPattern : NetPatternBase
    {
        internal string Name { get; set; }

        internal string ValueName { get; set; }


        internal Type Type { get; set; }

        public override PatternMatch Visit(Ast.VariableReferenceExpression variableReferenceExpression)
        {
            if (Type == null || variableReferenceExpression.Variable.LocalType == Type)
            {
                return new PatternMatch(Name, variableReferenceExpression.Variable, ValueName, variableReferenceExpression);
            }
            return null;
        }

        public override PatternMatch Visit(Ast.AssignExpression assignExpression)
        {
            var target = assignExpression.Target as VariableReferenceExpression;
            if (target != null)
            {
                if (Type == null || target.Variable.LocalType == Type)
                {
                    return new PatternMatch(Name, target.Variable, ValueName, assignExpression);
                }
            }
            return null;
        }

        public override PatternMatch Visit(DebugPointExpression point)
        {
            if (point.Value != null)
            {
                var match = point.Value.Accept(this);
                if (match != null)
                {
                    return new PatternMatch(
                        Name, match.GetCapture(Name),
                        ValueName, new DebugPointExpression(point.Point, (Expression)match.GetCapture(ValueName)));
                }
            }
            return null;
        }
    }
}
