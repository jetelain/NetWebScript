using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using NetWebScript.JsClr.Ast;
using NetWebScript.JsClr.AstBuilder;
using NetWebScript.JsClr.AstBuilder.Cil;
using NetWebScript.JsClr.AstBuilder.PdbInfo;
using NetWebScript.JsClr.JsBuilder.JsSyntax;
using NetWebScript.JsClr.TypeSystem;
using NetWebScript.JsClr.TypeSystem.Invoker;
using NetWebScript.Metadata;

namespace NetWebScript.JsClr.Compiler
{
    internal class AstScriptWriter : IStatementVisitor<JsToken>, IRootInvoker
	{
        private readonly ScriptSystem system;
        private readonly MethodBaseMetadata methodMetadata;
        private readonly MethodAst method;
        private readonly bool isctor;
        private readonly bool pretty;

        public AstScriptWriter(ScriptSystem system, MethodAst method, MethodBaseMetadata methodMetadata, bool pretty)
		{
            this.system = system;
            this.pretty = pretty;
            if (!Attribute.IsDefined(method.Method, typeof(DebuggerHiddenAttribute)))
            {
                this.methodMetadata = methodMetadata;
            }
            this.method = method;
            isctor = method.Method is ConstructorInfo;
		}

        public bool IsDebug
        {
            get { return methodMetadata != null; }
        }

        internal string VariableName(LocalVariable variable)
        {
            if (variable.LocalIndex == -1)
            {
                return variable.Name;
            }
            return String.Format("v{0}", variable.LocalIndex);
        }

        internal string VariableReference(LocalVariable variable)
        {
            if (IsDebug)
            {
                return "t." + VariableName(variable);
            }
            return VariableName(variable);
        }

        internal string ArgumentName(ParameterInfo param)
        {
            return string.Format("a{0}", param.Position);
        }

        internal string ArgumentReference(ParameterInfo variable)
        {
            if (IsDebug)
            {
                return "t." + ArgumentName(variable);
            }
            return ArgumentName(variable);
        }

        public JsToken Visit(ArgumentReferenceExpression node)
        {
            return JsToken.Name(ArgumentReference(node.Argument));
        }

