using System;
using System.Collections.Generic;
using System.Globalization;
using Jint;
using Jint.Expressions;
using NetWebScript.JsClr.JsBuilder.JsSyntax;
using NetWebScript.JsClr.ScriptAst;

namespace NetWebScript.JsClr.JsBuilder.Pattern
{
    public class InlineFragment
    {
        private readonly Program program;
        
        public InlineFragment(string fragment)
        {
            program = JintEngine.Compile(fragment, false);
        }

        public JsToken Execute(IDictionary<string, JsToken> locals)
        {
            var visitor = new Visitor(locals);
            program.Accept(visitor);
            return visitor.result;
        }

        private class Visitor : IStatementVisitor
        {
            public JsToken result;
            private readonly IDictionary<string, JsToken> locals;

            public Visitor(IDictionary<string, JsToken> locals)
            {
                this.locals = locals;
            }

            #region IStatementVisitor Members

            public void Visit(Statement expression)
            {
                throw new NotImplementedException();
            }

            public void Visit(RegexpExpression expression)
            {
                throw new NotImplementedException();
            }

            public void Visit(ValueExpression expression)
            {
                switch (expression.TypeCode)
                {
                    case TypeCode.Boolean: 
                        result = JsToken.Name((bool)expression.Value ? "true":"false"); 
                        break;
                    case TypeCode.Int32:
                    case TypeCode.Single:
                    case TypeCode.Double: 
                        result = JsToken.Name(Convert.ToDouble(expression.Value).ToString(CultureInfo.InvariantCulture)); 
                        break;
                    case TypeCode.String: 
                        result = JsToken.LiteralString((string)expression.Value); 
                        break;
                    default:
                        throw new NotSupportedException(expression.TypeCode.ToString());
                }
            }

            public void Visit(UnaryExpression expression)
            {
                result = null;
                expression.Expression.Accept(this);
                var target = result;

                ScriptUnaryOperator op = ScriptUnaryOperator.BitwiseNot;
                string strOp = null;

                var left = result;
                switch (expression.Type)
                {
                    case UnaryExpressionType.Delete:
                        strOp = "delete ";
                        break;
                    case UnaryExpressionType.Inv:
                        op = ScriptUnaryOperator.BitwiseNot;
                        break;
                    case UnaryExpressionType.Negate:
                        op = ScriptUnaryOperator.Negate;
                        break;
                    case UnaryExpressionType.Not:
                        op = ScriptUnaryOperator.LogicalNot;
                        break;
                    case UnaryExpressionType.PostfixMinusMinus:
                        op = ScriptUnaryOperator.PostDecrement;
                        break;
                    case UnaryExpressionType.PostfixPlusPlus:
                        op = ScriptUnaryOperator.PostIncrement;
                        break;
                    case UnaryExpressionType.PrefixMinusMinus:
                        op = ScriptUnaryOperator.PreDecrement;
                        break;
                    case UnaryExpressionType.PrefixPlusPlus:
                        op = ScriptUnaryOperator.PreIncrement;
                        break;
                    case UnaryExpressionType.TypeOf:
                        strOp = "typeof ";
                        break;
                    default:
                        throw new NotSupportedException(expression.Type.ToString());
                }
                if (strOp != null)
                {
                    result = JsToken.Combine(strOp, target);
                }
                else
                {
                    result = JsToken.Combine(target, op);
                }
            }

            public void Visit(TernaryExpression expression)
            {
                result = null;
                expression.LeftExpression.Accept(this);
                var left = result;

                result = null;
                expression.MiddleExpression.Accept(this);
                var middle = result;

                result = null;
                expression.RightExpression.Accept(this);
                var right = result;

                result = JsToken.Condition(left, middle, right);
            }

