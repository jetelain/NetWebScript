using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection.Emit;

namespace NetWebScript.Test.AstBuilder.StatementBuilder
{
    [TestClass]
    public class Condition
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

        public static int E()
        {
            return 5;
        }

        public static int F()
        {
            return 6;
        }

        public static int G()
        {
            return 7;
        }
        public static void Show2(int a, int b)
        {
        }
        public static void Show4(int a, int b, int c, int d)
        {
        }
        [TestMethod]
        public void Condition_ConsumeStack()
        {
            var tester = new SBTester(typeof(void));
            var il = tester.Il;
            il.Emit(OpCodes.Call, new Func<int>(A).Method);
            il.Emit(OpCodes.Call, new Func<int>(B).Method);
            il.Emit(OpCodes.Call, new Func<int>(C).Method);
            il.Emit(OpCodes.Ldc_I4_2);
            var lbl = il.DefineLabel();
            il.Emit(OpCodes.Beq, lbl);
            il.Emit(OpCodes.Pop);
            il.Emit(OpCodes.Call, new Func<int>(D).Method);
            il.MarkLabel(lbl);
            il.Emit(OpCodes.Call, new Action<int,int>(Show2).Method);
            il.Emit(OpCodes.Ret);

            var ast = tester.GetMethodAst();

            Assert.AreEqual(4, ast.Statements.Count);
            Assert.AreEqual("v0 = Condition.A()", ast.Statements[0].ToString());
            Assert.AreEqual("v1 = Condition.B()", ast.Statements[1].ToString());
            Assert.AreEqual(@"if ( (Condition.C() ValueInequality 2) )
{
	v1 = Condition.D();
}", ast.Statements[2].ToString());
            Assert.AreEqual("Condition.Show2(v0,v1)", ast.Statements[3].ToString());
        }

        [TestMethod]
        public void Condition_ConsumeStack2()
        {
            var tester = new SBTester(typeof(void));
            var il = tester.Il;
            il.Emit(OpCodes.Call, new Func<int>(A).Method);
            il.Emit(OpCodes.Call, new Func<int>(B).Method);
            il.Emit(OpCodes.Call, new Func<int>(C).Method);
            il.Emit(OpCodes.Call, new Func<int>(D).Method);
            il.Emit(OpCodes.Call, new Func<int>(E).Method);
            il.Emit(OpCodes.Ldc_I4_2);
            var lbl = il.DefineLabel();
            il.Emit(OpCodes.Beq, lbl);
            il.Emit(OpCodes.Pop);
            il.Emit(OpCodes.Pop);
            il.Emit(OpCodes.Call, new Func<int>(F).Method);
            il.Emit(OpCodes.Call, new Func<int>(G).Method);
            il.MarkLabel(lbl);
            il.Emit(OpCodes.Call, new Action<int, int, int, int>(Show4).Method);
            il.Emit(OpCodes.Ret);

            var ast = tester.GetMethodAst();

            Assert.AreEqual(6, ast.Statements.Count);
            Assert.AreEqual("v0 = Condition.A()", ast.Statements[0].ToString());
            Assert.AreEqual("v1 = Condition.B()", ast.Statements[1].ToString());
            Assert.AreEqual("v2 = Condition.C()", ast.Statements[2].ToString());
            Assert.AreEqual("v3 = Condition.D()", ast.Statements[3].ToString());
            Assert.AreEqual(@"if ( (Condition.E() ValueInequality 2) )
{
	v2 = Condition.F();
	v3 = Condition.G();
}", ast.Statements[4].ToString());
            Assert.AreEqual("Condition.Show4(v0,v1,v2,v3)", ast.Statements[5].ToString());
        }

        [TestMethod]
        public void Condition_Ternary()
        {
            var tester = new SBTester(typeof(void));
            var il = tester.Il;
            il.Emit(OpCodes.Call, new Func<int>(A).Method);
            var case2 = il.DefineLabel();
            var end = il.DefineLabel();
            il.Emit(OpCodes.Ldc_I4_2);
            il.Emit(OpCodes.Beq, case2);
            il.Emit(OpCodes.Call, new Func<int>(B).Method);
            il.Emit(OpCodes.Br, end);
            il.MarkLabel(case2);
            il.Emit(OpCodes.Call, new Func<int>(C).Method);
            il.MarkLabel(end);
            il.Emit(OpCodes.Stloc, il.DeclareLocal(typeof(int)));
            il.Emit(OpCodes.Ret);

            var ast = tester.GetMethodAst();

            Assert.AreEqual(1, ast.Statements.Count);
            Assert.AreEqual("v0 = ((Condition.A() ValueEquality 2)?Condition.C():Condition.B())", ast.Statements[0].ToString());
        }

    }
}
