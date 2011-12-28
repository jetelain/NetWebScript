using System.Collections.Generic;
using System.Linq;
using NetWebScript.JsClr.Ast;
using NetWebScript.JsClr.AstBuilder.Cil;
using NetWebScript.JsClr.AstPattern;
using NetWebScript.JsClr.AstPattern.NetAst;

namespace NetWebScript.JsClr.AstBuilder.AstFilter
{
    /// <summary>
    /// 
    /// </summary>
    class SwitchCaseString : AstFilterBase
    {
        private static readonly NetPatternBase SwitchCaseOnStringPattern = new IfStatementPattern()
        {
            Condition = new BinaryExpressionPattern()
            {
                Left = new AnyVariableOrAssignPattern() { Name = "switchVar", ValueName = "switchValue", Type = typeof(string) },
                Operator = BinaryOperator.ValueInequality,
                Right = new LiteralExpressionPattern() { Value = null }
            },
            Then = new ExactSequencePattern<Statement>()
            {
                new IfStatementPattern()
                {
                    Condition = new BinaryExpressionPattern()
                    {
                        Left = new AnyStaticFieldPattern() { Name = "cachedDict", Type = typeof(Dictionary<string,int>) },
                        Operator = BinaryOperator.ValueEquality,
                        Right = new LiteralExpressionPattern() { Value = null }
                    },
                    Then = new CompositeSequencePattern<Statement>() 
                    { 
                        new ExactSequencePattern<Statement>()
                        {
                            new AssignExpressionPattern()
                            {
                                Target = new AnyVariablePattern() { Name = "cachedDictVar", Type = typeof(Dictionary<string,int>) },
                                Value = new ObjectCreationPattern() 
                                {
                                    ConstructorOwner = typeof(Dictionary<string,int>),
                                    Arguments = new List<PatternBase<Statement>>()
                                    {
                                        new AnyLiteralPattern() { Type = typeof(int) }
                                    }
                                }
                            }
                        },
                        new RepeatSequencePattern<Statement>()
                        {
                            Name = "fillCachedDict",
                            Pattern = new MethodInvocationPattern()
                            {
                                MethodName = "Add",
                                Target = new AnyVariablePattern() { Name = "cachedDictVar", Type = typeof(Dictionary<string,int>) },
                                Arguments = new List<PatternBase<Statement>>()
                                {
                                    new AnyLiteralPattern() { Type = typeof(string) },
                                    new AnyLiteralPattern() { Type = typeof(int) }
                                }
                            }
                        },
                        new ExactSequencePattern<Statement>()
                        {
                            new AssignExpressionPattern()
                            {
                                Target = new AnyStaticFieldPattern() { Name = "cachedDict", Type = typeof(Dictionary<string,int>) },
                                Value = new AnyVariablePattern() { Name = "cachedDictVar", Type = typeof(Dictionary<string,int>) }
                            }
                        }
                    }
                },
                new IfStatementPattern()
                {
                    Condition = new MethodInvocationPattern()
                    {
                        MethodName = "TryGetValue",
                        Target = new AnyStaticFieldPattern() { Name = "cachedDict", Type = typeof(Dictionary<string,int>) },
                        Arguments = new List<PatternBase<Statement>>()
                        {
                            new AnyVariablePattern() { Name = "switchVar", Type = typeof(string) },
                            new AnyMakeByRefVariablePattern() { Name="intermediateVar", Type = typeof(int) }
                        }
                    },
                    Then = new ExactSequencePattern<Statement>()
                        {
                            new SwitchStatementPattern()
                            {
                                Value =  new AnyVariablePattern() { Name = "intermediateVar", Type = typeof(int) },
                                StatementName = "switchStatement"
                            }   
                        },
                    Else = new AnySequencePattern<Statement>() { Name = "defaultBlock" }
                }
            },
            Else = new AnySequencePattern<Statement>() { Name = "nullBlock" }
        };

        public override Statement Visit(IfStatement ifStatement)
        {
            var context = new PatternContext();
            if (SwitchCaseOnStringPattern.Test(ifStatement, context))
            {
                List<Statement> nullBlock = (List<Statement>)context.GetCapture("nullBlock");
                List<Statement> defaultBlock = (List<Statement>)context.GetCapture("defaultBlock");
                List<Statement> fillCachedDict = (List<Statement>)context.GetCapture("fillCachedDict");
                SwitchStatement @switch = (SwitchStatement)context.GetCapture("switchStatement");
                Expression @switchValue = (Expression)context.GetCapture("switchValue");

                SwitchStatement @newSwitch = new SwitchStatement();
                @newSwitch.Value = @switchValue;

                foreach (MethodInvocationExpression mie in fillCachedDict)
                {
                    var str = (string)((LiteralExpression)mie.Arguments[0]).Value;
                    var num = (int)((LiteralExpression)mie.Arguments[1]).Value;

                    var @case = @switch.Cases.First(c => (int)c.Value == num);

                    @newSwitch.Cases.Add(new Case() { Value = str, Statements = @case.Statements });
                }
                if (nullBlock != null)
                {
                    @newSwitch.Cases.Add(new Case() { Value = null, Statements = nullBlock });
                }
                if (defaultBlock != null)
                {
                    @newSwitch.Cases.Add(new Case() { Value = Case.DefaultCase, Statements = nullBlock });
                }
                return @newSwitch;
            }

            return base.Visit(ifStatement);
        }
    }
}
