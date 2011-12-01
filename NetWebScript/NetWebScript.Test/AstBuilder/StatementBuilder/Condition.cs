﻿using System;
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
        public static void Show2(int a, int b)
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
            Assert.AreEqual("t0 = Condition.A()", ast.Statements[0].ToString());
            Assert.AreEqual("t1 = Condition.B()", ast.Statements[1].ToString());
            Assert.AreEqual(@"if ( (Condition.C() ValueInequality 2) )
{
	t1;
	t1 = Condition.D();
}", ast.Statements[2].ToString());
            Assert.AreEqual("Condition.Show2(t0,t1)", ast.Statements[3].ToString());
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
