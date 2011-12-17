using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.JsClr.Ast;
using System.Reflection;

namespace NetWebScript.JsClr.AstBuilder.AstFilter
{
    class LiteralsTypeFix : AstFilterBase
    {
        private MethodAst currentMethod;

        private void SetValueType(Expression expr, Type type)
        {
            if (type == null)
            {
                return; // FIXME: This case should be avoided far earlier
            }
            LiteralExpression literal = expr as LiteralExpression;
            if (literal != null)
            {
                literal.SetExpressionType(type);
            }
            DebugPointExpression dbg = expr as DebugPointExpression;
            if (dbg != null && dbg.Value != null)
            {
                SetValueType(dbg.Value, type);
            }
        }

        public override Statement Visit(AssignExpression assignExpression)
        {
            if (assignExpression.Value.GetExpressionType() == null)
            {
                SetValueType(assignExpression.Value, assignExpression.Target.GetExpressionType());
            }
            return base.Visit(assignExpression);
        }

        public override Statement Visit(BinaryExpression node)
        {
            if (node.Operator == BinaryOperator.ValueEquality)
            {
                Type typeA = node.Left.GetExpressionType();
                Type typeB = node.Right.GetExpressionType();
                if (typeA == null && typeB != null)
                {
                    SetValueType(node.Left,typeB);
                }
                else if (typeB == null && typeA != null)
                {
                    SetValueType(node.Right,typeA);
                }
            }
            return base.Visit(node);
        }

        public override Statement Visit(MethodInvocationExpression node)
        {
            MethodBase info = node.Method;
            ParameterInfo[] parameters = info.GetParameters();
            for (int i = 0; i < node.Arguments.Count; ++i)
            {
                Expression expr = node.Arguments[i];
                if (expr.GetExpressionType() == null)
                {
                    SetValueType(expr,parameters[i].ParameterType);
                }
            }
            return base.Visit(node);
        }

        public override Statement Visit(ConditionExpression node)
        {
            Type typeA = node.Then.GetExpressionType();
            Type typeB = node.Else.GetExpressionType();
            if (typeA == null && typeB != null)
            {
                SetValueType(node.Then,typeB);
            }
            else if (typeB == null && typeA != null)
            {
                SetValueType(node.Else,typeA);
            }
            return base.Visit(node);
        }

        public override Statement Visit(ReturnStatement returnStatement)
        {
            if (returnStatement.Value != null)
            {
                if (returnStatement.Value.GetExpressionType() == null)
                {
                    SetValueType(returnStatement.Value, ((MethodInfo)currentMethod.Info).ReturnType);
                }
            }
            return base.Visit(returnStatement);
        }

        public override void Visit(MethodAst method)
        {
            this.currentMethod = method;
            base.Visit(method);
        }

        public override Statement Visit(LiteralExpression literalExpression)
        {
            if (literalExpression.GetExpressionType() == null && literalExpression.Value != null)
            {
                literalExpression.SetExpressionType(literalExpression.Value.GetType());
            }
            return base.Visit(literalExpression);
        }
    }
}
