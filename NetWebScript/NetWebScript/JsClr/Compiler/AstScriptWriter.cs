using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using NetWebScript.JsClr.AstBuilder.Cil;
using NetWebScript.JsClr.AstBuilder.PdbInfo;
using NetWebScript.JsClr.JsBuilder.JsSyntax;
using NetWebScript.JsClr.ScriptAst;
using NetWebScript.JsClr.TypeSystem;
using NetWebScript.JsClr.TypeSystem.Invoker;
using NetWebScript.Metadata;

namespace NetWebScript.JsClr.Compiler
{
    internal class AstScriptWriter : IScriptStatementVisitor<JsToken>, IRootInvoker
    {
        private readonly MethodBaseMetadata methodMetadata;
        private readonly MethodScriptAst method;
        private readonly bool isctor;
        private readonly bool pretty;
        private readonly Instrumentation instrumentation;

        public AstScriptWriter()
        {
        }

        public AstScriptWriter(MethodScriptAst method, MethodBaseMetadata methodMetadata, bool pretty, Instrumentation instrumentation)
        {
            this.pretty = pretty;
            if (instrumentation != null && methodMetadata != null && !Attribute.IsDefined(method.Method, typeof(DebuggerHiddenAttribute)))
            {
                this.methodMetadata = methodMetadata;
                this.instrumentation = instrumentation;
            }
            this.method = method;
            isctor = method.Method is ConstructorInfo && !method.Method.IsStatic;
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
            if (instrumentation != null && instrumentation.CaptureMethodContext)
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
            if (instrumentation != null && instrumentation.CaptureMethodContext)
            {
                return "t." + ArgumentName(variable);
            }
            return ArgumentName(variable);
        }

        public JsToken Visit(ScriptArgumentReferenceExpression node)
        {
            return JsToken.Name(ArgumentReference(node.Argument));
        }

