using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection.Emit;

namespace NetWebScript.Test.AstBuilder.StatementBuilder
{
    [TestClass]
    public class Dup
    {
        public static int A()
        {
            return 1;
        }
        public static int B()
        {
            return 2;
        }
        public static int C()
        {
            return 3;
        }
        public static int D()
        {
            return 4;
        }
        public static void E()
        {
        }

        [TestMethod]
        public void Dup_Assignement()
        {
            var tester = new SBTester(typeof(void));
            var il = tester.Il;
            il.Emit(OpCodes.Call, new Func<int>(A).Method);
            il.Emit(OpCodes.Dup);
            il.Emit(OpCodes.Stloc, il.DeclareLocal(typeof(int)));
            il.Emit(OpCodes.Stloc, il.DeclareLocal(typeof(int)));
            il.Emit(OpCodes.Ret);
            var ast = tester.GetMethodAst();
            Assert.AreEqual(1, ast.Statements.Count);
            var s = ast.Statements[0];
            Assert.AreEqual("v1 = v0 = Dup.A()", s.ToString());
        }

        [TestMethod]
        public void Dup_Assignement_Return()
        {
            var tester = new SBTester();
            var il = tester.Il;
            il.Emit(OpCodes.Call, new Func<int>(A).Method);
            il.Emit(OpCodes.Dup);
            il.Emit(OpCodes.Dup);
            il.Emit(OpCodes.Stloc, il.DeclareLocal(typeof(int)));
            il.Emit(OpCodes.Stloc, il.DeclareLocal(typeof(int)));
            il.Emit(OpCodes.Ret);
            var s = tester.GetExpression();
            Assert.AreEqual("v1 = v0 = Dup.A()", s.ToString());
        }
        [TestMethod]
        public void Dup_BinaryOperation()
        {
            var tester = new SBTester(typeof(int));
            var il = tester.Il;
            il.Emit(OpCodes.Call, new Func<int>(A).Method);
            il.Emit(OpCodes.Dup);
            il.Emit(OpCodes.Add);
            il.Emit(OpCodes.Ret);
            var expr = tester.GetExpression();
            Assert.AreEqual("((v0 = Dup.A()) Add (v0))", expr.ToString());
        }

        [TestMethod]
        public void Dup_BinaryOperation_NestedOnce()
        {
            var tester = new SBTester(typeof(int));
            var il = tester.Il;
            il.Emit(OpCodes.Call, new Func<int>(A).Method);
            il.Emit(OpCodes.Dup);
            il.Emit(OpCodes.Dup); // A, A, A
            il.Emit(OpCodes.Add); // A, A + A
            il.Emit(OpCodes.Sub); // A - (A + A)
            il.Emit(OpCodes.Ret);
            var expr = tester.GetExpression();
            Assert.AreEqual("((v0 = Dup.A()) Subtract ((v0 Add v0)))", expr.ToString());
        }

        [TestMethod]
        public void Dup_BinaryOperation_Nested()
        {
            var tester = new SBTester(typeof(int));
            var il = tester.Il;
            il.Emit(OpCodes.Call, new Func<int>(A).Method);
            il.Emit(OpCodes.Dup);
            il.Emit(OpCodes.Dup);
            il.Emit(OpCodes.Dup); // A, A, A, A
            il.Emit(OpCodes.Add); // A, A, A + A
            il.Emit(OpCodes.Sub); // A, A - (A + A)
            il.Emit(OpCodes.Mul); // A * (A - (A + A))
            il.Emit(OpCodes.Ret);
            var expr = tester.GetExpression();
            Assert.AreEqual("((v0 = Dup.A()) Multiply ((v0 Subtract (v0 Add v0))))", expr.ToString());
        }
    }
}