            public void Visit(BinaryExpression expression)
            {
                result = null;
                expression.LeftExpression.Accept(this);
                var left = result;

                result = null;
                expression.RightExpression.Accept(this);
                var right = result;

                ScriptAst.ScriptBinaryOperator op = ScriptAst.ScriptBinaryOperator.Add;
                string strOp = null;

                switch (expression.Type)
                {
                    case BinaryExpressionType.And:
                        op = ScriptAst.ScriptBinaryOperator.LogicalAnd;
                        break;
                    case BinaryExpressionType.BitwiseAnd:
                        op = ScriptAst.ScriptBinaryOperator.BitwiseAnd;
                        break;
                    case BinaryExpressionType.BitwiseOr:
                        op = ScriptAst.ScriptBinaryOperator.BitwiseOr;
                        break;
                    case BinaryExpressionType.BitwiseXOr:
                        op = ScriptAst.ScriptBinaryOperator.BitwiseXor;
                        break;
                    case BinaryExpressionType.Div:
                        op = ScriptAst.ScriptBinaryOperator.Divide;
                        break;
                    case BinaryExpressionType.Equal:
                        strOp = "==";
                        break;
                    case BinaryExpressionType.Greater:
                        op = ScriptAst.ScriptBinaryOperator.GreaterThan;
                        break;
                    case BinaryExpressionType.GreaterOrEqual:
                        op = ScriptAst.ScriptBinaryOperator.GreaterThanOrEqual;
                        break;
                    case BinaryExpressionType.LeftShift:
                        op = ScriptAst.ScriptBinaryOperator.LeftShift;
                        break;
                    case BinaryExpressionType.Lesser:
                        op = ScriptAst.ScriptBinaryOperator.LessThan;
                        break;
                    case BinaryExpressionType.LesserOrEqual:
                        op = ScriptAst.ScriptBinaryOperator.LessThanOrEqual;
                        break;
                    case BinaryExpressionType.Minus:
                        op = ScriptAst.ScriptBinaryOperator.Subtract;
                        break;
                    case BinaryExpressionType.Modulo:
                        op = ScriptAst.ScriptBinaryOperator.Modulo;
                        break;
                    case BinaryExpressionType.NotEqual:
                        strOp = "!=";
                        break;
                    case BinaryExpressionType.NotSame:
                        op = ScriptAst.ScriptBinaryOperator.ValueInequality;
                        break;
                    case BinaryExpressionType.Or:
                        op = ScriptAst.ScriptBinaryOperator.LogicalOr;
                        break;
                    case BinaryExpressionType.Plus:
                        op = ScriptAst.ScriptBinaryOperator.Add;
                        break;
                    case BinaryExpressionType.Pow:
                        break;
                    case BinaryExpressionType.RightShift:
                        op = ScriptAst.ScriptBinaryOperator.RightShift;
                        break;
                    case BinaryExpressionType.Same:
                        op = ScriptAst.ScriptBinaryOperator.ValueEquality;
                        break;
                    case BinaryExpressionType.Times:
                        op = ScriptAst.ScriptBinaryOperator.Multiply;
                        break;
                    case BinaryExpressionType.InstanceOf:
                        strOp = " instanceof ";
                        break;
                    default:
                        throw new NotSupportedException();
                }
                if (strOp != null)
                {
                    result = JsToken.Combine(left, strOp, right);
                }
                else
                {
                    result = JsToken.Combine(left, op, right);
                }
            }

            public void Visit(NewExpression expression)
            {
                foreach (var property in expression.Identifiers)
                {
                    property.Accept(this);
                }
                var target = result;

                var parameters = new List<JsToken>(expression.Arguments.Count);
                foreach (var arg in expression.Arguments)
                {
                    result = null;
                    arg.Accept(this);
                    parameters.Add(result);
                }

                result = JsToken.CreateInstance(target, parameters);
            }

            public void Visit(JsonExpression expression)
            {
                throw new NotImplementedException();
            }

            public void Visit(Identifier expression)
            {
                var name = expression.Text;
                if (!locals.TryGetValue(name, out result))
                {
                    result = JsToken.Name(name);
                }
            }

            public void Visit(PropertyDeclarationExpression expression)
            {
                throw new NotImplementedException();
            }

            public void Visit(PropertyExpression expression)
            {
                var name = expression.Text;
                if (result != null)
                {
                    result = JsToken.Member(result, name);
                }
                else
                {
                    if (!locals.TryGetValue(name, out result))
                    {
                        result = JsToken.Name(name);
                    }
                } 
            }

            public void Visit(Indexer expression)
            {
                var target = result;

                result = null;
                expression.Index.Accept(this);
                var index = result;

                result = JsToken.Indexer(target, index);
            }

            public void Visit(MethodCall expression)
            {
                var target = result;
                var parameters = new List<JsToken>(expression.Arguments.Count);
                foreach ( var arg in expression.Arguments)
                {
                    result = null;
                    arg.Accept(this);
                    parameters.Add(result);
                }
                result = JsToken.Call(target, parameters);
            }

            public void Visit(MemberExpression expression)
            {
                if (expression.Previous != null)
                {
                    expression.Previous.Accept(this);
                }
                expression.Member.Accept(this);
            }

