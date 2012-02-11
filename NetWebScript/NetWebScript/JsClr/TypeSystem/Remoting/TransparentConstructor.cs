using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.JsClr.ScriptWriter.Declaration;
using NetWebScript.JsClr.ScriptAst;
using NetWebScript.JsClr.TypeSystem.Invoker;

namespace NetWebScript.JsClr.TypeSystem.Remoting
{
    class TransparentConstructor : IScriptMethodBaseDeclaration, IInvocableConstructor
    {
        private readonly MethodScriptAst ast;
        private readonly TransparentType type;

        internal TransparentConstructor(TransparentType type)
        {
            this.type = type;

            ast = new MethodScriptAst();
            var arg = new ScriptArgument() { Index = 0, Name = "realProxy" };
            ast.Arguments.Add(arg);
            ast.Statements.Add(
                new ScriptAssignExpression(
                    new ScriptFieldReferenceExpression(new ScriptThisReferenceExpression(), type.RealProxyField), 
                    new ScriptArgumentReferenceExpression(arg)));
            ast.Statements.Add(new ScriptReturnStatement(new ScriptThisReferenceExpression()));
        }

        public string ImplId
        {
            get { return "$i"; }
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
            get { return string.Empty; }
        }

        public bool IsStatic
        {
            get { return false; }
        }

        public Invoker.IObjectCreationInvoker CreationInvoker
        {
            get { return StandardInvoker.Instance; }
        }


        public IInvocableType Owner
        {
            get { return type; }
        }

        public IMethodInvoker Invoker
        {
            get { return StandardInvoker.Instance; }
        }

        public string DisplayName
        {
            get { return ".ctor"; }
        }

        public bool IsVirtual
        {
            get { return false; }
        }
    }
}
