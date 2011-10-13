using System.Reflection.Emit;
using NetWebScript.JsClr.Ast;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NetWebScript.Test.AstBuilder.StatementBuilder
{
    /// <summary>
    /// Units tests on arithmetic OpCodes transposition
    /// </summary>
    [TestClass]
    public class Arithmetic
    {
        [TestMethod]
        public void Arithmetic_Add()
        {
            var tester = new SBTester();
            var il = tester.Il;
            il.Emit(OpCodes.Ldc_I4, 31);
            il.Emit(OpCodes.Ldc_I4_1);
            il.Emit(OpCodes.Add);
            il.Emit(OpCodes.Ret);
            var expr = tester.GetExpression();

            Assert.AreEqual("(31 Add 1)", expr.ToString());

            var add = (BinaryExpression)expr;
            Assert.AreEqual(BinaryOperator.Add, add.Operator);
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
        public void Arithmetic_Subtract()
        {
            var tester = new SBTester();
            var il = tester.Il;
            il.Emit(OpCodes.Ldc_I4, 31);
            il.Emit(OpCodes.Ldc_I4_2);
            il.Emit(OpCodes.Sub);
            il.Emit(OpCodes.Ret);
            var expr = tester.GetExpression();

            Assert.AreEqual("(31 Subtract 2)", expr.ToString());

            var add = (BinaryExpression)expr;
            Assert.AreEqual(BinaryOperator.Subtract, add.Operator);
            Assert.AreEqual(6, add.IlOffset);
            Assert.AreEqual(0, add.FirstIlOffset);

            var ld = (LiteralExpression)add.Left;
            Assert.AreEqual(31, (int)ld.Value);
            Assert.AreEqual(0, ld.IlOffset);

            ld = (LiteralExpression)add.Right;
            Assert.AreEqual(2, (int)ld.Value);
            Assert.AreEqual(5, ld.IlOffset);

        }

        [TestMethod]
        public void Arithmetic_Multiply()
        {
            var tester = new SBTester();
            var il = tester.Il;
            il.Emit(OpCodes.Ldc_I4, 16);
            il.Emit(OpCodes.Ldc_I4_3);
            il.Emit(OpCodes.Mul);
            il.Emit(OpCodes.Ret);
            var expr = tester.GetExpression();

            Assert.AreEqual("(16 Multiply 3)", expr.ToString());

            var add = (BinaryExpression)expr;
            Assert.AreEqual(BinaryOperator.Multiply, add.Operator);
            Assert.AreEqual(6, add.IlOffset);
            Assert.AreEqual(0, add.FirstIlOffset);

            var ld = (LiteralExpression)add.Left;
            Assert.AreEqual(16, (int)ld.Value);
            Assert.AreEqual(0, ld.IlOffset);

            ld = (LiteralExpression)add.Right;
            Assert.AreEqual(3, (int)ld.Value);
            Assert.AreEqual(5, ld.IlOffset);

        }

        [TestMethod]
        public void Arithmetic_Divide()
        {
            var tester = new SBTester();
            var il = tester.Il;
            il.Emit(OpCodes.Ldc_I4, 16);
            il.Emit(OpCodes.Ldc_I4_4);
            il.Emit(OpCodes.Div);
            il.Emit(OpCodes.Ret);
            var expr = tester.GetExpression();

            Assert.AreEqual("(16 Divide 4)", expr.ToString());

            var add = (BinaryExpression)expr;
            Assert.AreEqual(BinaryOperator.Divide, add.Operator);
            Assert.AreEqual(6, add.IlOffset);
            Assert.AreEqual(0, add.FirstIlOffset);

            var ld = (LiteralExpression)add.Left;
            Assert.AreEqual(16, (int)ld.Value);
            Assert.AreEqual(0, ld.IlOffset);

            ld = (LiteralExpression)add.Right;
            Assert.AreEqual(4, (int)ld.Value);
            Assert.AreEqual(5, ld.IlOffset);

        }

        [TestMethod]
        public void Arithmetic_Modulo()
        {
            var tester = new SBTester();
            var il = tester.Il;
            il.Emit(OpCodes.Ldc_I4, 16);
            il.Emit(OpCodes.Ldc_I4_5);
            il.Emit(OpCodes.Rem);
            il.Emit(OpCodes.Ret);
            var expr = tester.GetExpression();

            Assert.AreEqual("(16 Modulo 5)", expr.ToString());

            var add = (BinaryExpression)expr;
            Assert.AreEqual(BinaryOperator.Modulo, add.Operator);
            Assert.AreEqual(6, add.IlOffset);
            Assert.AreEqual(0, add.FirstIlOffset);

            var ld = (LiteralExpression)add.Left;
            Assert.AreEqual(16, (int)ld.Value);
            Assert.AreEqual(0, ld.IlOffset);

            ld = (LiteralExpression)add.Right;
            Assert.AreEqual(5, (int)ld.Value);
            Assert.AreEqual(5, ld.IlOffset);

        }

        [TestMethod]
        public void Arithmetic_BitwiseOr()
        {
            var tester = new SBTester();
            var il = tester.Il;
            il.Emit(OpCodes.Ldc_I4, 16);
            il.Emit(OpCodes.Ldc_I4_6);
            il.Emit(OpCodes.Or);
            il.Emit(OpCodes.Ret);
            var expr = tester.GetExpression();

            Assert.AreEqual("(16 BitwiseOr 6)", expr.ToString());

            var add = (BinaryExpression)expr;
            Assert.AreEqual(BinaryOperator.BitwiseOr, add.Operator);
            Assert.AreEqual(6, add.IlOffset);
            Assert.AreEqual(0, add.FirstIlOffset);

            var ld = (LiteralExpression)add.Left;
            Assert.AreEqual(16, (int)ld.Value);
            Assert.AreEqual(0, ld.IlOffset);

            ld = (LiteralExpression)add.Right;
            Assert.AreEqual(6, (int)ld.Value);
            Assert.AreEqual(5, ld.IlOffset);

        }

        [TestMethod]
        public void Arithmetic_BitwiseAnd()
        {
            var tester = new SBTester();
            var il = tester.Il;
            il.Emit(OpCodes.Ldc_I4, 16);
            il.Emit(OpCodes.Ldc_I4_7);
            il.Emit(OpCodes.And);
            il.Emit(OpCodes.Ret);
            var expr = tester.GetExpression();

            Assert.AreEqual("(16 BitwiseAnd 7)", expr.ToString());

            var add = (BinaryExpression)expr;
            Assert.AreEqual(BinaryOperator.BitwiseAnd, add.Operator);
            Assert.AreEqual(6, add.IlOffset);
            Assert.AreEqual(0, add.FirstIlOffset);

            var ld = (LiteralExpression)add.Left;
            Assert.AreEqual(16, (int)ld.Value);
            Assert.AreEqual(0, ld.IlOffset);

            ld = (LiteralExpression)add.Right;
            Assert.AreEqual(7, (int)ld.Value);
            Assert.AreEqual(5, ld.IlOffset);

        }

        [TestMethod]
        public void Arithmetic_BitwiseXor()
        {
            var tester = new SBTester();
            var il = tester.Il;
            il.Emit(OpCodes.Ldc_I4, 16);
            il.Emit(OpCodes.Ldc_I4_8);
            il.Emit(OpCodes.Xor);
            il.Emit(OpCodes.Ret);
            var expr = tester.GetExpression();

            Assert.AreEqual("(16 BitwiseXor 8)", expr.ToString());

            var add = (BinaryExpression)expr;
            Assert.AreEqual(BinaryOperator.BitwiseXor, add.Operator);
            Assert.AreEqual(6, add.IlOffset);
            Assert.AreEqual(0, add.FirstIlOffset);

            var ld = (LiteralExpression)add.Left;
            Assert.AreEqual(16, (int)ld.Value);
            Assert.AreEqual(0, ld.IlOffset);

            ld = (LiteralExpression)add.Right;
            Assert.AreEqual(8, (int)ld.Value);
            Assert.AreEqual(5, ld.IlOffset);

        }

        [TestMethod]
        public void Arithmetic_LeftShift()
        {
            var tester = new SBTester();
            var il = tester.Il;
            il.Emit(OpCodes.Ldc_I4, 16);
            il.Emit(OpCodes.Ldc_I4_2);
            il.Emit(OpCodes.Shl);
            il.Emit(OpCodes.Ret);
            var expr = tester.GetExpression();

            Assert.AreEqual("(16 LeftShift 2)", expr.ToString());

            var add = (BinaryExpression)expr;
            Assert.AreEqual(BinaryOperator.LeftShift, add.Operator);
            Assert.AreEqual(6, add.IlOffset);
            Assert.AreEqual(0, add.FirstIlOffset);

            var ld = (LiteralExpression)add.Left;
            Assert.AreEqual(16, (int)ld.Value);
            Assert.AreEqual(0, ld.IlOffset);

            ld = (LiteralExpression)add.Right;
            Assert.AreEqual(2, (int)ld.Value);
            Assert.AreEqual(5, ld.IlOffset);

        }

        [TestMethod]
        public void Arithmetic_RightShift()
        {
            var tester = new SBTester();
            var il = tester.Il;
            il.Emit(OpCodes.Ldc_I4, 16);
            il.Emit(OpCodes.Ldc_I4_2);
            il.Emit(OpCodes.Shr);
            il.Emit(OpCodes.Ret);
            var expr = tester.GetExpression();

            Assert.AreEqual("(16 RightShift 2)", expr.ToString());

            var add = (BinaryExpression)expr;
            Assert.AreEqual(BinaryOperator.RightShift, add.Operator);
            Assert.AreEqual(6, add.IlOffset);
            Assert.AreEqual(0, add.FirstIlOffset);

            var ld = (LiteralExpression)add.Left;
            Assert.AreEqual(16, (int)ld.Value);
            Assert.AreEqual(0, ld.IlOffset);

            ld = (LiteralExpression)add.Right;
            Assert.AreEqual(2, (int)ld.Value);
            Assert.AreEqual(5, ld.IlOffset);

        }

        [TestMethod]
        public void Arithmetic_Negate()
        {
            var tester = new SBTester();
            var il = tester.Il;
            il.Emit(OpCodes.Ldc_I4, 31);
            il.Emit(OpCodes.Neg);
            il.Emit(OpCodes.Ret);
            var expr = tester.GetExpression();

            Assert.AreEqual("Negate(31)", expr.ToString());

            var add = (UnaryExpression)expr;
            Assert.AreEqual(UnaryOperator.Negate, add.Operator);
            Assert.AreEqual(5, add.IlOffset);
            Assert.AreEqual(0, add.FirstIlOffset);

            var ld = (LiteralExpression)add.Operand;
            Assert.AreEqual(31, (int)ld.Value);
            Assert.AreEqual(0, ld.IlOffset);
            Assert.AreEqual(0, ld.FirstIlOffset);
        }

        [TestMethod]
        public void Arithmetic_BitwiseNot()
        {
            var tester = new SBTester();
            var il = tester.Il;
            il.Emit(OpCodes.Ldc_I4, 31);
            il.Emit(OpCodes.Not);
            il.Emit(OpCodes.Ret);
            var expr = tester.GetExpression();

            Assert.AreEqual("BitwiseNot(31)", expr.ToString());

            var add = (UnaryExpression)expr;
            Assert.AreEqual(UnaryOperator.BitwiseNot, add.Operator);
            Assert.AreEqual(5, add.IlOffset);
            Assert.AreEqual(0, add.FirstIlOffset);

            var ld = (LiteralExpression)add.Operand;
            Assert.AreEqual(31, (int)ld.Value);
            Assert.AreEqual(0, ld.IlOffset);
            Assert.AreEqual(0, ld.FirstIlOffset);
        }
    }
}
