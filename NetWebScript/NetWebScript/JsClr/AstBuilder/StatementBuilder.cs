using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.JsClr.Ast;
using NetWebScript.JsClr.AstBuilder.Cil;
using NetWebScript.JsClr.AstBuilder.Flow;
using System.Diagnostics.Contracts;

namespace NetWebScript.JsClr.AstBuilder
{
    /// <summary>
    /// Class to build a <see cref="Statement"/> list from an <see cref="Sequence"/> list.
    /// </summary>
    internal sealed class StatementBuilder : IExpressionBuilderClient
    {
        private readonly List<Statement> statements = new List<Statement>();

        //private readonly MethodCil body;
        //private readonly MethodAst built;

        private readonly IEnumerator<Sequence> enumerator;

        private SingleBlock current;

        private readonly ExpressionBuilder builder;
        //private readonly Catch @catch;
        private readonly DoWhileStatement doWhileStatement;

        private StatementBuilder(ExpressionBuilder builder, List<Sequence> sequences, Catch @catch, DoWhileStatement doWhileStatement)
        {
            this.doWhileStatement = doWhileStatement;
            this.builder = builder;

            builder.PushClient(this);

            builder.Reset(); // The reset operation should be useless TODO: remove if really useless
            // The problem may resides in "registers".

            if (@catch != null)
            {
                builder.Push(new CurrentExceptionExpression(@catch.Type));
            }

            enumerator = sequences.GetEnumerator();

            while (enumerator.MoveNext())
            {
                Process(enumerator.Current);
            }

            builder.PopClient(this);
        }

        private StatementBuilder(MethodAst built, MethodCil body, List<Sequence> sequences)
        {
            builder = new ExpressionBuilder(built, body, this);

            enumerator = sequences.GetEnumerator();

            while(enumerator.MoveNext())
            {
                Process(enumerator.Current);
            }

            if (builder.StackHeight != 0)
            {
                throw new AstBuilderException(current.Block.Last.Offset, "Stack should be empty here");
            }
        }

        private void Process(Sequence sequence)
        {
            current = sequence as SingleBlock;
            if (current != null)
            {
                if (current is InlineBlock)
                {
                    builder.SetEnforceInline();
                    foreach (Instruction instruction in current.Block)
                    {
                        builder.Visit(instruction);
                    }
                    builder.UnsetEnforceInline(); // This should be useless, as an inline block should ends with a branch instruction
                }
                else
                {
                    foreach (Instruction instruction in current.Block)
                    {
                        builder.Visit(instruction);
                    }
                }
            }
            else if (sequence is Break)
            {
                statements.Add(new BreakStatement());
            }
            else if (sequence is Continue)
            {
                statements.Add(new ContinueStatement());
            }
            else if (sequence is PostLoop)
            {
                statements.Add(TransformDoWhile((PostLoop)sequence));
            }
            else
            {
                TryCatch trycath = sequence as TryCatch;
                if (trycath != null)
                {
                    TryCatchStatement statement = new TryCatchStatement();
                    statement.Body = Transform(trycath.Body);
                    if (trycath.CatchList != null)
                    {
                        statement.CatchList = new List<Catch>(trycath.CatchList.Count);
                        foreach (CatchFlow catchflow in trycath.CatchList)
                        {
                            Catch catchstatement = new Catch();
                            catchstatement.Type = catchflow.Type;
                            catchstatement.Body = TransformCatch(catchstatement, catchflow.Body);
                            statement.CatchList.Add(catchstatement);
                        }
                    }
                    if (trycath.Finally != null)
                    {
                        statement.Finally = Transform(trycath.Finally);
                    }
                    statements.Add(statement);
                }
                else
                {
                    // Cas impossible
                    throw new InvalidOperationException();
                }
            }
        }

        void IExpressionBuilderClient.Exec(Expression expr)
        {
            if (builder.StackHeight != 0)
            {
                throw new InvalidOperationException(expr.IlOffset.ToString());
            }
            statements.Add(expr);
        }

