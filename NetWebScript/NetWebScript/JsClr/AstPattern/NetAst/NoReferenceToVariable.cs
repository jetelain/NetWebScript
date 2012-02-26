using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.JsClr.Ast;
using NetWebScript.JsClr.AstBuilder.Cil;

namespace NetWebScript.JsClr.AstPattern.NetAst
{
    class NoReferenceToVariable : IStatementVisitor<bool>
    {
        private readonly LocalVariable targetVariable;

        internal NoReferenceToVariable (LocalVariable targetVariable )
        {
            this.targetVariable = targetVariable;
        }

        public bool Visit(ArgumentReferenceExpression node)
        {
            return true;
        }

        public bool Visit(ArrayCreationExpression arrayCreationExpression)
        {
            if (arrayCreationExpression.Initialize != null)
            {
                if (arrayCreationExpression.Initialize.Any(i => !i.Accept(this)))
                {
                    return false;
                }
            }
            return arrayCreationExpression.Size.Accept(this);
        }

        public bool Visit(ArrayIndexerExpression arrayIndexerExpression)
        {
            return arrayIndexerExpression.Target.Accept(this) && arrayIndexerExpression.Index.Accept(this);
        }

        public bool Visit(AssignExpression assignExpression)
        {
            return assignExpression.Target.Accept(this) && assignExpression.Value.Accept(this);
        }

        public bool Visit(BinaryExpression binaryExpression)
        {
            return binaryExpression.Left.Accept(this) && binaryExpression.Right.Accept(this);
        }

        public bool Visit(BreakStatement breakStatement)
        {
            throw new InvalidOperationException();
        }

        public bool Visit(CastExpression castExpression)
        {
            return castExpression.Value.Accept(this);
        }

        public bool Visit(ContinueStatement continueStatement)
        {
            throw new InvalidOperationException();
        }

        public bool Visit(FieldReferenceExpression fieldReferenceExpression)
        {
            if (fieldReferenceExpression.Target != null)
            {
                return fieldReferenceExpression.Target.Accept(this);
            }
            return true;
        }

        public bool Visit(IfStatement ifStatement)
        {
            throw new InvalidOperationException();
        }

        public bool Visit(LiteralExpression literalExpression)
        {
            return true;
        }

        public bool Visit(MethodInvocationExpression methodInvocationExpression)
        {
            if (methodInvocationExpression.Target != null)
            {
                if (!methodInvocationExpression.Target.Accept(this))
                {
                    return false;
                }
            }
            return methodInvocationExpression.Arguments.All(i => i.Accept(this));
        }

        public bool Visit(ObjectCreationExpression objectCreationExpression)
        {
            return objectCreationExpression.Arguments.All(i => i.Accept(this));
        }

        public bool Visit(ReturnStatement returnStatement)
        {
            throw new InvalidOperationException();
        }

        public bool Visit(SwitchStatement switchStatement)
        {
            throw new InvalidOperationException();
        }

        public bool Visit(ThisReferenceExpression thisReferenceExpression)
        {
            return true;
        }

        public bool Visit(UnaryExpression unaryExpression)
        {
            return unaryExpression.Operand.Accept(this);
        }

        public bool Visit(VariableReferenceExpression variableReferenceExpression)
        {
            return variableReferenceExpression.Variable != targetVariable;
        }

        public bool Visit(WhileStatement whileStatement)
        {
            throw new InvalidOperationException();
        }

        public bool Visit(ConditionExpression conditionExpression)
        {
            return conditionExpression.Condition.Accept(this) 
                && conditionExpression.Then.Accept(this) 
                && conditionExpression.Else.Accept(this);
        }

        public bool Visit(TryCatchStatement tryCatchStatement)
        {
            throw new InvalidOperationException();
        }

        public bool Visit(BoxExpression boxExpression)
        {
            return boxExpression.Value.Accept(this);
        }

        public bool Visit(DebugPointExpression point)
        {
            if (point.Value != null)
            {
                return point.Value.Accept(this);
            }
            return true;
        }

        public bool Visit(CurrentExceptionExpression currentExceptionExpression)
        {
            return true;
        }

        public bool Visit(ThrowStatement throwStatement)
        {
            throw new InvalidOperationException();
        }

        public bool Visit(DoWhileStatement doWhileStatement)
        {
            throw new InvalidOperationException();
        }

        public bool Visit(SafeCastExpression safeCastExpression)
        {
            return safeCastExpression.Value.Accept(this);
        }

        public bool Visit(UnboxExpression unboxExpression)
        {
            return unboxExpression.Value.Accept(this);
        }

        public bool Visit(MakeByRefFieldExpression refExpression)
        {
            if (refExpression.Target != null)
            {
                return refExpression.Target.Accept(this);
            }
            return true;
        }

        public bool Visit(ByRefSetExpression byRefSetExpression)
        {
            return byRefSetExpression.Target.Accept(this) && byRefSetExpression.Value.Accept(this);
        }

        public bool Visit(MakeByRefVariableExpression makeByRefVariableExpression)
        {
            return makeByRefVariableExpression.Variable != targetVariable;
        }

        public bool Visit(ByRefGetExpression byRefGetExpression)
        {
            return byRefGetExpression.Target.Accept(this);
        }

        public bool Visit(MakeByRefArgumentExpression makeByRefArgumentExpression)
        {
            return true;
        }

        public bool Visit(DefaultValueExpression defaultValueExpression)
        {
            return true;
        }

        public bool Visit(NumberConvertionExpression numberConvertion)
        {
            return numberConvertion.Value.Accept(this);
        }
    }
}
