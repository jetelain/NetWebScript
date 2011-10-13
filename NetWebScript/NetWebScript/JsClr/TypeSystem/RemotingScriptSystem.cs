using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.JsClr.TypeSystem;
using NetWebScript.JsClr.AstBuilder;
using System.Reflection;
using NetWebScript.JsClr.Ast;
using NetWebScript.Remoting;

namespace NetWebScript.Remoting
{
    public class RemotingScriptSystem : ScriptSystem
    {
        public RemotingScriptSystem(string moduleId) :base(moduleId)
        {
        }

        public override MethodAst GetMethodAst(IScriptMethodBase scriptMethod, MethodBase methodBase)
        {
            if (methodBase is MethodInfo)
            {
                if (Attribute.IsDefined(methodBase, typeof(ServerSideAttribute)))
                {
                    var ast = new MethodAst(methodBase);
                    var args = new List<Expression>();
                    foreach(var arg in ast.Arguments )
                    {
                        args.Add(new ArgumentReferenceExpression(null,arg));

                        // Ensure that argument is serializable
                        GetScriptType(arg.ParameterType);
                    }
                    var count = new LiteralExpression(args.Count);
                    count.SetExpressionType(typeof(int));

                    ast.Statements = new List<Statement>();
                    var call = new MethodInvocationExpression(null, false, new Func<string,string,object,object[],object>(RemoteInvoker.Invoke).Method, null, new List<Expression>() {
                        new LiteralExpression(scriptMethod.Owner.TypeId),
                        new LiteralExpression(scriptMethod.ImplId),
                        methodBase.IsStatic ? (Expression)new LiteralExpression(null) : (Expression)new ThisReferenceExpression(null, methodBase.DeclaringType),
                        new ArrayCreationExpression(null,typeof(object),count) { Initialize = args }
                    });
                    if (ast.IsVoidMethod())
                    {
                        ast.Statements = new List<Statement>(){call};
                    }
                    else
                    {
                        ast.Statements = new List<Statement>(){new ReturnStatement() { Value = call }};
                    }
                    return ast;
                }
            }
            return base.GetMethodAst(scriptMethod, methodBase);
        }
    }
}
