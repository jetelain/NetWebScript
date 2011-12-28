using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.JsClr.Ast;

namespace NetWebScript.JsClr.AstPattern.NetAst
{
    abstract class NetPatternBase : PatternBase<Statement>, IStatementVisitor<PatternMatch>
    {
        protected override PatternMatch Visit(Statement statement)
        {
            return statement.Accept(this);
        }

        public virtual PatternMatch Visit(ArgumentReferenceExpression node)
        {
            return null;
        }

        public virtual PatternMatch Visit(ArrayCreationExpression arrayCreationExpression)
        {
            return null;
        }

        public virtual PatternMatch Visit(ArrayIndexerExpression arrayIndexerExpression)
        {
            return null;
        }

        public virtual PatternMatch Visit(AssignExpression assignExpression)
        {
            return null;
        }

        public virtual PatternMatch Visit(BinaryExpression binaryExpression)
        {
            return null;
        }

        public virtual PatternMatch Visit(BreakStatement breakStatement)
        {
            return null;
        }

        public virtual PatternMatch Visit(CastExpression castExpression)
        {
            return null;
        }

        public virtual PatternMatch Visit(ContinueStatement continueStatement)
        {
            return null;
        }

        public virtual PatternMatch Visit(FieldReferenceExpression fieldReferenceExpression)
        {
            return null;
        }

        public virtual PatternMatch Visit(IfStatement ifStatement)
        {
            return null;
        }

        public virtual PatternMatch Visit(LiteralExpression literalExpression)
        {
            return null;
        }

        public virtual PatternMatch Visit(MethodInvocationExpression methodInvocationExpression)
        {
            return null;
        }

        public virtual PatternMatch Visit(ObjectCreationExpression objectCreationExpression)
        {
            return null;
        }

        public virtual PatternMatch Visit(ReturnStatement returnStatement)
        {
            return null;
        }

        public virtual PatternMatch Visit(SwitchStatement switchStatement)
        {
            return null;
        }

        public virtual PatternMatch Visit(ThisReferenceExpression thisReferenceExpression)
        {
            return null;
        }

        public virtual PatternMatch Visit(UnaryExpression unaryExpression)
        {
            return null;
        }

        public virtual PatternMatch Visit(VariableReferenceExpression variableReferenceExpression)
        {
            return null;
        }

        public virtual PatternMatch Visit(WhileStatement whileStatement)
        {
            return null;
        }

        public virtual PatternMatch Visit(ConditionExpression conditionExpression)
        {
            return null;
        }

        public virtual PatternMatch Visit(TryCatchStatement tryCatchStatement)
        {
            return null;
        }

        public virtual PatternMatch Visit(BoxExpression boxExpression)
        {
            return null;
        }

        public virtual PatternMatch Visit(DebugPointExpression point)
        {
            return null;
        }

        public virtual PatternMatch Visit(CurrentExceptionExpression currentExceptionExpression)
        {
            return null;
        }

        public virtual PatternMatch Visit(ThrowStatement throwStatement)
        {
            return null;
        }

        public virtual PatternMatch Visit(DoWhileStatement doWhileStatement)
        {
            return null;
        }

        public virtual PatternMatch Visit(SafeCastExpression safeCastExpression)
        {
            return null;
        }

        public virtual PatternMatch Visit(UnboxExpression unboxExpression)
        {
            return null;
        }

        public virtual PatternMatch Visit(MakeByRefFieldExpression refExpression)
        {
            return null;
        }

        public virtual PatternMatch Visit(ByRefSetExpression byRefSetExpression)
        {
            return null;
        }

        public virtual PatternMatch Visit(MakeByRefVariableExpression makeByRefVariableExpression)
        {
            return null;
        }

        public virtual PatternMatch Visit(ByRefGetExpression byRefGetExpression)
        {
            return null;
        }

        public virtual PatternMatch Visit(MakeByRefArgumentExpression makeByRefArgumentExpression)
        {
            return null;
        }

        public virtual PatternMatch Visit(DefaultValueExpression defaultValueExpression)
        {
            return null;
        }

        public virtual PatternMatch Visit(NumberConvertionExpression numberConvertion)
        {
            return null;
        }

    }
}