        public JsToken Visit(ArrayCreationExpression arrayCreationExpression)
        {
            JsTokenWriter writer = new JsTokenWriter();
            if (arrayCreationExpression.Initialize != null)
            {
                LiteralExpression literal = arrayCreationExpression.Size as LiteralExpression;
                if (literal == null || (int)literal.Value != arrayCreationExpression.Initialize.Count)
                {
                    throw new InvalidOperationException();
                }
                writer.Write("[");
                bool first = true;
                foreach (Expression expr in arrayCreationExpression.Initialize)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        writer.Write(',');
                    }
                    writer.WriteCommaSeparated(expr.Accept(this));
                }
                writer.Write(']');
            }
            else
            {

                writer.Write("new Array(");
                writer.WriteCommaSeparated(arrayCreationExpression.Size.Accept(this));
                writer.Write(')');
            }
            return writer.ToToken(JsPrecedence.FunctionCall);
        }

        public JsToken Visit(ArrayIndexerExpression arrayIndexerExpression)
        {
            return JsToken.Indexer(
                arrayIndexerExpression.Target.Accept(this), 
                arrayIndexerExpression.Index.Accept(this));
        }

        public JsToken Visit(AssignExpression assignExpression)
        {
            return JsToken.Assign(
                assignExpression.Target.Accept(this), 
                assignExpression.Value.Accept(this));
        }

        public JsToken Visit(BinaryExpression binaryExpression)
        {
            return JsToken.Combine(
                binaryExpression.Left.Accept(this), 
                binaryExpression.Operator, 
                binaryExpression.Right.Accept(this));
        }

        public JsToken Visit(BreakStatement breakStatement)
        {
            return JsToken.Statement("break");
        }

        public JsToken Visit(ContinueStatement continueStatement)
        {
            return JsToken.Statement("continue");
        }

        public JsToken Visit(FieldReferenceExpression fieldReferenceExpression)
        {
            var field = system.GetScriptField(fieldReferenceExpression.Field);
            if (field == null)
            {
                throw new InvalidOperationException(); // Error should have been rised by DependenciesFinder
            }
            return field.Invoker.WriteField(field, fieldReferenceExpression, this);
        }

        public JsToken Visit(IfStatement ifStatement)
        {
            JsTokenWriter writer = new JsTokenWriter();
            writer.Write("if(");
            writer.WriteCommaSeparated(ifStatement.Condition.Accept(this));
            writer.WriteLine(")");
            writer.WriteInBlock(pretty, ifStatement.Then.Select(s => s.Accept(this)));
            if (ifStatement.Else != null)
            {
                writer.WriteLine();
                writer.WriteLine("else");
                writer.WriteInBlock(pretty, ifStatement.Else.Select(s => s.Accept(this)));
            }
            return writer.ToFullStatement();
        }

        public JsToken Visit(TryCatchStatement tryCatchStatement)
        {
            JsTokenWriter writer = new JsTokenWriter();
            writer.WriteLine("try");
            writer.WriteInBlock(pretty, tryCatchStatement.Body.Select(s => s.Accept(this)));
            if (tryCatchStatement.CatchList != null)
            {
                if (tryCatchStatement.CatchList.Count > 1)
                {
                    writer.WriteLine("catch($e)");
                    writer.WriteLine("{}");
                }
                else
                {
                    Catch @catch = tryCatchStatement.CatchList[0];
                    writer.WriteLine("catch($e)");
                    writer.WriteInBlock(pretty, @catch.Body.Select(s => s.Accept(this)));
                }
            }
            if (tryCatchStatement.Finally != null)
            {
                writer.WriteLine("finally");
                writer.WriteInBlock(pretty, tryCatchStatement.Finally.Select(s => s.Accept(this)));
            }
            return writer.ToFullStatement();
        }

        public JsToken Visit(LiteralExpression literalExpression)
        {
            return Literal(literalExpression.GetExpressionType(), literalExpression.Value);
        }

        public JsToken Visit(MethodInvocationExpression methodInvocationExpression)
        {
            var method = system.GetScriptMethodBase(methodInvocationExpression.Method);
            if (method == null)
            {
                throw new InvalidOperationException(); // Error should have been rised by DependenciesFinder
            }
            return method.Invoker.WriteMethod(method, methodInvocationExpression, this);
        }
        
        public JsToken Visit(ObjectCreationExpression objectCreationExpression)
        {
            var ctor = system.GetScriptConstructor(objectCreationExpression.Constructor);
            if (ctor == null)
            {
                throw new InvalidOperationException(); // Error should have been rised by DependenciesFinder
            }
            return ctor.CreationInvoker.WriteObjectCreation(ctor, objectCreationExpression, this);
        }

        public JsToken Visit(ReturnStatement returnStatement)
        {
            JsTokenWriter writer = new JsTokenWriter();
            if (returnStatement.Value == null)
            {
                if (IsDebug)
                {
                   writer.Write("$dbgL();");
                }
                if (isctor)
                {
                    writer.Write("return this");
                }
                else
                {
                    writer.Write("return");
                }
            }
            else
            {
                writer.Write("return ");
                if (IsDebug)
                {
                    writer.Write("$dbgL(");
                    writer.WriteCommaSeparated(returnStatement.Value.Accept(this));
                    writer.Write(")");
                }
                else
                {
                    writer.WriteCommaSeparated(returnStatement.Value.Accept(this));
                }
            }
            return writer.ToToken(JsPrecedence.Statement);
        }

        public JsToken Visit(ThrowStatement throwStatement)
        {
            JsTokenWriter writer = new JsTokenWriter();
            writer.Write("throw ");
            if (IsDebug)
            {
                writer.Write("$dbgL(");
                writer.WriteCommaSeparated(throwStatement.Value.Accept(this));
                writer.Write(")");
            }
            else
            {
                writer.WriteCommaSeparated(throwStatement.Value.Accept(this));
            }
            return writer.ToToken(JsPrecedence.Statement);
        }

        public JsToken Visit(SwitchStatement switchStatement)
        {
            JsTokenWriter writer = new JsTokenWriter();
            writer.Write("switch(");
            writer.WriteCommaSeparated(switchStatement.Value.Accept(this));
            writer.WriteLine(")");
            writer.Write("{");
            foreach (Case @case in switchStatement.Cases)
            {
                writer.WriteLine();
                if (@case.Value == Case.DefaultCase)
                {
                    writer.WriteLine("default:");
                }
                else
                {
                    writer.Write("case ");
                    writer.Write(Literal(null,@case.Value).Text);
                    writer.WriteLine(":");
                }
                writer.WriteIndented(pretty, @case.Statements.Select(s => s.Accept(this)));
                writer.Write("break;");
            }
            writer.WriteLine("}");
            return writer.ToFullStatement();
        }

        public JsToken Visit(ThisReferenceExpression thisReferenceExpression)
        {
            return JsToken.Name("this");
        }

        public JsToken Visit(UnaryExpression unaryExpression)
        {
            return JsToken.Combine(unaryExpression.Operand.Accept(this), unaryExpression.Operator);
        }

        public JsToken Visit(VariableReferenceExpression variableReferenceExpression)
        {
            return JsToken.Name(VariableReference(variableReferenceExpression.Variable));
        }

        public JsToken Visit(WhileStatement whileStatement)
        {
            JsTokenWriter writer = new JsTokenWriter();
            writer.Write("while(");
            writer.WriteCommaSeparated(whileStatement.Condition.Accept(this));
            writer.WriteLine(")");
            writer.WriteInBlock(pretty, whileStatement.Body.Select(s => s.Accept(this)));
            return writer.ToFullStatement();
        }
        public JsToken Visit(DoWhileStatement whileStatement)
        {
            JsTokenWriter writer = new JsTokenWriter();
            writer.WriteLine("do");
            writer.WriteInBlock(pretty, whileStatement.Body.Select(s => s.Accept(this)));
            writer.Write("while(");
            writer.WriteCommaSeparated(whileStatement.Condition.Accept(this));
            writer.WriteLine(")");
            return writer.ToFullStatement();
        }
        public JsToken Visit(ConditionExpression conditionExpression)
        {
            return JsToken.Condition(
                conditionExpression.Condition.Accept(this), 
                conditionExpression.Then.Accept(this), 
                conditionExpression.Else.Accept(this));
        }

        #region RuntimeAstFilter club

        public JsToken Visit(CastExpression castExpression)
        {
            // Casting must have been transformed by RuntimeAstFilter
            throw new InvalidOperationException();
        }

        public JsToken Visit(SafeCastExpression castExpression)
        {
            // Casting must have been transformed by RuntimeAstFilter
            throw new InvalidOperationException();
        }

        public JsToken Visit(BoxExpression boxExpression)
        {
            // Boxing must have been transformed by RuntimeAstFilter
            throw new InvalidOperationException();
        }

        public JsToken Visit(UnboxExpression unboxExpression)
        {
            // Unboxing must have been transformed by RuntimeAstFilter
            throw new InvalidOperationException();
        }

        #endregion

        internal JsToken Write(Expression expression)
        {
            return expression.Accept(this);
        }

        internal JsToken Literal(Type type, object value)
        {
            if (value == null)
            {
                return JsToken.Name("null");
            }

            if ( type == null && value != null )
            {
                type = value.GetType();
            }

            var scriptType = system.GetScriptType(type);
            if (scriptType == null || scriptType.Serializer == null)
            {
                throw new InvalidOperationException(); // Error should have been rised by DependenciesFinder
            }
            return scriptType.Serializer.LiteralValue(scriptType, value, this);
        }

        internal void WriteBody(TextWriter targetwriter)
        {
            var ast = method;
            JsTokenWriter writer = new JsTokenWriter();
            writer.WriteLine("{");
            if (IsDebug)
            {
                writer.Write("var t={");
                bool first = true;
                if (!ast.Method.IsStatic)
                {
                    first = false;
                    writer.Write("$this:this");
                }
                foreach (var arg in ast.Arguments)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        writer.Write(',');
                    }
                    writer.Write("{0}:{0}", ArgumentName(arg));
                }
                writer.WriteLine("};");
                writer.WriteLine("$dbgE('{0}',t);", methodMetadata.Id);
            }
            else if (ast.Variables.Count > 0)
            {
                bool first = true;
                writer.Write("var ");
                foreach (LocalVariable v in ast.Variables)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        writer.Write(',');
                    }
                    writer.Write(VariableName(v));
                }
                writer.WriteLine(";");
            }
            if (methodMetadata != null)
            {
                foreach (var v in ast.Variables)
                {
                    methodMetadata.Variables.Add(new VariableMetadata() { Name = VariableName(v), CName = v.Name });
                }
                foreach (var arg in ast.Arguments)
                {
                    methodMetadata.Variables.Add(new VariableMetadata() { Name = ArgumentName(arg), CName = arg.Name });
                }
            }
            writer.WriteIndented(pretty, ast.Statements.Select(s => s.Accept(this)));
            if (IsDebug)
            {
                writer.WriteLine("$dbgL();");
            }
            if (isctor)
            {
                writer.WriteLine("return this;");
            }
            writer.Write("}");
            targetwriter.Write(writer.ToString());
        }

        public JsToken Visit(DebugPointExpression point)
        {
            if (!IsDebug)
            {
                if (point.Value != null)
                {
                    return point.Value.Accept(this);
                }
                return null;
            }
            JsTokenWriter builder = new JsTokenWriter();
            builder.Write("$dbgP");
            builder.WriteOpenArgs();
            builder.Write("'" + AddDebugPoint(point.Point).Id + "'");
            builder.WriteCloseArgs();
            if (point.Value != null)
            {
                builder.Write("?");
                builder.WriteRight(JsPrecedence.Conditional, point.Value.Accept(this));
                builder.Write(":''");
                return builder.ToToken(JsPrecedence.Conditional);
            }
            return builder.ToToken(JsPrecedence.FunctionCall); 
        }

        public String DebugPointCall(PdbSequencePoint point)
        {
            if (!IsDebug)
            {
                throw new InvalidOperationException();
            }
            JsTokenWriter builder = new JsTokenWriter();
            builder.Write("$dbgP");
            builder.WriteOpenArgs();
            builder.Write("'" + AddDebugPoint(point).Id + "'");
            builder.WriteCloseArgs();
            return builder.ToString();
        }

        public JsToken Visit(CurrentExceptionExpression currentExceptionExpression)
        {
            return JsToken.Name("$e");
        }
        private DebugPointMetadata AddDebugPoint(PdbSequencePoint point)
        {
            var meta = new DebugPointMetadata();
            meta.Method = methodMetadata;
            meta.DocumentId = methodMetadata.Type.Module.GetDocumentReference(point.Filename).Id;
            meta.Name = String.Format("{0}", methodMetadata.Points.Count + 1);
            meta.Offset = point.Offset;
            meta.StartCol = point.StartCol;
            meta.StartRow = point.StartRow;
            meta.EndCol = point.EndCol;
            meta.EndRow = point.EndRow;
            methodMetadata.Points.Add(meta);
            return meta;
        }

        public JsToken Visit(MakeByRefFieldExpression refExpression)
        {
            throw new NotImplementedException();
        }

        public JsToken Visit(ByRefSetExpression byRefSetExpression)
        {
            throw new NotImplementedException();
        }

        public JsToken Visit(MakeByRefVariableExpression makeByRefVariableExpression)
        {
            return JsToken.Name(VariableReference(makeByRefVariableExpression.Variable));
        }


        public JsToken Visit(ByRefGetExpression byRefGetExpression)
        {
            throw new NotImplementedException();
        }

        public JsToken Visit(MakeByRefArgumentExpression makeByRefArgumentExpression)
        {
            throw new NotImplementedException();
        }
    }
}
