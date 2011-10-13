using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection.Emit;
using NetWebScript.JsClr.Ast;

namespace NetWebScript.Test.AstBuilder.StatementBuilder
{
    [TestClass]
    public class Comparaison
    {
        [TestMethod]
        public void Comparaison_Equal()
        {
            var tester = new SBTester();
            var il = tester.Il;
            il.Emit(OpCodes.Ldc_I4, 31);
            il.Emit(OpCodes.Ldc_I4_1);
            il.Emit(OpCodes.Ceq);
            il.Emit(OpCodes.Ret);
            var expr = tester.GetExpression();

            Assert.AreEqual("(31 ValueEquality 1)", expr.ToString());

            var add = (BinaryExpression)expr;
            Assert.AreEqual(BinaryOperator.ValueEquality, add.Operator);
            Assert.AreEqual(6, add.IlOffset);
            Assert.AreEqual(0, add.FirstIlOffset);

            var ld = (LiteralExpression)add.Left;
            Assert.AreEqual(31, (int)ld.Value);
            Assert.AreEqual(0, ld.IlOffset);
            Assert.AreEqual(0, ld.FirstIlOffset);

            ld = (LiteralExpression)add.Right;
            Assert.AreEqual(1, (int)ld.Value);
            Assert.AreEqual(5, ld.IlOffset);
            Assert.AreEqual(5, ld.FirstIlOffset);
        }

        [TestMethod]
        public void Comparaison_GreaterThan()
        {
            var tester = new SBTester();
            var il = tester.Il;
            il.Emit(OpCodes.Ldc_I4, 31);
            il.Emit(OpCodes.Ldc_I4_1);
            il.Emit(OpCodes.Cgt);
            il.Emit(OpCodes.Ret);
            var expr = tester.GetExpression();

            Assert.AreEqual("(31 GreaterThan 1)", expr.ToString());

            var add = (BinaryExpression)expr;
            Assert.AreEqual(BinaryOperator.GreaterThan, add.Operator);
            Assert.AreEqual(6, add.IlOffset);
            Assert.AreEqual(0, add.FirstIlOffset);

            var ld = (LiteralExpression)add.Left;
            Assert.AreEqual(31, (int)ld.Value);
            Assert.AreEqual(0, ld.IlOffset);
            Assert.AreEqual(0, ld.FirstIlOffset);

            ld = (LiteralExpression)add.Right;
            Assert.AreEqual(1, (int)ld.Value);
            Assert.AreEqual(5, ld.IlOffset);
            Assert.AreEqual(5, ld.FirstIlOffset);
        }

        [TestMethod]
        public void Comparaison_LessThan()
        {
            var tester = new SBTester();
            var il = tester.Il;
            il.Emit(OpCodes.Ldc_I4, 31);
            il.Emit(OpCodes.Ldc_I4_1);
            il.Emit(OpCodes.Clt);
            il.Emit(OpCodes.Ret);
            var expr = tester.GetExpression();
            
            Assert.AreEqual("(31 LessThan 1)", expr.ToString());

            var add = (BinaryExpression)expr;
            Assert.AreEqual(BinaryOperator.LessThan, add.Operator);
            Assert.AreEqual(6, add.IlOffset);
            Assert.AreEqual(0, add.FirstIlOffset);

            var ld = (LiteralExpression)add.Left;
            Assert.AreEqual(31, (int)ld.Value);
            Assert.AreEqual(0, ld.IlOffset);
            Assert.AreEqual(0, ld.FirstIlOffset);

            ld = (LiteralExpression)add.Right;
            Assert.AreEqual(1, (int)ld.Value);
            Assert.AreEqual(5, ld.IlOffset);
            Assert.AreEqual(5, ld.FirstIlOffset);
        }
    }
}
