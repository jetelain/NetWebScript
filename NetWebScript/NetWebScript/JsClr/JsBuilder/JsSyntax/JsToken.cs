using System;
using System.Collections.Generic;
using NetWebScript.JsClr.Ast;
using NetWebScript.JsClr.ScriptAst;

namespace NetWebScript.JsClr.JsBuilder.JsSyntax
{
    public class JsToken
    {
        public string Text { get; private set; }

        internal JsPrecedence Precedence { get; private set; }

        private JsToken(String text)
        {
            this.Precedence = JsPrecedence.Name;
            this.Text = text;
        }

        internal JsToken(JsPrecedence precedence, String text)
        {
            this.Precedence = precedence;
            this.Text = text;
        }

        private JsToken(JsPrecedence opPrecedence, JsToken a, string op, JsToken b)
        {
            this.Precedence = opPrecedence;
            JsTokenWriter builder = new JsTokenWriter();
            builder.WriteLeft(opPrecedence, a);
            builder.Write(op);
            builder.WriteRight(opPrecedence, b);
            this.Text = builder.ToString();
        }

        private JsToken(JsPrecedence opPrecedence, JsToken v, string op)
        {
            this.Precedence = opPrecedence;
            JsTokenWriter builder = new JsTokenWriter();
            builder.Write(opPrecedence, v);
            builder.Write(op);
            this.Text = builder.ToString();
        }

        private JsToken(JsPrecedence opPrecedence, string op, JsToken v)
        {
            this.Precedence = opPrecedence;
            JsTokenWriter builder = new JsTokenWriter();
            builder.Write(op);
            builder.Write(opPrecedence, v);
            this.Text = builder.ToString();
        }

        private JsToken(JsToken a, JsToken b, JsToken c)
        {
            this.Precedence = JsPrecedence.Conditional;
            JsTokenWriter builder = new JsTokenWriter();
            builder.Write(JsPrecedence.Conditional, a);
            builder.Write('?');
            builder.Write(JsPrecedence.Conditional, b);
            builder.Write(':');
            builder.Write(JsPrecedence.Conditional, c);
            this.Text = builder.ToString();
        }

 

        public JsToken Op(string op, JsToken b)
        {
            return Combine(this, op, b);
        }

        public static JsToken Combine(JsToken a, string op, JsToken b)
        {
            return new JsToken(JsOperators.GetBinaryPrecedence(op), a, op, b);
        }

        public static JsToken Combine(JsToken a, ScriptBinaryOperator op, JsToken b)
        {
            return new JsToken(JsOperators.GetPrecedence(op), a, JsOperators.ToString(op), b);
        }

        private static bool IsPostUnaryOperator(ScriptUnaryOperator op)
        {
            switch (op)
            {
                case ScriptUnaryOperator.PostIncrement:
                case ScriptUnaryOperator.PostDecrement:
                    return true;
                default:
                    return false;
            }
        }

        public static JsToken Combine(string op, JsToken v)
        {
            return new JsToken(JsOperators.GetUnaryPrecedence(op), op, v);
        }

        public static JsToken Combine(JsToken v, string op)
        {
            return new JsToken(JsOperators.GetUnaryPrecedence(op), v, op);
        }

        public static JsToken Combine(JsToken v, ScriptUnaryOperator op)
        {
            if (IsPostUnaryOperator(op))
            {
                return new JsToken(JsOperators.GetPrecedence(op), v, JsOperators.ToString(op));
            }
            else
            {
                return new JsToken(JsOperators.GetPrecedence(op), JsOperators.ToString(op), v);
            }
        }

        public static JsToken Condition(JsToken cond, JsToken @then, JsToken @else)
        {
            return new JsToken(cond, @then, @else);
        }

        public JsToken Call(String name, IEnumerable<JsToken> args)
        {
            return Call(this, name, args);
        }

        public JsToken Call(string name, JsToken a)
        {
            return Call(this, name, new List<JsToken>() { a });
        }

        public JsToken Call(string name, JsToken a, JsToken b)
        {
            return Call(this, name, new List<JsToken>() { a, b });
        }

        public static JsToken Call(JsToken target, String name, IEnumerable<JsToken> args)
        {
            JsTokenWriter builder = new JsTokenWriter();
            builder.Write(JsPrecedence.Member,target);
            builder.Write('.');
            builder.Write(name);
            builder.WriteArgs(args);
            return builder.ToToken(JsPrecedence.FunctionCall);
        }

        public static JsToken Call(JsToken target, IEnumerable<JsToken> args)
        {
            JsTokenWriter builder = new JsTokenWriter();
            builder.WriteLeft(JsPrecedence.FunctionCall, target);
            builder.WriteArgs(args);
            return builder.ToToken(JsPrecedence.FunctionCall);
        }

        public static JsToken Name(string name)
        {
            return new JsToken(name);
        }

        public override string ToString()
        {
            return Text;
        }

        internal JsToken Member(string name)
        {
            return Member(this, name);
        }

        internal static JsToken Member(JsToken target, string name)
        {
            JsTokenWriter builder = new JsTokenWriter();
            builder.Write(JsPrecedence.Member, target);
            builder.Write('.');
            builder.Write(name);
            return builder.ToToken(JsPrecedence.Member);
        }

        internal static JsToken Assign(JsToken target, JsToken value)
        {
            JsTokenWriter builder = new JsTokenWriter();
            builder.WriteLeft(JsPrecedence.Assignement, target);
            builder.Write('=');
            builder.WriteRight(JsPrecedence.Assignement, value);
            return builder.ToToken(JsPrecedence.Assignement);
        }

        internal static JsToken Statement(String name)
        {
            return new JsToken(JsPrecedence.Statement, name);
        }

        internal static JsToken LiteralString(string strvalue)
        {
            JsTokenWriter builder = new JsTokenWriter();
            builder.WriteLiteralString(strvalue);
            return builder.ToToken(JsPrecedence.Name);
        }

        internal static JsToken Indexer(JsToken target, JsToken index)
        {
            JsTokenWriter writer = new JsTokenWriter();
            writer.WriteLeft(JsPrecedence.Member, target);
            writer.Write('[');
            writer.WriteLeft(JsPrecedence.Comma, index);
            writer.Write(']');
            return writer.ToToken(JsPrecedence.Member);
        }

        internal static JsToken CreateInstance(string name, IEnumerable<JsToken> parameters)
        {
            JsTokenWriter writer = new JsTokenWriter();
            writer.Write("new ");
            writer.Write(name);
            writer.WriteArgs(parameters);
            return writer.ToToken(JsPrecedence.New);
        }

        internal static JsToken CreateInstance(JsToken target, IEnumerable<JsToken> parameters)
        {
            JsTokenWriter writer = new JsTokenWriter();
            writer.Write("new ");
            writer.WriteLeft(JsPrecedence.FunctionCall, target);
            writer.WriteArgs(parameters);
            return writer.ToToken(JsPrecedence.New);
        }
    }
}
