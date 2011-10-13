﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection.Emit;

namespace NetWebScript.Test.AstBuilder.StatementBuilder
{
    [TestClass]
    public class ExecOrder
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
        public void ExecOrder_HighStack()
        {
            var tester = new SBTester();
            var il = tester.Il;
            il.Emit(OpCodes.Call, new Func<int>(A).Method);
            il.Emit(OpCodes.Call, new Func<int>(B).Method);
            il.Emit(OpCodes.Call, new Func<int>(C).Method);
            il.Emit(OpCodes.Call, new Func<int>(D).Method);
            il.Emit(OpCodes.Call, new Action(E).Method);
            // Stack = A, B, C, D
            il.Emit(OpCodes.Mul);
            // Stack =  A, B, (C * D)
            il.Emit(OpCodes.Add);
            // Stack =  A, (B + (C * D))
            il.Emit(OpCodes.Sub); 
            // Stack = (A - (B + (C * D))) 
            il.Emit(OpCodes.Ret);
            var ast = tester.GetMethodAst();
            Assert.AreEqual(6, ast.Statements.Count);

            var s = ast.Statements[0];
            Assert.AreEqual("t3 = ExecOrder.A()", s.ToString());
            
            s = ast.Statements[1];
            Assert.AreEqual("t2 = ExecOrder.B()", s.ToString());
            
            s = ast.Statements[2];
            Assert.AreEqual("t1 = ExecOrder.C()", s.ToString());
            
            s = ast.Statements[3];
            Assert.AreEqual("t0 = ExecOrder.D()", s.ToString());
            
            s = ast.Statements[4];
            Assert.AreEqual("ExecOrder.E()", s.ToString());
            
            s = ast.Statements[5];
            Assert.AreEqual("return (t3 Subtract (t2 Add (t1 Multiply t0)))", s.ToString());

            // TODO: Check IL offsets
        }

        [TestMethod]
        public void ExecOrder_Dup()
        {
            var tester = new SBTester();
            var il = tester.Il;
            il.Emit(OpCodes.Call, new Func<int>(A).Method);
            il.Emit(OpCodes.Dup);
            il.Emit(OpCodes.Call, new Action(E).Method);
            il.Emit(OpCodes.Mul);
            il.Emit(OpCodes.Ret);
            var ast = tester.GetMethodAst();
            Assert.AreEqual(3, ast.Statements.Count);

            var s = ast.Statements[0];
            Assert.AreEqual("t0 = ExecOrder.A()", s.ToString());

            s = ast.Statements[1];
            Assert.AreEqual("ExecOrder.E()", s.ToString());

            s = ast.Statements[2];
            Assert.AreEqual("return (t0 Multiply t0)", s.ToString());

            // TODO: Check IL offsets
        }

        [TestMethod]
        public void ExecOrder_Dup_HighStack()
        {
            var tester = new SBTester();
            var il = tester.Il;
            il.Emit(OpCodes.Call, new Func<int>(A).Method);
            il.Emit(OpCodes.Dup); // A, A
            il.Emit(OpCodes.Dup); // A, A, A
            il.Emit(OpCodes.Call, new Func<int>(B).Method);
            il.Emit(OpCodes.Dup); // A, A, A, B, B
            il.Emit(OpCodes.Call, new Action(E).Method);
            il.Emit(OpCodes.Mul); // A, A, A, B * B
            il.Emit(OpCodes.Add); // A, A, A + (B * B)
            il.Emit(OpCodes.Sub); // A, A - (A + (B * B))
            il.Emit(OpCodes.Div); // A / (A - (A + (B * B)))
            il.Emit(OpCodes.Ret);
            var ast = tester.GetMethodAst();
            Assert.AreEqual(4, ast.Statements.Count);

            var s = ast.Statements[0];
            Assert.AreEqual("t1 = ExecOrder.A()", s.ToString());

            s = ast.Statements[1];
            Assert.AreEqual("t0 = ExecOrder.B()", s.ToString());

            s = ast.Statements[2];
            Assert.AreEqual("ExecOrder.E()", s.ToString());

            s = ast.Statements[3];
            Assert.AreEqual("return (t1 Divide (t1 Subtract (t1 Add (t0 Multiply t0))))", s.ToString());

            // TODO: Check IL offsets
        }
    }
}
