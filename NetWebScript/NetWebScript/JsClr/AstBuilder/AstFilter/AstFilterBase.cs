using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.JsClr.Ast;

namespace NetWebScript.JsClr.AstBuilder.AstFilter
{
    class AstFilterBase : IStatementVisitor<Statement>, IAstFilter
    {

        public virtual Statement Visit(ArgumentReferenceExpression node)
        {
            return node;
        }

        public virtual Statement Visit(ArrayCreationExpression arrayCreationExpression)
        {
            arrayCreationExpression.Size = (Expression)arrayCreationExpression.Size.Accept(this);
            if (arrayCreationExpression.Initialize != null)
            {
                arrayCreationExpression.Initialize = Visit(arrayCreationExpression.Initialize);
            }
            return arrayCreationExpression;
        }

        public virtual Statement Visit(ArrayIndexerExpression arrayIndexerExpression)
        {
            arrayIndexerExpression.Target = (Expression)arrayIndexerExpression.Target.Accept(this);
            arrayIndexerExpression.Index = (Expression)arrayIndexerExpression.Index.Accept(this);
            return arrayIndexerExpression;
        }

        public virtual Statement Visit(AssignExpression assignExpression)
        {
            assignExpression.Target = (AssignableExpression)assignExpression.Target.Accept(this);
            assignExpression.Value = (Expression)assignExpression.Value.Accept(this);
            return assignExpression;
        }

        public virtual Statement Visit(BinaryExpression binaryExpression)
        {
            binaryExpression.Left = (Expression)binaryExpression.Left.Accept(this);
            binaryExpression.Right = (Expression)binaryExpression.Right.Accept(this);
            return binaryExpression;
        }

        public virtual Statement Visit(BreakStatement breakStatement)
        {
            return breakStatement;
        }

        public virtual Statement Visit(CastExpression castExpression)
        {
            castExpression.Value = (Expression)castExpression.Value.Accept(this);
            return castExpression;
        }

        public virtual Statement Visit(SafeCastExpression safeCastExpression)
        {
            safeCastExpression.Value = (Expression)safeCastExpression.Value.Accept(this);
            return safeCastExpression;
        }

        public virtual Statement Visit(ContinueStatement continueStatement)
        {
            return continueStatement;
        }

        public virtual Statement Visit(FieldReferenceExpression fieldReferenceExpression)
        {
            if (fieldReferenceExpression.Target != null)
            {
                fieldReferenceExpression.Target = (Expression)fieldReferenceExpression.Target.Accept(this);
            }
            return fieldReferenceExpression;
        }

        public virtual Statement Visit(IfStatement ifStatement)
        {
            ifStatement.Condition = (Expression)ifStatement.Condition.Accept(this);
            ifStatement.Then = Visit(ifStatement.Then);
            if (ifStatement.Else != null)
            {
                ifStatement.Else = Visit(ifStatement.Else);
            }
            return ifStatement;
        }

        public virtual Statement Visit(LiteralExpression literalExpression)
        {
            return literalExpression;
        }

        public virtual Statement Visit(MethodInvocationExpression methodInvocationExpression)
        {
            if (methodInvocationExpression.Target != null)
            {
                methodInvocationExpression.Target = (Expression)methodInvocationExpression.Target.Accept(this);
            }
            methodInvocationExpression.Arguments = Visit(methodInvocationExpression.Arguments);
            return methodInvocationExpression;
        }

        //public virtual Statement Visit(SetPropertyExpression node)
        //{
        //    if (node.Target != null)
        //    {
        //        node.Target = (Expression)node.Target.Accept(this);
        //    }
        //    node.Arguments = Visit(node.Arguments);
        //    return node;
        //}

        //public virtual Statement Visit(GetPropertyExpression node)
        //{
        //    if (node.Target != null)
        //    {
        //        node.Target = (Expression)node.Target.Accept(this);
        //    }
        //    node.Arguments = Visit(node.Arguments);
        //    return node;
        //}

        public virtual Statement Visit(ObjectCreationExpression objectCreationExpression)
        {
            objectCreationExpression.Arguments = Visit(objectCreationExpression.Arguments);
            return objectCreationExpression;
        }

        public virtual Statement Visit(ReturnStatement returnStatement)
        {
            if (returnStatement.Value != null)
            {
                returnStatement.Value = (Expression)returnStatement.Value.Accept(this);
            }
            return returnStatement;
        }

        public virtual Statement Visit(SwitchStatement switchStatement)
        {
            if (switchStatement.Value != null)
            {
                switchStatement.Value = (Expression)switchStatement.Value.Accept(this);
            }
            foreach (Case @case in switchStatement.Cases)
            {
                @case.Statements = Visit(@case.Statements);
            }
            return switchStatement;
        }

