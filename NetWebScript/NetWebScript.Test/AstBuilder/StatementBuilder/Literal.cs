using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetWebScript.JsClr.Ast;
using System.Reflection.Emit;

namespace NetWebScript.Test.AstBuilder.StatementBuilder
{
    [TestClass]
    public class Literal
    {
        private void TestOpCode(OpCode code, int value)
        {
            var tester = new SBTester();
            var il = tester.Il;
            il.Emit(code);
            il.Emit(OpCodes.Ret);
            var expr = tester.GetExpression();

            var l = (LiteralExpression)expr;
            Assert.AreEqual(value, (int)l.Value);
            Assert.AreEqual(0, l.IlOffset);
            Assert.AreEqual(typeof(int), l.GetExpressionType());
        }

        [TestMethod]
        public void Literal_Ldc_I4()
        {
            var tester = new SBTester();
            var il = tester.Il;
            il.Emit(OpCodes.Ldc_I4, 31);
            il.Emit(OpCodes.Ret);
            var expr = tester.GetExpression();

            var l = (LiteralExpression)expr;
            Assert.AreEqual(31, (int)l.Value);
            Assert.AreEqual(0, l.IlOffset);
            Assert.AreEqual(typeof(int), l.GetExpressionType());
        }

        [TestMethod]
        public void Literal_Ldc_I4_S()
        {
            var tester = new SBTester();
            var il = tester.Il;
            il.Emit(OpCodes.Ldc_I4_S, 31);
            il.Emit(OpCodes.Ret);
            var expr = tester.GetExpression();

            var l = (LiteralExpression)expr;
            Assert.AreEqual(31, (int)l.Value);
            Assert.AreEqual(0, l.IlOffset);
            Assert.AreEqual(typeof(int), l.GetExpressionType());
        }

        [TestMethod]
        public void Literal_Ldc_I4_X()
        {
            TestOpCode(OpCodes.Ldc_I4_0, 0);
            TestOpCode(OpCodes.Ldc_I4_1, 1);
            TestOpCode(OpCodes.Ldc_I4_2, 2);
            TestOpCode(OpCodes.Ldc_I4_3, 3);
            TestOpCode(OpCodes.Ldc_I4_4, 4);
            TestOpCode(OpCodes.Ldc_I4_5, 5);
            TestOpCode(OpCodes.Ldc_I4_6, 6);
            TestOpCode(OpCodes.Ldc_I4_7, 7);
            TestOpCode(OpCodes.Ldc_I4_8, 8);
            TestOpCode(OpCodes.Ldc_I4_M1, -1);
        }

        [TestMethod]
        public void Literal_Ldnull()
        {
            var tester = new SBTester(typeof(object));
            var il = tester.Il;
            il.Emit(OpCodes.Ldnull);
            il.Emit(OpCodes.Ret);
            var expr = tester.GetExpression();

            var l = (LiteralExpression)expr;
            Assert.AreEqual(null, l.Value);
            Assert.AreEqual(0, l.IlOffset);
            Assert.AreEqual(typeof(object), l.GetExpressionType());
        }

        [TestMethod]
        public void Literal_Ldstr()
        {
            var tester = new SBTester(typeof(string));
            var il = tester.Il;
            il.Emit(OpCodes.Ldstr, "Hello world!");
            il.Emit(OpCodes.Ret);
            var expr = tester.GetExpression();

            var l = (LiteralExpression)expr;
            Assert.AreEqual("Hello world!", (string)l.Value);
            Assert.AreEqual(0, l.IlOffset);
            Assert.AreEqual(typeof(string), l.GetExpressionType());
        }

        [TestMethod]
        public void Literal_Ldc_I8()
        {
            var tester = new SBTester(typeof(long));
            var il = tester.Il;
            il.Emit(OpCodes.Ldc_I8, 12345L);
            il.Emit(OpCodes.Ret);
            var expr = tester.GetExpression();

            var l = (LiteralExpression)expr;
            Assert.AreEqual(12345L, (long)l.Value);
            Assert.AreEqual(0, l.IlOffset);
            Assert.AreEqual(typeof(long), l.GetExpressionType());
        }

        [TestMethod]
        public void Literal_Ldc_R4()
        {
            var tester = new SBTester(typeof(float));
            var il = tester.Il;
            il.Emit(OpCodes.Ldc_R4, 1.2345F);
            il.Emit(OpCodes.Ret);
            var expr = tester.GetExpression();

            var l = (LiteralExpression)expr;
            Assert.AreEqual(1.2345F, (float)l.Value);
            Assert.AreEqual(0, l.IlOffset);
            Assert.AreEqual(typeof(float), l.GetExpressionType());
        }

        [TestMethod]
        public void Literal_Ldc_R8()
        {
            var tester = new SBTester(typeof(double));
            var il = tester.Il;
            il.Emit(OpCodes.Ldc_R8, 1.2345);
            il.Emit(OpCodes.Ret);
            var expr = tester.GetExpression();

            var l = (LiteralExpression)expr;
            Assert.AreEqual(1.2345, (double)l.Value);
            Assert.AreEqual(0, l.IlOffset);
            Assert.AreEqual(typeof(double), l.GetExpressionType());
        }
    }
}
