using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.JsClr.Ast;
using NetWebScript.JsClr.AstBuilder.Flow;
using NetWebScript.JsClr.AstBuilder.Cil;

namespace NetWebScript.JsClr.AstBuilder
{
    internal sealed class InlineExpressionBuilder : IExpressionBuilderClient
    {
        private readonly ExpressionBuilder builder;

        private readonly IEnumerator<Sequence> enumerator;
        private SingleBlock current;

        private readonly Expression result;

        private InlineExpressionBuilder(ExpressionBuilder builder, List<Sequence> sequences)
        {
            this.builder = builder;
            builder.PushClient(this);
            enumerator = sequences.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Process(enumerator.Current);
            }
            result = builder.Pop();
            builder.PopClient(this);
        }


        internal static Expression Transform(ExpressionBuilder builder, List<Sequence> sequences)
        {
            return new InlineExpressionBuilder(builder, sequences).result;
        }

        private void Process(Sequence sequence)
        {
            current = sequence as SingleBlock;
            if (current != null)
            {
                if (current is PreLoop || current is Switch/* || current is PostLoop*/)
                {
                    throw new NotImplementedException();
                }
                foreach (Instruction instruction in current.Block)
                {
                    builder.Visit(instruction);
                }
            }
            else
            {
                // Cas impossible
                throw new InvalidOperationException();
            }
        }

        public void Exec(Expression expr)
        {
            throw new NotImplementedException();
        }

        public void Branch(Expression condition, NetWebScript.JsClr.AstBuilder.Cil.Instruction instruction)
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
                        Expression @then = Transform(builder, cond.Jump);
                        Expression @else = Transform(builder, cond.NoJump);
                        // Transformation en ternaire
                        builder.Push(new ConditionExpression(instruction.Offset, condition, @then, @else));
                        if (enumerator.MoveNext())
                        {
                            Process(enumerator.Current);
                        }
                        return;
                    }
                }
                throw new NotImplementedException();
            }

            if (current.Block.Successors.Length != 1)
            {
                throw new NotImplementedException();
            }
        }

        void IExpressionBuilderClient.Return(Expression expr)
        {
            throw new NotImplementedException();
        }
        void IExpressionBuilderClient.Throw(Expression expr)
        {
            throw new NotImplementedException();
        }
    }
}
