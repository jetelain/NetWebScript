using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.JsClr.Ast;
using NetWebScript.JsClr.AstBuilder.Cil;
using NetWebScript.JsClr.AstBuilder.Flow;

namespace NetWebScript.JsClr.AstBuilder
{
    /// <summary>
    /// Class to build a <see cref="Statement"/> list from an <see cref="Sequence"/> list.
    /// </summary>
    internal sealed class StatementBuilder : IExpressionBuilderClient
    {
        private readonly List<Statement> statements = new List<Statement>();

        private readonly MethodCil body;
        private readonly MethodAst built;

        private readonly IEnumerator<Sequence> enumerator;

        private SingleBlock current;

        private readonly ExpressionBuilder builder;
        private readonly Catch @catch;
        private readonly DoWhileStatement doWhileStatement;

        private StatementBuilder(MethodAst built, MethodCil body, List<Sequence> sequences, Catch @catch, DoWhileStatement doWhileStatement)
        {
            this.body = body;
            this.built = built;
            this.@catch = @catch;
            this.doWhileStatement = doWhileStatement;

            builder = new ExpressionBuilder(built, body, this);

            if (@catch != null)
            {
                builder.Push(new CurrentExceptionExpression(@catch.Type));
            }

            enumerator = sequences.GetEnumerator();

            while(enumerator.MoveNext())
            {
                Process(enumerator.Current);
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
                    builder.UnsetEnforceInline();
                }
                else
                {
                    foreach (Instruction instruction in current.Block)
                    {
                        builder.Visit(instruction);
                    }
                }

                if (builder.StackHeight != 0)
                {
                    throw new AstBuilderException(current.Block.Last.Offset, "Stack should be empty here");
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
            if (current.Block.Last != instruction)
            {
                // Cas impossible
                throw new InvalidOperationException();
            }

            Condition cond = current as Condition;
            if (cond != null)
            {
                if (cond.StackAfter != 0)
                {
                    int delta = cond.StackAfter - instruction.StackAfter;
                    if (delta == 1 && cond.Jump != null && cond.NoJump != null)
                    {
                        Expression @then = InlineExpressionBuilder.Transform(builder, cond.Jump);
                        Expression @else = InlineExpressionBuilder.Transform(builder, cond.NoJump);
                        // Transformation en ternaire
                        builder.Push(new ConditionExpression(instruction.Offset, condition, @then, @else));

                        if (enumerator.MoveNext())
                        {
                            Process(enumerator.Current);
                        }

                        return;
                    }
                    else
                    {
                        throw new AstBuilderException(instruction.Offset, "Unsupported condition expression");
                    }
                }
                else
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
                return;
            }
            if (instruction.StackAfter != 0)
            {
                throw new AstBuilderException(instruction.Offset, "Unsupported flow stack behaviour");
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
            //DoWhile doWhile = current as DoWhile;
            //if (doWhile != null)
            //{
            //    DoWhileStatement @while = new DoWhileStatement();
            //    if (loop.Jump == LoopBody.Jump)
            //    {
            //        @while.Condition = condition;
            //    }
            //    else
            //    {
            //        @while.Condition = condition.Negate();
            //    }
            //    @while.Body = Transform(loop.Body);
            //    statements.Add(@while);
            //    return;
            //}
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

        private List<Statement> Transform(List<Sequence> list)
        {
            return new StatementBuilder(built, body, list, null, null).statements;
        }

        private List<Statement> TransformCatch(Catch @catch, List<Sequence> list)
        {
            return new StatementBuilder(built, body, list, @catch, null).statements;
        }

        private Statement TransformDoWhile(PostLoop doWhile)
        {
            DoWhileStatement statement = new DoWhileStatement();
            statement.Body = new StatementBuilder(built, body, doWhile.Body, null, statement).statements;
            if (doWhile.Jump == LoopBody.NoJump)
            {
                statement.Condition = statement.Condition.Negate();
            }
            return statement;
        }

        internal static MethodAst Transform(MethodCil body, List<Sequence> list)
        {
            MethodAst ast = new MethodAst(body);
            ast.Statements = new StatementBuilder(ast, body, list, null, null).statements;
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
