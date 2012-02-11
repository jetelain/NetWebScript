using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.JsClr.ScriptWriter.Declaration;
using NetWebScript.JsClr.ScriptAst;
using System.Diagnostics.Contracts;
using System.Reflection;

namespace NetWebScript.JsClr.TypeSystem.Remoting
{
    class TransparentMethod : IScriptMethodDeclaration
    {
        private readonly IScriptMethod method;
        private readonly MethodScriptAst ast;

        public TransparentMethod(TransparentType type, IScriptMethod method, IScriptMethod realProxyInvoke)
        {
            Contract.Requires(!method.IsStatic);
            Contract.Requires(method.ImplId != null || method.SlodId != null);
            this.method = method;

            ast = new MethodScriptAst();
            foreach (var parameter in method.Method.GetParameters())
            {
                ast.Arguments.Add(new ScriptArgument() { Index = parameter.Position, Name = parameter.Name });
            }
            // this.realProxy.Invoke("ImplId/SlotId",[arg0,arg1,arg3...]);
            var invokeExpression = new ScriptMethodInvocationExpression(true,
                    realProxyInvoke,
                    new ScriptFieldReferenceExpression(new ScriptThisReferenceExpression(), type.RealProxyField),
                    new List<ScriptExpression>() { 
                        ScriptLiteralExpression.StringLiteral(type.Proxied.TypeId),
                        ScriptLiteralExpression.StringLiteral(method.ImplId ?? method.SlodId),
                        new ScriptArrayCreationExpression(ScriptLiteralExpression.IntegerLiteral(ast.Arguments.Count))
                        {
                            Initialize = ast.Arguments.Select(a => (ScriptExpression)new ScriptArgumentReferenceExpression(a)).ToList()
                        }
                    });

            if (((MethodInfo)method.Method).ReturnType == typeof(void))
            {
                ast.Statements.Add(invokeExpression);
            }
            else
            {
                ast.Statements.Add(new ScriptReturnStatement(invokeExpression));
            }
        }

        public bool IsGlobal
        {
            get { return false; }
        }

        public string SlodId
        {
            get { return method.SlodId; }
        }

        public string ImplId
        {
            get { return method.ImplId; }
        }

        public bool HasNativeBody
        {
            get { return false; }
        }

        public string NativeBody
        {
            get { throw new InvalidOperationException(); }
        }

        public ScriptAst.MethodScriptAst Ast
        {
            get { return ast; }
        }

        public Metadata.MethodBaseMetadata Metadata
        {
            get { return null; }
        }

        public string PrettyName
        {
            get { return method.Method.ToString(); }
        }

        public bool IsStatic
        {
            get { return false; }
        }
    }
}
