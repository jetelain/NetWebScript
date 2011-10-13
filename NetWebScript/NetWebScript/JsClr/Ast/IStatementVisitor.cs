using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetWebScript.JsClr.Ast
{
    public interface IStatementVisitor
    {
        void Visit(ArgumentReferenceExpression node);

        void Visit(ArrayCreationExpression arrayCreationExpression);

        void Visit(ArrayIndexerExpression arrayIndexerExpression);

        void Visit(AssignExpression assignExpression);

        void Visit(BinaryExpression binaryExpression);

        void Visit(BreakStatement breakStatement);

        void Visit(CastExpression castExpression);

        void Visit(ContinueStatement continueStatement);

        void Visit(FieldReferenceExpression fieldReferenceExpression);

        void Visit(IfStatement ifStatement);

        void Visit(LiteralExpression literalExpression);

        void Visit(MethodInvocationExpression methodInvocationExpression);

        void Visit(ObjectCreationExpression objectCreationExpression);

        void Visit(ReturnStatement returnStatement);

        void Visit(SwitchStatement switchStatement);

        void Visit(ThisReferenceExpression thisReferenceExpression);

        void Visit(UnaryExpression unaryExpression);

        void Visit(VariableReferenceExpression variableReferenceExpression);

        void Visit(WhileStatement whileStatement);

        void Visit(ConditionExpression conditionExpression);

        void Visit(TryCatchStatement tryCatchStatement);

        //void Visit(ForeachStatement foreachStatement);

        //void Visit(SetPropertyExpression node);

        //void Visit(GetPropertyExpression node);

        void Visit(BoxExpression boxExpression);

        void Visit(DebugPointExpression point);

        void Visit(CurrentExceptionExpression currentExceptionExpression);

        void Visit(ThrowStatement throwStatement);

        void Visit(DoWhileStatement doWhileStatement);

        void Visit(SafeCastExpression safeCastExpression);

        void Visit(UnboxExpression unboxExpression);
    }

    public interface IStatementVisitor<TResult>
    {
        TResult Visit(ArgumentReferenceExpression node);

        TResult Visit(ArrayCreationExpression arrayCreationExpression);

        TResult Visit(ArrayIndexerExpression arrayIndexerExpression);

        TResult Visit(AssignExpression assignExpression);

        TResult Visit(BinaryExpression binaryExpression);

        TResult Visit(BreakStatement breakStatement);

        TResult Visit(CastExpression castExpression);

        TResult Visit(ContinueStatement continueStatement);

        TResult Visit(FieldReferenceExpression fieldReferenceExpression);

        TResult Visit(IfStatement ifStatement);

        TResult Visit(LiteralExpression literalExpression);

        TResult Visit(MethodInvocationExpression methodInvocationExpression);

        TResult Visit(ObjectCreationExpression objectCreationExpression);

        TResult Visit(ReturnStatement returnStatement);

        TResult Visit(SwitchStatement switchStatement);

        TResult Visit(ThisReferenceExpression thisReferenceExpression);

        TResult Visit(UnaryExpression unaryExpression);

        TResult Visit(VariableReferenceExpression variableReferenceExpression);

        TResult Visit(WhileStatement whileStatement);

        TResult Visit(ConditionExpression conditionExpression);

        TResult Visit(TryCatchStatement tryCatchStatement);

        //TResult Visit(ForeachStatement foreachStatement);

        //TResult Visit(SetPropertyExpression node);

        //TResult Visit(GetPropertyExpression node);

        TResult Visit(BoxExpression boxExpression);

        TResult Visit(DebugPointExpression point);

        TResult Visit(CurrentExceptionExpression currentExceptionExpression);

        TResult Visit(ThrowStatement throwStatement);

        TResult Visit(DoWhileStatement doWhileStatement);

        TResult Visit(SafeCastExpression safeCastExpression);

        TResult Visit(UnboxExpression unboxExpression);
    }
}