        public JsToken Visit(ScriptArrayCreationExpression arrayCreationExpression)
        {
            JsTokenWriter writer = new JsTokenWriter();
            if (arrayCreationExpression.Initialize != null)
            {
                ScriptLiteralExpression literal = arrayCreationExpression.Size as ScriptLiteralExpression;
                if (literal == null || (int)literal.Value != arrayCreationExpression.Initialize.Count)
                {
                    throw new InvalidOperationException();
                }
                writer.Write("[");
                bool first = true;
                foreach (ScriptExpression expr in arrayCreationExpression.Initialize)
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

        public JsToken Visit(ScriptArrayIndexerExpression arrayIndexerExpression)
        {
            return JsToken.Indexer(
                arrayIndexerExpression.Target.Accept(this), 
                arrayIndexerExpression.Index.Accept(this));
        }

        public JsToken Visit(ScriptAssignExpression assignExpression)
        {
            return JsToken.Assign(
                assignExpression.Target.Accept(this), 
                assignExpression.Value.Accept(this));
        }

        public JsToken Visit(ScriptBinaryExpression binaryExpression)
        {
            return JsToken.Combine(
                binaryExpression.Left.Accept(this), 
                binaryExpression.Operator, 
                binaryExpression.Right.Accept(this));
        }

        public JsToken Visit(ScriptBreakStatement breakStatement)
        {
            return JsToken.Statement("break");
        }

        public JsToken Visit(ScriptContinueStatement continueStatement)
        {
            return JsToken.Statement("continue");
        }

        public JsToken Visit(ScriptFieldReferenceExpression fieldReferenceExpression)
        {
            var field = fieldReferenceExpression.Field;

            return field.Invoker.WriteField(field, fieldReferenceExpression, this);
        }

        public JsToken Visit(ScriptIfStatement ifStatement)
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

        public JsToken Visit(ScriptTryCatchStatement tryCatchStatement)
        {
            JsTokenWriter writer = new JsTokenWriter();
            writer.WriteLine("try");
            writer.WriteInBlock(pretty, tryCatchStatement.Body.Select(s => s.Accept(this)));
            if (tryCatchStatement.Catch != null)
            {
                writer.WriteLine("catch($e)");
                writer.WriteInBlock(pretty, tryCatchStatement.Catch.Select(s => s.Accept(this)));
            }
            if (tryCatchStatement.Finally != null)
            {
                writer.WriteLine("finally");
                writer.WriteInBlock(pretty, tryCatchStatement.Finally.Select(s => s.Accept(this)));
            }
            return writer.ToFullStatement();
        }

        public JsToken Visit(ScriptLiteralExpression literalExpression)
        {
            if (literalExpression.Value == null)
            {
                return JsToken.Name("null");
            }
            if (literalExpression.Type == null || literalExpression.Type.Serializer == null)
            {
                throw new InvalidOperationException("Value => " + literalExpression.Value); // Error should have been rised by DependenciesFinder
            }
            return literalExpression.Type.Serializer.LiteralValue(literalExpression.Type, literalExpression.Value, this);
        }

        public JsToken Visit(ScriptMethodInvocationExpression methodInvocationExpression)
        {
            var method = methodInvocationExpression.Method;
            return method.Invoker.WriteMethod(method, methodInvocationExpression, this);
        }
        
        public JsToken Visit(ScriptObjectCreationExpression objectCreationExpression)
        {
            var ctor = objectCreationExpression.Constructor;
            return ctor.CreationInvoker.WriteObjectCreation(ctor, objectCreationExpression, this);
        }

        public JsToken Visit(ScriptReturnStatement returnStatement)
        {
            JsTokenWriter writer = new JsTokenWriter();
            if (returnStatement.Value == null)
            {
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
                writer.WriteCommaSeparated(returnStatement.Value.Accept(this));
            }
            return writer.ToToken(JsPrecedence.Statement);
        }

        public JsToken Visit(ScriptThrowStatement throwStatement)
        {
            JsTokenWriter writer = new JsTokenWriter();
            writer.Write("throw ");
            writer.WriteCommaSeparated(throwStatement.Value.Accept(this));
            return writer.ToToken(JsPrecedence.Statement);
        }

        public JsToken Visit(ScriptSwitchStatement switchStatement)
        {
            JsTokenWriter writer = new JsTokenWriter();
            writer.Write("switch(");
            writer.WriteCommaSeparated(switchStatement.Value.Accept(this));
            writer.WriteLine(")");
            writer.Write("{");
            foreach (ScriptCase @case in switchStatement.Cases)
            {
                writer.WriteLine();
                if (@case.Value == ScriptCase.DefaultCase)
                {
                    writer.WriteLine("default:");
                }
                else
                {
                    writer.Write("case ");
                    writer.Write(Visit(@case.Value).Text);
                    writer.WriteLine(":");
                }
                writer.WriteIndented(pretty, @case.Statements.Select(s => s.Accept(this)));
                writer.Write("break;");
            }
            writer.WriteLine("}");
            return writer.ToFullStatement();
        }

        public JsToken Visit(ScriptThisReferenceExpression thisReferenceExpression)
        {
            return JsToken.Name("this");
        }

        public JsToken Visit(ScriptUnaryExpression unaryExpression)
        {
            return JsToken.Combine(unaryExpression.Operand.Accept(this), unaryExpression.Operator);
        }

        public JsToken Visit(ScriptVariableReferenceExpression variableReferenceExpression)
        {
            return JsToken.Name(VariableReference(variableReferenceExpression.Variable));
        }

        public JsToken Visit(ScriptWhileStatement whileStatement)
        {
            JsTokenWriter writer = new JsTokenWriter();
            writer.Write("while(");
            writer.WriteCommaSeparated(whileStatement.Condition.Accept(this));
            writer.WriteLine(")");
            writer.WriteInBlock(pretty, whileStatement.Body.Select(s => s.Accept(this)));
            return writer.ToFullStatement();
        }

        public JsToken Visit(ScriptDoWhileStatement whileStatement)
        {
            JsTokenWriter writer = new JsTokenWriter();
            writer.WriteLine("do");
            writer.WriteInBlock(pretty, whileStatement.Body.Select(s => s.Accept(this)));
            writer.Write("while(");
            writer.WriteCommaSeparated(whileStatement.Condition.Accept(this));
            writer.WriteLine(")");
            return writer.ToFullStatement();
        }

        public JsToken Visit(ScriptConditionExpression conditionExpression)
        {
            return JsToken.Condition(
                conditionExpression.Condition.Accept(this), 
                conditionExpression.Then.Accept(this), 
                conditionExpression.Else.Accept(this));
        }

        internal void WriteBody(TextWriter targetwriter)
        {
            var ast = method;
            JsTokenWriter writer = new JsTokenWriter();
            writer.WriteLine("{");
            if (instrumentation != null)
            {
                if (instrumentation.CaptureMethodContext)
                {
                    writer.Write("var t={");
                    writer.Write("$static:{0}", methodMetadata.Type.Name);
                    if (!ast.Method.IsStatic)
                    {
                        writer.Write(",$this:this");
                    }
                    foreach (var arg in ast.Arguments)
                    {
                        writer.Write(",{0}:{0}", ArgumentName(arg));
                    }
                    writer.WriteLine("};");
                }
                else
                {
                    writer.WriteLine("var t=null;");
                }
                writer.WriteLine("{0}.{1}('{2}',t);", instrumentation.EnterMethod.Owner.TypeId, instrumentation.EnterMethod.ImplId, methodMetadata.Id);
                writer.WriteLine("try {");
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
            if (methodMetadata != null && instrumentation != null && instrumentation.CaptureMethodContext)
            {
                foreach (var v in ast.Variables)
                {
                    methodMetadata.Variables.Add(new VariableMetadata() { Name = VariableName(v), CName = v.Name, CompilerGenerated = v.IsCompilerGenerated });
                }
                foreach (var arg in ast.Arguments)
                {
                    methodMetadata.Variables.Add(new VariableMetadata() { Name = ArgumentName(arg), CName = arg.Name, CompilerGenerated = false });
                }
            }
            writer.WriteIndented(pretty, ast.Statements.Select(s => s.Accept(this)));
            if (isctor)
            {
                writer.WriteLine("return this;");
            }
            if (instrumentation != null)
            {
                writer.WriteLine("} finally {");
                writer.WriteLine("{0}.{1}();",  instrumentation.LeaveMethod.Owner.TypeId, instrumentation.LeaveMethod.ImplId);
                writer.WriteLine("}");
            }
            writer.Write("}");
            targetwriter.Write(writer.ToString());
        }

        public JsToken Visit(ScriptDebugPointExpression point)
        {
            if (instrumentation == null)
            {
                if (point.Value != null)
                {
                    return point.Value.Accept(this);
                }
                return null;
            }
            JsTokenWriter builder = new JsTokenWriter();
            builder.Write("{0}.{1}", instrumentation.Point.Owner.TypeId, instrumentation.Point.ImplId);
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

        public JsToken Visit(ScriptCurrentExceptionExpression currentExceptionExpression)
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
    }
}