        void IExpressionBuilderClient.Branch(Expression condition, Instruction instruction)
        {
            builder.UnsetEnforceInline(); // Branch denotes end of block, so enforceinline is no more relevant if set

            if (current.Block.Last != instruction)
            {
                throw new InvalidOperationException();
            }

            Condition cond = current as Condition;
            if (cond != null)
            {
                BranchCondition(cond, condition, instruction);
                return;
            }
            if (instruction.StackAfter != 0)
            {
                throw new AstBuilderException(instruction.Offset, string.Format("Unsupported flow stack behaviour. Stack='{0}' Current='{1}'", instruction.StackAfter, current.GetType().Name));
            }
            PreLoop loop = current as PreLoop;
            if (loop != null)
            {
                WhileStatement @while = new WhileStatement();
                if (loop.Jump == LoopBody.Jump)
                {
                    @while.Condition = condition;
                }
                else
                {
                    @while.Condition = condition.Negate();
                }
                @while.Body = Transform(loop.Body);
                statements.Add(@while);
                return;
            }
            Switch sw = current as Switch;
            if (sw != null)
            {
                SwitchStatement @switch = new SwitchStatement();
                @switch.Value = condition;
                foreach (KeyValuePair<int, List<Sequence>> pair in sw.Cases)
                {
                    Case @case = new Case();
                    if (pair.Key == -1)
                    {
                        @case.Value = Case.DefaultCase;
                    }
                    else
                    {
                        @case.Value = pair.Key;
                    }
                    @case.Statements = Transform(pair.Value);
                    @switch.Cases.Add(@case);
                }
                statements.Add(@switch);
                return;
            }
            if (doWhileStatement != null)
            {
                doWhileStatement.Condition = condition;
                return;
            }
            if (current.Block.Successors.Length > 1)
            {
                throw new InvalidOperationException();
            }
        }

        private void BranchCondition(Condition cond, Expression condition, Instruction instruction)
        {
            if (cond.StackAfter != 0)
            {
                int contentLowestStack = cond.ContentLowestStack;
                // Count how many elements from stack are used by Jump/NoJump
                int poped = instruction.StackAfter - contentLowestStack;
                // Count how many elements are added on stack by Jump/NoJump
                int pushed = cond.StackAfter - contentLowestStack;
                if (poped == 0 && pushed == 1 && cond.Jump != null && cond.NoJump != null)
                {
                    // No element poped, one pushed : it can be transformed into a ternary expression
                    Expression @then = InlineExpressionBuilder.Transform(builder, cond.Jump);
                    Expression @else = InlineExpressionBuilder.Transform(builder, cond.NoJump);
                    builder.Push(new ConditionExpression(instruction.Offset, condition, @then, @else));
                }
                else
                {
                    // Generic approach for condition operation with a non empty stack
                    BranchConditionNonEmptyStack(cond, condition, pushed, poped);
                }
            }
            else
            {
                // Regular (empty stack) condition operation
                BranchConditionRegular(cond, condition);
            }
        }

        private List<Statement> TransformConditionBranch(List<Sequence> list, List<LocalVariable> restore, List<LocalVariable> collect)
        {
            Contract.Assert(builder.StackHeight == 0);
            foreach (var register in restore)
            {
                builder.Push(new VariableReferenceExpression(null, register));
            }
            List<Statement> branchStatements = new StatementBuilder(builder, list, null, null).statements;
            List<Expression> toassign = new List<Expression>();
            for (int i = 0; i < collect.Count; ++i)
            {
                toassign.Add(builder.Pop());
            }
            Contract.Assert(builder.StackHeight == 0);
            for (int i = 0; i < collect.Count; ++i)
            {
                branchStatements.Add(new AssignExpression(null, new VariableReferenceExpression(null, collect[i]), toassign[toassign.Count - i - 1]));
            }
            return branchStatements;
        }

