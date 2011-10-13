using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection.Emit;
using System.Reflection;
using NetWebScript.JsClr.AstBuilder;
using NetWebScript.JsClr.Ast;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NetWebScript.Test.AstBuilder.StatementBuilder
{
    public class SBTester
    {
        
        private readonly ILGenerator il;
        private readonly TypeBuilder type;

        public SBTester() : this(typeof(int))
        {
        }

        public SBTester(Type expressionType)
        {
            lock (Tester.Lock)
            {
                type = Tester.CreateStaticTestType();
                var method = type.DefineMethod("test", MethodAttributes.Static | MethodAttributes.Public, expressionType, Type.EmptyTypes);
                il = method.GetILGenerator();
            }
        }

        public ILGenerator Il
        {
            get { return il; }
        }

        public Expression GetExpression()
        {
            var ast = GetMethodAst();
            Assert.AreEqual(1, ast.Statements.Count);
            var ret = (ReturnStatement)ast.Statements[0];
            return ret.Value;
        }

        public MethodAst GetMethodAst()
        {
            MethodInfo m;
            lock (Tester.Lock)
            {
                m = type.CreateType().GetMethod("test");
            }
            return MethodAst.GetMethodAst(m);
        }

    }
}