        public virtual Statement Visit(ThisReferenceExpression thisReferenceExpression)
        {
            return thisReferenceExpression;
        }

        public virtual Statement Visit(UnaryExpression unaryExpression)
        {
            unaryExpression.Operand = (Expression)unaryExpression.Operand.Accept(this);
            return unaryExpression;
        }

        public virtual Statement Visit(VariableReferenceExpression variableReferenceExpression)
        {
            return variableReferenceExpression;
        }

        public virtual Statement Visit(WhileStatement whileStatement)
        {
            whileStatement.Condition = (Expression)whileStatement.Condition.Accept(this);
            whileStatement.Body = Visit(whileStatement.Body);
            return whileStatement;
        }

        public virtual Statement Visit(DoWhileStatement whileStatement)
        {
            whileStatement.Condition = (Expression)whileStatement.Condition.Accept(this);
            whileStatement.Body = Visit(whileStatement.Body);
            return whileStatement;
        }

        public virtual Statement Visit(ConditionExpression conditionExpression)
        {
            conditionExpression.Condition = (Expression)conditionExpression.Condition.Accept(this);
            conditionExpression.Then = (Expression)conditionExpression.Then.Accept(this);
            conditionExpression.Else = (Expression)conditionExpression.Else.Accept(this);

            return conditionExpression;
        }

        public virtual Statement Visit(TryCatchStatement tryCatchStatement)
        {
            tryCatchStatement.Body = Visit(tryCatchStatement.Body);
            if (tryCatchStatement.CatchList != null)
            {
                foreach (Catch @catch in tryCatchStatement.CatchList)
                {
                    @catch.Body = Visit(@catch.Body);
                }
            }
            if (tryCatchStatement.Finally != null)
            {
                tryCatchStatement.Finally = Visit(tryCatchStatement.Finally);
            }
            return tryCatchStatement;
        }

        //public virtual Statement Visit(ForeachStatement foreachStatement)
        //{
        //    foreachStatement.Body = Visit(foreachStatement.Body);
        //    foreachStatement.Source = (Expression)foreachStatement.Source.Accept(this);
        //    return foreachStatement;
        //}

        public virtual Statement Visit(BoxExpression boxExpression)
        {
            boxExpression.Value = (Expression)boxExpression.Value.Accept(this);
            return boxExpression;
        }

        public virtual List<Statement> Visit(List<Statement> list)
        {
            for (int i = 0; i < list.Count; ++i)
            {
                list[i] = list[i].Accept(this);
            }
            return list;
        }

        public virtual List<Expression> Visit(List<Expression> list)
        {
            for (int i = 0; i < list.Count; ++i)
            {
                list[i] = (Expression)list[i].Accept(this);
            }
            return list;
        }

        public virtual void Visit(MethodAst method)
        {
            Visit(method.Statements);
        }

        #region IStatementVisitor<Statement> Members


        public virtual Statement Visit(DebugPointExpression point)
        {
            if (point.Value != null)
            {
                point.Value = (Expression)point.Value.Accept(this); 
            }
            return point;
        }

        #endregion

        public virtual Statement Visit(CurrentExceptionExpression currentExceptionExpression)
        {
            return currentExceptionExpression;
        }

        #region IStatementVisitor<Statement> Members


        public virtual Statement Visit(ThrowStatement throwStatement)
        {
            throwStatement.Value = (Expression)throwStatement.Value.Accept(this);
            return throwStatement;
        }

        #endregion

        #region IStatementVisitor<Statement> Members

        public virtual Statement Visit(UnboxExpression unboxExpression)
        {
            unboxExpression.Value = (Expression)unboxExpression.Value.Accept(this);
            return unboxExpression;
        }

        #endregion

        public virtual Statement Visit(MakeByRefFieldExpression refExpression)
        {
            if (refExpression.Target != null)
            {
                refExpression.Target = (Expression)refExpression.Target.Accept(this);
            }
            return refExpression;
        }

        public virtual Statement Visit(ByRefSetExpression byRefSetExpression)
        {
            byRefSetExpression.Target = (Expression)byRefSetExpression.Target.Accept(this);
            byRefSetExpression.Value = (Expression)byRefSetExpression.Value.Accept(this);
            return byRefSetExpression;
        }

        public virtual Statement Visit(MakeByRefVariableExpression makeByRefVariableExpression)
        {
            return makeByRefVariableExpression;
        }

        public virtual Statement Visit(ByRefGetExpression byRefGetExpression)
        {
            byRefGetExpression.Target = (Expression)byRefGetExpression.Target.Accept(this);
            return byRefGetExpression;
        }

        public Statement Visit(MakeByRefArgumentExpression makeByRefArgumentExpression)
        {
            throw new AstBuilderException(makeByRefArgumentExpression.IlOffset, "Unsupported operation");
        }
    }
}
