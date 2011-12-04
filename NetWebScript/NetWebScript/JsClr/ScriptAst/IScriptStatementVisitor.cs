using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetWebScript.JsClr.ScriptAst
{
    public interface IScriptStatementVisitor
    {
        void Visit(ScriptArgumentReferenceExpression node);

        void Visit(ScriptArrayCreationExpression arrayCreationExpression);

        void Visit(ScriptArrayIndexerExpression arrayIndexerExpression);

        void Visit(ScriptAssignExpression assignExpression);

        void Visit(ScriptBinaryExpression binaryExpression);

        void Visit(ScriptBreakStatement breakStatement);

        void Visit(ScriptContinueStatement continueStatement);

        void Visit(ScriptFieldReferenceExpression fieldReferenceExpression);

        void Visit(ScriptIfStatement ifStatement);

        void Visit(ScriptLiteralExpression literalExpression);

        void Visit(ScriptMethodInvocationExpression methodInvocationExpression);

        void Visit(ScriptObjectCreationExpression objectCreationExpression);

        void Visit(ScriptReturnStatement returnStatement);

        void Visit(ScriptSwitchStatement switchStatement);

        void Visit(ScriptThisReferenceExpression thisReferenceExpression);

        void Visit(ScriptUnaryExpression unaryExpression);

        void Visit(ScriptVariableReferenceExpression variableReferenceExpression);

        void Visit(ScriptWhileStatement whileStatement);

        void Visit(ScriptConditionExpression conditionExpression);

        void Visit(ScriptTryCatchStatement tryCatchStatement);

        void Visit(ScriptDebugPointExpression point);

        void Visit(ScriptCurrentExceptionExpression currentExceptionExpression);

        void Visit(ScriptThrowStatement throwStatement);

        void Visit(ScriptDoWhileStatement doWhileStatement);
    }

    public interface IScriptStatementVisitor<TResult>
    {
        TResult Visit(ScriptArgumentReferenceExpression node);

        TResult Visit(ScriptArrayCreationExpression arrayCreationExpression);

        TResult Visit(ScriptArrayIndexerExpression arrayIndexerExpression);

        TResult Visit(ScriptAssignExpression assignExpression);

        TResult Visit(ScriptBinaryExpression binaryExpression);

        TResult Visit(ScriptBreakStatement breakStatement);

        TResult Visit(ScriptContinueStatement continueStatement);

        TResult Visit(ScriptFieldReferenceExpression fieldReferenceExpression);

        TResult Visit(ScriptIfStatement ifStatement);

        TResult Visit(ScriptLiteralExpression literalExpression);

        TResult Visit(ScriptMethodInvocationExpression methodInvocationExpression);

        TResult Visit(ScriptObjectCreationExpression objectCreationExpression);

        TResult Visit(ScriptReturnStatement returnStatement);

        TResult Visit(ScriptSwitchStatement switchStatement);

        TResult Visit(ScriptThisReferenceExpression thisReferenceExpression);

        TResult Visit(ScriptUnaryExpression unaryExpression);

        TResult Visit(ScriptVariableReferenceExpression variableReferenceExpression);

        TResult Visit(ScriptWhileStatement whileStatement);

        TResult Visit(ScriptConditionExpression conditionExpression);

        TResult Visit(ScriptTryCatchStatement tryCatchStatement);

        TResult Visit(ScriptDebugPointExpression point);

        TResult Visit(ScriptCurrentExceptionExpression currentExceptionExpression);

        TResult Visit(ScriptThrowStatement throwStatement);

        TResult Visit(ScriptDoWhileStatement doWhileStatement);
    }
}
