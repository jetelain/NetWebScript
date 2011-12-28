using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.JsClr.Ast;

namespace NetWebScript.JsClr.AstPattern.NetAst
{
    class MethodInvocationPattern : NetPatternBase
    {
        public string MethodName { get; set; }

        public Type MethodOwner { get; set; }

        public PatternBase<Statement> Target { get; set; }

        public List<PatternBase<Statement>> Arguments { get; set; }

        public override PatternMatch Visit(Ast.MethodInvocationExpression methodInvocationExpression)
        {
            if (methodInvocationExpression.Method.Name != MethodName)
            {
                return null;
            }
            if (MethodOwner != null && methodInvocationExpression.Method.DeclaringType != MethodOwner)
            {
                return null;
            }
            if (Arguments == null)
            {
                if (methodInvocationExpression.Arguments.Count != 0)
                {
                    return null;
                }
            }
            else if (Arguments.Count != methodInvocationExpression.Arguments.Count)
            {
                return null;
            }
            PatternMatch match = new PatternMatch();
            if (Target == null)
            {
                if (methodInvocationExpression.Target != null)
                {
                    return null;
                }
            }
            else if (!Target.Test(methodInvocationExpression.Target, match))
            {
                return null;
            }
            if (Arguments != null)
            {
                for (int i = 0; i < Arguments.Count; ++i)
                {
                    if (!Arguments[i].Test(methodInvocationExpression.Arguments[i], match))
                    {
                        return null;
                    }
                }
            }
            return match;
        }
    }
}