            public void Visit(FunctionExpression expression)
            {
                throw new NotImplementedException();
            }

            public void Visit(CommaOperatorStatement expression)
            {
                throw new NotImplementedException();
            }

            public void Visit(ArrayDeclaration expression)
            {
                throw new NotImplementedException();
            }

            public void Visit(WhileStatement expression)
            {
                throw new NotImplementedException();
            }

            public void Visit(VariableDeclarationStatement expression)
            {
                throw new NotImplementedException();
            }

            public void Visit(TryStatement expression)
            {
                throw new NotImplementedException();
            }

            public void Visit(ThrowStatement expression)
            {
                throw new NotImplementedException();
            }

            public void Visit(WithStatement expression)
            {
                throw new NotImplementedException();
            }

            public void Visit(SwitchStatement expression)
            {
                throw new NotImplementedException();
            }

            public void Visit(ReturnStatement expression)
            {
                throw new NotImplementedException();
            }

            public void Visit(IfStatement expression)
            {
                throw new NotImplementedException();
            }

            public void Visit(FunctionDeclarationStatement expression)
            {
                throw new NotImplementedException();
            }

            public void Visit(ForStatement expression)
            {
                throw new NotImplementedException();
            }

            public void Visit(ForEachInStatement expression)
            {
                throw new NotImplementedException();
            }

            public void Visit(ExpressionStatement expression)
            {
                result = null;
                expression.Expression.Accept(this);
            }

            public void Visit(EmptyStatement expression)
            {
                throw new NotImplementedException();
            }

            public void Visit(DoWhileStatement expression)
            {
                throw new NotImplementedException();
            }

            public void Visit(ContinueStatement expression)
            {
                throw new NotImplementedException();
            }

            public void Visit(BreakStatement expression)
            {
                throw new NotImplementedException();
            }

            public void Visit(BlockStatement expression)
            {
                throw new NotImplementedException();
            }

            public void Visit(AssignmentExpression statement)
            {
                switch (statement.AssignmentOperator)
                {
                    case AssignmentOperator.Assign: statement.Right.Accept(this);
                        break;
                    //case AssignmentOperator.Multiply: new BinaryExpression(BinaryExpressionType.Times, statement.Left, statement.Right).Accept(this);
                    //    break;
                    //case AssignmentOperator.Divide: new BinaryExpression(BinaryExpressionType.Div, statement.Left, statement.Right).Accept(this);
                    //    break;
                    //case AssignmentOperator.Modulo: new BinaryExpression(BinaryExpressionType.Modulo, statement.Left, statement.Right).Accept(this);
                    //    break;
                    //case AssignmentOperator.Add: new BinaryExpression(BinaryExpressionType.Plus, statement.Left, statement.Right).Accept(this);
                    //    break;
                    //case AssignmentOperator.Substract: new BinaryExpression(BinaryExpressionType.Minus, statement.Left, statement.Right).Accept(this);
                    //    break;
                    //case AssignmentOperator.ShiftLeft: new BinaryExpression(BinaryExpressionType.LeftShift, statement.Left, statement.Right).Accept(this);
                    //    break;
                    //case AssignmentOperator.ShiftRight: new BinaryExpression(BinaryExpressionType.RightShift, statement.Left, statement.Right).Accept(this);
                    //    break;
                    //case AssignmentOperator.UnsignedRightShift: new BinaryExpression(BinaryExpressionType.UnsignedRightShift, statement.Left, statement.Right).Accept(this);
                    //    break;
                    //case AssignmentOperator.And: new BinaryExpression(BinaryExpressionType.BitwiseAnd, statement.Left, statement.Right).Accept(this);
                    //    break;
                    //case AssignmentOperator.Or: new BinaryExpression(BinaryExpressionType.BitwiseOr, statement.Left, statement.Right).Accept(this);
                    //    break;
                    //case AssignmentOperator.XOr: new BinaryExpression(BinaryExpressionType.BitwiseXOr, statement.Left, statement.Right).Accept(this);
                    //    break;
                    default: throw new NotSupportedException(statement.AssignmentOperator.ToString());
                }

                var right = result;

                result = null;
                MemberExpression left = statement.Left as MemberExpression;
                if (left == null)
                {
                    left = new MemberExpression(statement.Left, null);
                }
                left.Accept(this);
                
                result = JsToken.Assign(result, right);
            }

            public void Visit(Program program)
            {
                foreach (var statement in program.Statements)
                {
                    statement.Accept(this);
                }
            }

            #endregion
        }


    }
}
