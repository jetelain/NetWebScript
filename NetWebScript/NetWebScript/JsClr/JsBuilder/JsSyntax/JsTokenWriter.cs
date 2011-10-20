using System.Collections.Generic;
using System.IO;

namespace NetWebScript.JsClr.JsBuilder.JsSyntax
{
    public class JsTokenWriter : StringWriter
    {
        internal void WriteTarget(JsToken token)
        {
            WriteLeft(JsPrecedence.Member, token);
        }

        internal void WriteCommaSeparated(JsToken token)
        {
            WriteLeft(JsPrecedence.Comma, token);
        }

        // Left-associativity (left-to-right) means that it is processed as (a OP b) OP c, 
        // Right-associativity (right-to-left) means it is interpreted as a OP (b OP c). 

        public void WriteRight(JsPrecedence precedence, JsToken token)
        {
            int diff = (int)(precedence & JsPrecedence.PrecedenceMask) - (int)(token.Precedence & JsPrecedence.PrecedenceMask);
            if (diff < 0 || (diff == 0 && (precedence & JsPrecedence.AssocMask) != JsPrecedence.RightToLeft) )
            {
                Write('(');
                Write(token.Text);
                Write(')');
            }
            else
            {
                Write(token.Text);
            }
        }

        public void WriteLeft(JsPrecedence precedence, JsToken token)
        {
            int diff = (int)(precedence & JsPrecedence.PrecedenceMask) - (int)(token.Precedence & JsPrecedence.PrecedenceMask);
            if (diff < 0 || (diff == 0 && (precedence & JsPrecedence.AssocMask) != JsPrecedence.LeftToRight))
            {
                Write('(');
                Write(token.Text);
                Write(')');
            }
            else
            {
                Write(token.Text);
            }
        }

        public void Write ( JsPrecedence precedence, JsToken token )
        {
            if ((precedence & JsPrecedence.PrecedenceMask) <= (token.Precedence & JsPrecedence.PrecedenceMask))
            {
                Write('(');
                Write(token.Text);
                Write(')');
            }
            else
            {
                Write(token.Text);
            }
        }

        private Stack<bool> argsStack;

        public void WriteOpenArgs()
        {
            if ( argsStack == null )
            {
                argsStack = new Stack<bool>();
            }
            argsStack.Push(true);
            Write('(');
        }

        public void WriteArg(JsToken token)
        {
            if (argsStack.Peek())
            {
                argsStack.Pop();
                argsStack.Push(false);
            }
            else
            {
                Write(',');
            }
            Write(JsPrecedence.Comma,token);
        }

        public void WriteCloseArgs()
        {
            argsStack.Pop();
            Write(')');
        }

        public void WriteArgs(IEnumerable<JsToken> args)
        {
            WriteOpenArgs();
            foreach (JsToken arg in args)
            {
                WriteArg(arg);
            }
            WriteCloseArgs();
        }

        public JsToken ToToken(JsPrecedence precedence)
        {
            return new JsToken(precedence, ToString());
        }

        public JsToken ToStatement()
        {
            return new JsToken(JsPrecedence.Statement, ToString());
        }

        public JsToken ToFullStatement()
        {
            return new JsToken(JsPrecedence.FullStatement, ToString());
        }

        internal void WriteInBlock(bool pretty, IEnumerable<JsToken> statements)
        {
            WriteLine('{');
            WriteIndented(pretty, statements);
            Write('}');
        }

        internal void WriteIndented(bool pretty, IEnumerable<JsToken> statements)
        {
            foreach (JsToken token in statements)
            {
                if (token != null)
                {
                    if ( pretty ) Write('\t');
                    if (token.Precedence == JsPrecedence.FullStatement)
                    {
                        if (pretty)
                        {
                            WriteLine(token.Text.Replace("\n", "\n\t"));
                        }
                        else
                        {
                            WriteLine(token.Text);
                        }
                    }
                    else
                    {
                        Write(token.Text);
                        WriteLine(';');
                    }
                }
            }
        }

        internal void WriteLiteralString(string value)
        {
            Write('"');
            Write(value.Replace("\\", "\\\\").Replace("\n", "\\n").Replace("\r", "\\r").Replace("\"", "\\\"").Replace("'", "\\'"));
            Write('"');
        }
    }
}
