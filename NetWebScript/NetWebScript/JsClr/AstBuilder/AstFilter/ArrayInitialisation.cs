using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.JsClr.AstPattern;
using NetWebScript.JsClr.Ast;
using NetWebScript.JsClr.AstPattern.NetAst;
using NetWebScript.JsClr.AstBuilder.Cil;

namespace NetWebScript.JsClr.AstBuilder.AstFilter
{
    class ArrayInitialisation :  AstFilterBase
    {
        // variable = new Array[literal];
        // (variable[literal] = any;)+
        private readonly SubSequencePattern<Statement> Pattern = new SubSequencePattern<Statement>()
        {
            Name = "sequence",
            Pattern = new CompositeSequencePattern<Statement>()
            { 
                new ExactSequencePattern<Statement>()
                {
                     new AssignExpressionPattern()
                     {
                        StatementName = "assign",
                        Target = new AnyVariablePattern() { Name = "array" },
                        Value = new ArrayCreationPattern()
                        {
                            StatementName = "createArray",
                            Size = new AnyLiteralPattern() { Type = typeof(int), Name = "arraySize" }
                        }
                     }
                },
                new RepeatSequencePattern<Statement>()
                {
                    Name = "items",
                    Pattern = new AssignExpressionPattern()
                    {
                        Target = new ArrayIndexerPattern()
                        {
                            Target = new AnyVariablePattern() { Name = "array" },
                            Index = new AnyLiteralPattern() { Type = typeof(int) }
                        },
                        Value = new AnyPattern<Statement>()
                    }
                }
            }
        };

        public override List<Statement> Visit(List<Statement> list)
        {
            PatternContext context = new PatternContext();
            if (Pattern.Test(list, context))
            {
                List<PatternContext> sequences = (List<PatternContext>)context.GetCapture("sequence");
                foreach (var sequence in sequences)
                {
                    var size = (int)sequence.GetCapture("arraySize");
                    var items = (List<Statement>)sequence.GetCapture("items");
                    if (items.Count == size)
                    {
                        List<Expression> values = new List<Expression>();
                        var variableTester = new NoReferenceToVariable((LocalVariable)sequence.GetCapture("array"));
                        bool valid = true;
                        for(int i=0;i<size;++i)
                        {
                            var assign = (AssignExpression)items[i];
                            var indexer = (ArrayIndexerExpression)assign.Target;
                            var indexerValue = (LiteralExpression)indexer.Index;
                            if ((int)indexerValue.Value != i || !assign.Value.Accept(variableTester))
                            {
                                valid = false;
                                break;
                            }
                            values.Add(assign.Value);
                        }

                        if (valid)
                        {
                            var arrayCreation = (ArrayCreationExpression)sequence.GetCapture("createArray");
                            arrayCreation.Initialize = values;
                            int first = list.IndexOf(items[0]);
                            list.RemoveRange(first, items.Count);
                            //foreach (var item in items)
                            //{
                            //    //list.RemoveAll(i => object.ReferenceEquals(i,item));
                            //    list.Remove(item);
                            //}
                        }
                    }
                }
            }
            return base.Visit(list);
        }



    }
}