        private void BranchConditionNonEmptyStack(Condition cond, Expression condition, int pushed, int poped)
        {
            // Save full stack to variables ("registers")
            // Allocate variables to collect pushed data
            // At the begenning of each branch restore poped data in stack using "resgisters"
            // At the end of each branch collect pushed data into variables
            // After branches restore stack using "registers" and push variables data

            // TODO: consider using "real" registers to allow re-use (for both "registers" and "collect")
            // Some register are written, this may cause some problems.

            List<LocalVariable> registers = builder.FullStackToVariables();
            List<LocalVariable> restoreRegisters = registers.Take(registers.Count - poped).ToList();
            List<LocalVariable> branchRegisters = registers.Skip(registers.Count - poped).ToList();
            List<LocalVariable> collect;

            Contract.Assert(branchRegisters.Count == poped);

            IfStatement @if = new IfStatement();
            if (cond.Jump != null && cond.NoJump != null)
            {
                collect = Enumerable.Range(0, pushed).Select(c => builder.MethodBuilt.AllocateVariable(typeof(object))).ToList();
                @if.Condition = condition.Negate();
                @if.Then = TransformConditionBranch(cond.NoJump, branchRegisters, collect);
                @if.Else = TransformConditionBranch(cond.Jump, branchRegisters, collect);
            }
            else
            {
                Contract.Assert(pushed == poped);
                // If condition has only one branch, branch should simply write in registers
                collect = branchRegisters;
                // FIXME: Ask for mutables registers to avoid problems
                if (cond.Jump != null)
                {
                    @if.Condition = condition;
                    @if.Then = TransformConditionBranch(cond.Jump, branchRegisters, collect);
                }
                else if (cond.NoJump != null)
                {
                    @if.Condition = condition.Negate();
                    @if.Then = TransformConditionBranch(cond.NoJump, branchRegisters, collect);
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
            statements.Add(@if);

            foreach (var register in restoreRegisters)
            {
                builder.Push(new VariableReferenceExpression(null, register));
            }
            foreach (var register in collect)
            {
                builder.Push(new VariableReferenceExpression(null, register));
            }
        }

        private void BranchConditionRegular(Condition cond, Expression condition)
        {
            IfStatement @if = new IfStatement();
            if (cond.Jump != null && cond.NoJump != null)
            {
                @if.Condition = condition.Negate();
                @if.Then = Transform(cond.NoJump);
                @if.Else = Transform(cond.Jump);
            }
            else if (cond.Jump != null)
            {
                @if.Condition = condition;
                @if.Then = Transform(cond.Jump);
            }
            else if (cond.NoJump != null)
            {
                @if.Condition = condition.Negate();
                @if.Then = Transform(cond.NoJump);
            }
            else
            {
                throw new InvalidOperationException();
            }
            statements.Add(@if);
        }



        private List<Statement> Transform(List<Sequence> list)
        {
            return new StatementBuilder(builder, list, null, null).statements;
        }

        private List<Statement> TransformCatch(Catch @catch, List<Sequence> list)
        {
            return new StatementBuilder(builder, list, @catch, null).statements;
        }

        private Statement TransformDoWhile(PostLoop doWhile)
        {
            DoWhileStatement statement = new DoWhileStatement();
            statement.Body = new StatementBuilder(builder, doWhile.Body, null, statement).statements;
            if (doWhile.Jump == LoopBody.NoJump)
            {
                statement.Condition = statement.Condition.Negate();
            }
            return statement;
        }

        internal static MethodAst Transform(MethodCil body, List<Sequence> list)
        {
            MethodAst ast = new MethodAst(body);
            ast.Statements = new StatementBuilder(ast, body, list).statements;
            return ast;
        }

        void IExpressionBuilderClient.Return(Expression expr)
        {
            statements.Add(new ReturnStatement() { Value = expr });
        }
        void IExpressionBuilderClient.Throw(Expression expr)
        {
            statements.Add(new ThrowStatement(expr));
        }
    }
}
