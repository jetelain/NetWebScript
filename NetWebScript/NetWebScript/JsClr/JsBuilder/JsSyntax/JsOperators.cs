using System;
using NetWebScript.JsClr.Ast;

namespace NetWebScript.JsClr.JsBuilder.JsSyntax
{
    internal static class JsOperators
    {
        internal static string ToString(UnaryOperator op)
        {
            switch (op)
            {
                case UnaryOperator.BitwiseNot:
                    return "~";
                case UnaryOperator.LogicalNot:
                    return "!";
                case UnaryOperator.Negate:
                    return "-";
                case UnaryOperator.PostDecrement:
                case UnaryOperator.PreDecrement:
                    return "--";
                case UnaryOperator.PostIncrement:
                case UnaryOperator.PreIncrement:
                    return "++";
                default: throw new ArgumentException();
            }
        }

        internal static string ToString(BinaryOperator op)
        {
            switch (op)
            {
                case BinaryOperator.Add: return "+";
                case BinaryOperator.BitwiseAnd: return "&";
                case BinaryOperator.BitwiseOr: return "|";
                case BinaryOperator.BitwiseXor: return "^";
                case BinaryOperator.Divide: return "/";
                case BinaryOperator.GreaterThan: return ">";
                case BinaryOperator.GreaterThanOrEqual: return ">=";
                case BinaryOperator.LeftShift: return "<<";
                case BinaryOperator.LessThan: return "<";
                case BinaryOperator.LessThanOrEqual: return "<=";
                case BinaryOperator.LogicalAnd: return "&&";
                case BinaryOperator.LogicalOr: return "||";
                case BinaryOperator.Modulo: return "%";
                case BinaryOperator.Multiply: return "*";
                case BinaryOperator.RightShift: return ">>";
                case BinaryOperator.Subtract: return "-";
                case BinaryOperator.ValueEquality: return "===";
                case BinaryOperator.ValueInequality: return "!==";
                default: throw new ArgumentException();
            }
        }

        internal static JsPrecedence GetBinaryPrecedence(String operationtype)
        {
            switch (operationtype)
            {
                case "*":
                case "/":
                case "%":
                    return JsPrecedence.MultiplyDivideModulo;
                case "+":
                case "-":
                    return JsPrecedence.AddSubtract;
                case ">>":
                case "<<":
                    return JsPrecedence.Shift;
                case "<":
                case "<=":
                case ">":
                case ">=":
                    return JsPrecedence.Relational;
                case "==":
                case "!=":
                case "===":
                case "!==":
                    return JsPrecedence.EqualityInequality;
                case "&":
                    return JsPrecedence.BitwiseAnd;
                case "^":
                    return JsPrecedence.BitwiseXor;
                case "|":
                    return JsPrecedence.BitwiseOr;
                case "&&":
                    return JsPrecedence.LogicalAnd;
                case "||":
                    return JsPrecedence.LogicalOr;
                case " instanceof ":
                    return JsPrecedence.Relational;
                default:
                    throw new NotImplementedException(operationtype.ToString());
            }
        }

        internal static JsPrecedence GetPrecedence(BinaryOperator operationtype)
        {
            switch (operationtype)
            {
                case BinaryOperator.Multiply:
                case BinaryOperator.Divide:
                case BinaryOperator.Modulo:
                    return JsPrecedence.MultiplyDivideModulo;
                case BinaryOperator.Add:
                case BinaryOperator.Subtract:
                    return JsPrecedence.AddSubtract;
                case BinaryOperator.RightShift:
                case BinaryOperator.LeftShift:
                    return JsPrecedence.Shift;
                case BinaryOperator.LessThan:
                case BinaryOperator.LessThanOrEqual:
                case BinaryOperator.GreaterThan:
                case BinaryOperator.GreaterThanOrEqual:
                    return JsPrecedence.Relational;
                case BinaryOperator.ValueEquality:
                case BinaryOperator.ValueInequality:
                    return JsPrecedence.EqualityInequality;
                case BinaryOperator.BitwiseAnd:
                    return JsPrecedence.BitwiseAnd;
                case BinaryOperator.BitwiseXor:
                    return JsPrecedence.BitwiseXor;
                case BinaryOperator.BitwiseOr:
                    return JsPrecedence.BitwiseOr;
                case BinaryOperator.LogicalAnd:
                    return JsPrecedence.LogicalAnd;
                case BinaryOperator.LogicalOr:
                    return JsPrecedence.LogicalOr;
                default:
                    throw new NotImplementedException(operationtype.ToString());
            }
        }

        internal static JsPrecedence GetPrecedence(UnaryOperator operationtype)
        {
            switch (operationtype)
            {
                case UnaryOperator.Negate:
                case UnaryOperator.LogicalNot:
                case UnaryOperator.BitwiseNot:
                    return JsPrecedence.NegateNot;
                case UnaryOperator.PostDecrement:
                case UnaryOperator.PostIncrement:
                case UnaryOperator.PreDecrement:
                case UnaryOperator.PreIncrement:
                    return JsPrecedence.PostPreIncrementDecrement;
                default:
                    throw new NotImplementedException(operationtype.ToString());
            }
        }

        internal static JsPrecedence GetUnaryPrecedence(string operationtype)
        {
            switch (operationtype)
            {
                case "-":
                case "!":
                case "~":
                    return JsPrecedence.NegateNot;
                case "++":
                case "--":
                    return JsPrecedence.PostPreIncrementDecrement;
                case "typeof ":
                    return JsPrecedence.Relational; // FIXME
                case "delete ":
                    return JsPrecedence.Statement; // FIXME
                default:
                    throw new NotImplementedException(operationtype.ToString());
            }
        }
    }
}
