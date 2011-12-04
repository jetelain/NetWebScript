using System;
using NetWebScript.JsClr.ScriptAst;

namespace NetWebScript.JsClr.JsBuilder.JsSyntax
{
    internal static class JsOperators
    {
        internal static string ToString(ScriptUnaryOperator op)
        {
            switch (op)
            {
                case ScriptUnaryOperator.BitwiseNot:
                    return "~";
                case ScriptUnaryOperator.LogicalNot:
                    return "!";
                case ScriptUnaryOperator.Negate:
                    return "-";
                case ScriptUnaryOperator.PostDecrement:
                case ScriptUnaryOperator.PreDecrement:
                    return "--";
                case ScriptUnaryOperator.PostIncrement:
                case ScriptUnaryOperator.PreIncrement:
                    return "++";
                default: throw new ArgumentException();
            }
        }

        internal static string ToString(ScriptBinaryOperator op)
        {
            switch (op)
            {
                case ScriptBinaryOperator.Add: return "+";
                case ScriptBinaryOperator.BitwiseAnd: return "&";
                case ScriptBinaryOperator.BitwiseOr: return "|";
                case ScriptBinaryOperator.BitwiseXor: return "^";
                case ScriptBinaryOperator.Divide: return "/";
                case ScriptBinaryOperator.GreaterThan: return ">";
                case ScriptBinaryOperator.GreaterThanOrEqual: return ">=";
                case ScriptBinaryOperator.LeftShift: return "<<";
                case ScriptBinaryOperator.LessThan: return "<";
                case ScriptBinaryOperator.LessThanOrEqual: return "<=";
                case ScriptBinaryOperator.LogicalAnd: return "&&";
                case ScriptBinaryOperator.LogicalOr: return "||";
                case ScriptBinaryOperator.Modulo: return "%";
                case ScriptBinaryOperator.Multiply: return "*";
                case ScriptBinaryOperator.RightShift: return ">>";
                case ScriptBinaryOperator.Subtract: return "-";
                case ScriptBinaryOperator.ValueEquality: return "===";
                case ScriptBinaryOperator.ValueInequality: return "!==";
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

        internal static JsPrecedence GetPrecedence(ScriptBinaryOperator operationtype)
        {
            switch (operationtype)
            {
                case ScriptBinaryOperator.Multiply:
                case ScriptBinaryOperator.Divide:
                case ScriptBinaryOperator.Modulo:
                    return JsPrecedence.MultiplyDivideModulo;
                case ScriptBinaryOperator.Add:
                case ScriptBinaryOperator.Subtract:
                    return JsPrecedence.AddSubtract;
                case ScriptBinaryOperator.RightShift:
                case ScriptBinaryOperator.LeftShift:
                    return JsPrecedence.Shift;
                case ScriptBinaryOperator.LessThan:
                case ScriptBinaryOperator.LessThanOrEqual:
                case ScriptBinaryOperator.GreaterThan:
                case ScriptBinaryOperator.GreaterThanOrEqual:
                    return JsPrecedence.Relational;
                case ScriptBinaryOperator.ValueEquality:
                case ScriptBinaryOperator.ValueInequality:
                    return JsPrecedence.EqualityInequality;
                case ScriptBinaryOperator.BitwiseAnd:
                    return JsPrecedence.BitwiseAnd;
                case ScriptBinaryOperator.BitwiseXor:
                    return JsPrecedence.BitwiseXor;
                case ScriptBinaryOperator.BitwiseOr:
                    return JsPrecedence.BitwiseOr;
                case ScriptBinaryOperator.LogicalAnd:
                    return JsPrecedence.LogicalAnd;
                case ScriptBinaryOperator.LogicalOr:
                    return JsPrecedence.LogicalOr;
                default:
                    throw new NotImplementedException(operationtype.ToString());
            }
        }

        internal static JsPrecedence GetPrecedence(ScriptUnaryOperator operationtype)
        {
            switch (operationtype)
            {
                case ScriptUnaryOperator.Negate:
                case ScriptUnaryOperator.LogicalNot:
                case ScriptUnaryOperator.BitwiseNot:
                    return JsPrecedence.NegateNot;
                case ScriptUnaryOperator.PostDecrement:
                case ScriptUnaryOperator.PostIncrement:
                case ScriptUnaryOperator.PreDecrement:
                case ScriptUnaryOperator.PreIncrement:
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
