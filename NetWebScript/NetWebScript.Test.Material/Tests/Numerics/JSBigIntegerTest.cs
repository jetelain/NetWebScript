using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetWebScript.Script.Numercis;
using NetWebScript.Script;

namespace NetWebScript.Test.Material.Tests.Numerics
{
    /// <summary>
    /// Unit tests of <see cref="JSBigInteger"/>
    /// </summary>
    [TestClass]
    [ScriptAvailable]
    public class JSBigIntegerTest
    {
        [TestMethod]
        public void JSBigInteger_ParseSimple()
        {
            Assert.AreEqual(JSBigInteger.FromInteger(1), JSBigInteger.Parse("1"));
            Assert.AreEqual(JSBigInteger.FromInteger(1234567890), JSBigInteger.Parse("1234567890"));
            Assert.AreEqual(JSBigInteger.FromInteger(2147483647), JSBigInteger.Parse("2147483647"));
        }
        [TestMethod]
        public void JSBigInteger_AddSimple()
        {
            var mOne = JSBigInteger.Parse("-111111111111111111111111111111");
            var mTwo = JSBigInteger.Parse("-222222222222222222222222222222");
            var mThree = JSBigInteger.Parse("-333333333333333333333333333333");
            var one = JSBigInteger.Parse("111111111111111111111111111111");
            var two = JSBigInteger.Parse("222222222222222222222222222222");
            var three = JSBigInteger.Parse("333333333333333333333333333333");

            Assert.AreEqual(two, one.Add(one));
            Assert.AreEqual(three, one.Add(two));
            Assert.AreEqual(three, two.Add(one));
            Assert.AreEqual(JSBigInteger.ZERO, one.Add(mOne));
            Assert.AreEqual(one, two.Add(mOne));
            Assert.AreEqual(mOne, two.Add(mThree));
            Assert.AreEqual(mThree, mOne.Add(mTwo));

            Assert.AreEqual(JSBigInteger.FromInteger(2), JSBigInteger.Parse("1").Add(JSBigInteger.Parse("1")));
        }

        //string[] addResults = getAnswers("add");
        //string[] subtractResults = getAnswers("subtract");
        //string[] multiplyResults = getAnswers("multiply");
        //string[] divRemResults = getAnswers("divRem");
        //string[] negateResults = getAnswers("negate");
        //string[] nextResults = getAnswers("next");
        //string[] prevResults = getAnswers("prev");
        //string[] absResults = getAnswers("abs");
        //string[] compareAbsResults = getAnswers("compareAbs");
        //string[] compareResults = getAnswers("compare");
        //string[] isUnitResults = getAnswers("isUnit");
        //string[] isZeroResults = getAnswers("isZero");
        //string[] isPositiveResults = getAnswers("isPositive");
        //string[] isNegativeResults = getAnswers("isNegative");
        //string[] squareResults = getAnswers("square");
        //string[] isEvenResults = getAnswers("isEven");
        //string[] isOddResults = getAnswers("isOdd");
        //string[] signResults = getAnswers("sign");
        //string[] exp10Results = getAnswers("exp10");
        //string[] powResults = getAnswers("pow");
        //string[] modPowResults = getAnswers("modPow");

        void runUnaryOperationTest(string[] expect, Func<string, string> test, string[] values)
        {
            var n = values.Length;
            for (var i = 0; i < n; i++)
            {
                var expected = expect[i];
                var result = test(values[i]);
                //checkBigInteger(result);
                if (expected != result)
                {
                    Assert.Fail(i + ": expected <" + expected + "> got <" + result + ">");
                }
            }
        }

        void runBinaryOperationTest(string[] expect, Func<string, string, string> test)
        {
            runBinaryOperationTest(expect, test, JSBigIntegerTestValues.testValues, JSBigIntegerTestValues.testValues);
        }

        void runBinaryOperationTest(string[] expect, Func<string, string, string> test, string[] values1, string[] values2)
        {
            var n1 = values1.Length;
            var n2 = values2.Length;
            for (var i = 0; i < n1; i++)
            {
                for (var j = 0; j < n2; j++)
                {
                    var expected = expect[i * n2 + j];
                    var result = test(values1[i], values2[j]);
                    if (expected != result)
                    {
                        Assert.Fail(i + "," + j + ": expected <" + expected + "> got <" + result + ">");
                    }
                }
            }
        }

        void runTrinaryOperationTest(string[] expect, Func<string, string, string, string> test, string[] values1, string[] values2, string[] values3)
        {

            var n1 = values1.Length;
            var n2 = values2.Length;
            var n3 = values3.Length;
            for (var i = 0; i < n1; i++)
            {
                for (var j = 0; j < n2; j++)
                {
                    for (var k = 0; k < n3; k++)
                    {
                        var expected = expect[(i * n2 + j) * n3 + k];
                        var result = test(values1[i], values2[j], values3[k]);
                        //if (result instanceof BigInteger) {
                        //    checkBigInteger(result);
                        //}
                        if (expected != result)
                        {
                            Assert.Fail(string.Join(",", new[] { i, j, k }) + ": expected <" + expected + "> got <" + result + ">");
                        }
                    }
                }
            }
        }

        //void runShortBinaryOperationTest(string[] expect, Func<string,string,string> test) {
        //    runBinaryOperationTest(expect, test, testValues1, shortTestValues);
        //}

        void checkBigInteger(JSBigInteger n, JSNumber[] d, int s)
        {

            var sign = n._s;
            var digits = n._d;

            if (sign == 0)
            {
                Assert.IsTrue(digits.Length == 0, "sign is zero, but array length is " + digits.Length);
            }
            if (digits.Length == 0)
            {
                Assert.IsTrue(sign == 0, "array length is zero, but sign is " + sign);
            }
            Assert.IsTrue(sign == 0 || sign == 1 || sign == -1, "sign is not one of {-1, 0, 1}: " + sign);

            Assert.IsTrue(digits.Length >= 0, "invalid digits array");
            if (digits.Length > 0)
            {
                Assert.IsTrue(digits[digits.Length - 1] != 0, "leading zero");
            }

            if (d != null)
            {
                CollectionAssert.AreEqual(new List<JSNumber>(d), new List<JSNumber>(digits));
            }
            if (s != 0)
            {
                Assert.AreEqual(s, sign);
            }
        }
        [TestMethod]
        public void JSBigInteger_Constructor()
        {
            var n = new JSBigInteger(new JSArray<JSNumber>(), 1);
            checkBigInteger(n, new JSArray<JSNumber>(), 0);

            n = new JSBigInteger(JSArray<JSNumber>.New(0, 0, 0), 1);
            checkBigInteger(n, new JSArray<JSNumber>(), 0);

            n = new JSBigInteger(JSArray<JSNumber>.New(1), 1);
            checkBigInteger(n, JSArray<JSNumber>.New(1), 1);

            n = new JSBigInteger(JSArray<JSNumber>.New(2, 0), 1);
            checkBigInteger(n, JSArray<JSNumber>.New(2), 1);

            n = new JSBigInteger(JSArray<JSNumber>.New(3), 0);
            checkBigInteger(n, JSArray<JSNumber>.New(3), 1);

            n = new JSBigInteger(JSArray<JSNumber>.New(4), -1);
            checkBigInteger(n, JSArray<JSNumber>.New(4), -1);

            n = new JSBigInteger(JSArray<JSNumber>.New(1, 2, 3), -1);
            checkBigInteger(n, JSArray<JSNumber>.New(1, 2, 3), -1);

            var a = JSArray<JSNumber>.New(3, 2, 1);
            n = new JSBigInteger(a, 1);
            a.Unshift(4);
            checkBigInteger(n, JSArray<JSNumber>.New(4, 3, 2, 1), 1);
        }
        [TestMethod]
        public void JSBigInteger_Conversion()
        {
            var n = JSBigInteger.Parse(-1);
            checkBigInteger(n, JSArray<JSNumber>.New(1), -1);

            n = JSBigInteger.Parse(-123);
            checkBigInteger(n, JSArray<JSNumber>.New(123), -1);

            n = JSBigInteger.Parse(4567);
            checkBigInteger(n, JSArray<JSNumber>.New(4567), 1);

            n = JSBigInteger.Parse("+42");
            checkBigInteger(n, JSArray<JSNumber>.New(42), 1);

            n = JSBigInteger.Parse("23x10^5");
            checkBigInteger(n, JSArray<JSNumber>.New(2300000), 1);

            n = JSBigInteger.Parse("3425 x 10 ^ -2");
            checkBigInteger(n, JSArray<JSNumber>.New(34), 1);

            n = JSBigInteger.Parse("342.5 x 10 ^ -2");
            checkBigInteger(n, JSArray<JSNumber>.New(3), 1);

            n = JSBigInteger.Parse("-23x10^5");
            checkBigInteger(n, JSArray<JSNumber>.New(2300000), -1);

            n = JSBigInteger.Parse("-3425 x 10 ^ -2");
            checkBigInteger(n, JSArray<JSNumber>.New(34), -1);

            n = JSBigInteger.Parse("23.45x10^5");
            checkBigInteger(n, JSArray<JSNumber>.New(2345000), 1);

            n = JSBigInteger.Parse("3425e-12");
            checkBigInteger(n, JSArray<JSNumber>.New(), 0);

            n = JSBigInteger.Parse("-3425e8");
            checkBigInteger(n, JSArray<JSNumber>.New(0, 34250), -1);

            n = JSBigInteger.Parse("3425e-12");
            checkBigInteger(n, JSArray<JSNumber>.New(), 0);

            n = JSBigInteger.Parse("+3425e0");
            checkBigInteger(n, JSArray<JSNumber>.New(3425), 1);

            n = JSBigInteger.Parse("0xDeadBeef");
            checkBigInteger(n, JSArray<JSNumber>.New(5928559, 373), 1);

            n = JSBigInteger.Parse("-0c715");
            checkBigInteger(n, JSArray<JSNumber>.New(461), -1);

            n = JSBigInteger.Parse("+0b1101");
            checkBigInteger(n, JSArray<JSNumber>.New(13), 1);
        }

        [TestMethod]
        public void JSBigInteger_Parse()
        {
            var n = JSBigInteger.Parse("0", 10);
            checkBigInteger(n, JSArray<JSNumber>.New(), 0);

            n = JSBigInteger.Parse("");
            checkBigInteger(n, JSArray<JSNumber>.New(), 0);

            n = JSBigInteger.Parse("1");
            checkBigInteger(n, JSArray<JSNumber>.New(1), 1);

            n = JSBigInteger.Parse("-1");
            checkBigInteger(n, JSArray<JSNumber>.New(1), -1);

            n = JSBigInteger.Parse("+42", 10);
            checkBigInteger(n, JSArray<JSNumber>.New(42), 1);

            n = JSBigInteger.Parse("+42", 5);
            checkBigInteger(n, JSArray<JSNumber>.New(22), 1);

            n = JSBigInteger.Parse("23x10^5");
            checkBigInteger(n, JSArray<JSNumber>.New(2300000), 1);

            n = JSBigInteger.Parse("3425 x 10 ^ -2");
            checkBigInteger(n, JSArray<JSNumber>.New(34), 1);

            n = JSBigInteger.Parse("342.5 x 10 ^ -2");
            checkBigInteger(n, JSArray<JSNumber>.New(3), 1);

            n = JSBigInteger.Parse("-23x10^5");
            checkBigInteger(n, JSArray<JSNumber>.New(2300000), -1);

            n = JSBigInteger.Parse("-3425 x 10 ^ -2");
            checkBigInteger(n, JSArray<JSNumber>.New(34), -1);

            n = JSBigInteger.Parse("23.45x10^5");
            checkBigInteger(n, JSArray<JSNumber>.New(2345000), 1);

            n = JSBigInteger.Parse("3425e-12");
            checkBigInteger(n, JSArray<JSNumber>.New(), 0);

            n = JSBigInteger.Parse("-3425e8");
            checkBigInteger(n, JSArray<JSNumber>.New(0, 34250), -1);

            n = JSBigInteger.Parse("-3425e-12");
            checkBigInteger(n, JSArray<JSNumber>.New(), 0);

            n = JSBigInteger.Parse("+3425e0");
            checkBigInteger(n, JSArray<JSNumber>.New(3425), 1);

            n = JSBigInteger.Parse("0xDeadBeef");
            checkBigInteger(n, JSArray<JSNumber>.New(5928559, 373), 1);

            n = JSBigInteger.Parse("12abz", 36);
            checkBigInteger(n, JSArray<JSNumber>.New(1786319), 1);

            n = JSBigInteger.Parse("-0c715");
            checkBigInteger(n, JSArray<JSNumber>.New(461), -1);

            n = JSBigInteger.Parse("+0b1101");
            checkBigInteger(n, JSArray<JSNumber>.New(13), 1);

            n = JSBigInteger.Parse("1011", 2);
            checkBigInteger(n, JSArray<JSNumber>.New(11), 1);

            n = JSBigInteger.Parse("1011", 3);
            checkBigInteger(n, JSArray<JSNumber>.New(31), 1);

            n = JSBigInteger.Parse("1011", 4);
            checkBigInteger(n, JSArray<JSNumber>.New(69), 1);

            n = JSBigInteger.Parse("1011", 5);
            checkBigInteger(n, JSArray<JSNumber>.New(131), 1);

            n = JSBigInteger.Parse("1011", 6);
            checkBigInteger(n, JSArray<JSNumber>.New(223), 1);

            n = JSBigInteger.Parse("1011", 7);
            checkBigInteger(n, JSArray<JSNumber>.New(351), 1);

            n = JSBigInteger.Parse("1011", 10);
            checkBigInteger(n, JSArray<JSNumber>.New(1011), 1);

            n = JSBigInteger.Parse("1011", 11);
            checkBigInteger(n, JSArray<JSNumber>.New(1343), 1);

            n = JSBigInteger.Parse("1011", 12);
            checkBigInteger(n, JSArray<JSNumber>.New(1741), 1);

            n = JSBigInteger.Parse("1011", 15);
            checkBigInteger(n, JSArray<JSNumber>.New(3391), 1);

            n = JSBigInteger.Parse("1011", 16);
            checkBigInteger(n, JSArray<JSNumber>.New(4113), 1);

            n = JSBigInteger.Parse("1011", 36);
            checkBigInteger(n, JSArray<JSNumber>.New(46693), 1);

            n = JSBigInteger.Parse("0b", 16);
            checkBigInteger(n, JSArray<JSNumber>.New(11), 1);

            n = JSBigInteger.Parse("0c", 16);
            checkBigInteger(n, JSArray<JSNumber>.New(12), 1);

            n = JSBigInteger.Parse("0b12", 16);
            checkBigInteger(n, JSArray<JSNumber>.New(2834), 1);

            n = JSBigInteger.Parse("0c12", 16);
            checkBigInteger(n, JSArray<JSNumber>.New(3090), 1);

            n = JSBigInteger.Parse("0b101", 2);
            checkBigInteger(n, JSArray<JSNumber>.New(5), 1);

            n = JSBigInteger.Parse("0c101", 8);
            checkBigInteger(n, JSArray<JSNumber>.New(65), 1);

            n = JSBigInteger.Parse("0x101", 16);
            checkBigInteger(n, JSArray<JSNumber>.New(257), 1);

            JSBigInteger.Parse("1", 2);
            JSBigInteger.Parse("2", 3);
            JSBigInteger.Parse("3", 4);
            JSBigInteger.Parse("4", 5);
            JSBigInteger.Parse("5", 6);
            JSBigInteger.Parse("6", 7);
            JSBigInteger.Parse("7", 8);
            JSBigInteger.Parse("8", 9);
            JSBigInteger.Parse("9", 10);

            JSBigInteger.Parse("a", 11);
            JSBigInteger.Parse("b", 12);
            JSBigInteger.Parse("c", 13);
            JSBigInteger.Parse("d", 14);
            JSBigInteger.Parse("e", 15);
            JSBigInteger.Parse("f", 16);
            JSBigInteger.Parse("g", 17);
            JSBigInteger.Parse("h", 18);
            JSBigInteger.Parse("i", 19);
            JSBigInteger.Parse("j", 20);

            JSBigInteger.Parse("k", 21);
            JSBigInteger.Parse("l", 22);
            JSBigInteger.Parse("m", 23);
            JSBigInteger.Parse("n", 24);
            JSBigInteger.Parse("o", 25);
            JSBigInteger.Parse("p", 26);
            JSBigInteger.Parse("q", 27);
            JSBigInteger.Parse("r", 28);
            JSBigInteger.Parse("s", 29);
            JSBigInteger.Parse("t", 30);

            JSBigInteger.Parse("u", 31);
            JSBigInteger.Parse("v", 32);
            JSBigInteger.Parse("w", 33);
            JSBigInteger.Parse("x", 34);
            JSBigInteger.Parse("y", 35);
            JSBigInteger.Parse("z", 36);
        }

        //function testParseFail() {
        //    function createTest(s, radix) {
        //        if (arguments.length < 2) {
        //            radix = 10;
        //        }
        //        return function() { JSBigInteger.Parse(s, radix); };
        //    }

        //    var radixError  = /^Illegal radix \d+./;
        //    var digitError  = /^Bad digit for radix \d+/;
        //    var formatError = /^Invalid BigInteger format: /;

        //    assertThrows(createTest("0", 1), radixError);
        //    assertThrows(createTest("0", 37), radixError);

        //    assertThrows(createTest("+ 42", 10), formatError);
        //    assertThrows(createTest("3425 x 10 ^ - 2"), formatError);
        //    assertThrows(createTest("34e-2", 16), formatError);
        //    assertThrows(createTest("- 23x10^5"), formatError);
        //    assertThrows(createTest("-+3425"), formatError);
        //    assertThrows(createTest("3425e-"), formatError);
        //    assertThrows(createTest("52", 5), digitError);
        //    assertThrows(createTest("23a105"), digitError);
        //    assertThrows(createTest("DeadBeef", 15), digitError);
        //    assertThrows(createTest("-0C715", 10), digitError);
        //    assertThrows(createTest("-0x715", 10), digitError);
        //    assertThrows(createTest("-0b715", 10), digitError);
        //    assertThrows(createTest("-0x715", 8), digitError);
        //    assertThrows(createTest("-0b715", 8), digitError);
        //    assertThrows(createTest("-0C715", 2), digitError);
        //    assertThrows(createTest("-0x715", 2), digitError);

        //    assertThrows(createTest("2", 2), digitError);
        //    assertThrows(createTest("3", 3), digitError);
        //    assertThrows(createTest("4", 4), digitError);
        //    assertThrows(createTest("5", 5), digitError);
        //    assertThrows(createTest("6", 6), digitError);
        //    assertThrows(createTest("7", 7), digitError);
        //    assertThrows(createTest("8", 8), digitError);
        //    assertThrows(createTest("9", 9), digitError);
        //    assertThrows(createTest("a", 10), digitError);
        //    assertThrows(createTest("b", 11), digitError);
        //    assertThrows(createTest("c", 12), digitError);
        //    assertThrows(createTest("d", 13), digitError);
        //    assertThrows(createTest("e", 14), digitError);
        //    assertThrows(createTest("f", 15), digitError);
        //    assertThrows(createTest("g", 16), digitError);
        //    assertThrows(createTest("h", 17), digitError);
        //    assertThrows(createTest("i", 18), digitError);
        //    assertThrows(createTest("j", 19), digitError);
        //    assertThrows(createTest("k", 20), digitError);
        //    assertThrows(createTest("l", 21), digitError);
        //    assertThrows(createTest("m", 22), digitError);
        //    assertThrows(createTest("n", 23), digitError);
        //    assertThrows(createTest("o", 24), digitError);
        //    assertThrows(createTest("p", 25), digitError);
        //    assertThrows(createTest("q", 26), digitError);
        //    assertThrows(createTest("r", 27), digitError);
        //    assertThrows(createTest("s", 28), digitError);
        //    assertThrows(createTest("t", 29), digitError);
        //    assertThrows(createTest("u", 30), digitError);
        //    assertThrows(createTest("v", 31), digitError);
        //    assertThrows(createTest("w", 32), digitError);
        //    assertThrows(createTest("x", 33), digitError);
        //    assertThrows(createTest("y", 34), digitError);
        //    assertThrows(createTest("z", 35), digitError);
        //};

        [TestMethod]
        public void JSBigInteger_ToString()
        {
            var narray = new[]{
		        new JSBigInteger(new JSNumber[0], 1).ToString(),
		        JSBigInteger.Parse(-1).ToString(),
		        JSBigInteger.Parse(-123).ToString(),
		        JSBigInteger.Parse(456).ToString(),
		        JSBigInteger.Parse("+42").ToString(),
		        JSBigInteger.Parse("23x10^5").ToString(),
		        JSBigInteger.Parse("342.5 x 10 ^ -2").ToString(),
		        JSBigInteger.Parse("-23x10^5").ToString(),
		        JSBigInteger.Parse("-3425 x 10 ^ -2").ToString(),
		        JSBigInteger.Parse("23.45x10^5").ToString(),
		        JSBigInteger.Parse("3425e-12").ToString(),
		        JSBigInteger.Parse("-3425e8").ToString(),
		        JSBigInteger.Parse("+3425e0").ToString(10),
		        JSBigInteger.Parse("0xDeadBeef").ToString(16),
		        JSBigInteger.Parse("-0c715").ToString(8),
		        JSBigInteger.Parse("+0b1101").ToString(2),
		        JSBigInteger.Parse("+42", 5).ToString(10),
		        JSBigInteger.Parse("+42", 5).ToString(5),
		        JSBigInteger.Parse("12abz", 36).ToString(36),
		        JSBigInteger.Parse("-0c715").ToString(),
                };
            var sarray = new[]{
		        "0",
		        "-1",
		        "-123",
		        "456",
		        "42",
		        "2300000",
		        "3",
		        "-2300000",
		        "-34",
		        "2345000",
		        "0",
		        "-342500000000",
		        "3425",
		        "DEADBEEF",
		        "-715",
		        "1101",
		        "22",
		        "42",
		        "12ABZ",
		        "-461"
	        };
            CollectionAssert.AreEqual(new List<string>(sarray), new List<string>(narray));
            //assertArraySimilar(sarray, narray);
            //assertArraySimilar(testStrings, testValues1);
        }


        [TestMethod]
        public void JSBigInteger_Constants()
        {
            Assert.AreEqual(37, JSBigInteger.small.Length);

            checkBigInteger(JSBigInteger.small[0], JSArray<JSNumber>.New(), 0);
            checkBigInteger(JSBigInteger.ZERO, JSArray<JSNumber>.New(), 0);
            checkBigInteger(JSBigInteger.ONE, JSArray<JSNumber>.New(1), 1);
            checkBigInteger(JSBigInteger.M_ONE, JSArray<JSNumber>.New(1), -1);

            for (var i = 1; i <= 36; i++)
            {
                checkBigInteger(JSBigInteger.small[i], JSArray<JSNumber>.New(i), 1);
            }
        }


        [TestMethod]
        public void JSBigInteger_ValueOf() {
            var narray = new[]{
                new JSBigInteger(new JSNumber[0], 1),
                JSBigInteger.Parse(-1),
                JSBigInteger.Parse(-123),
                JSBigInteger.Parse(456),
                JSBigInteger.Parse("+42"),
                JSBigInteger.Parse("23x10^5"),
                JSBigInteger.Parse("342.5 x 10 ^ -2"),
                JSBigInteger.Parse("-23x10^5"),
                JSBigInteger.Parse("-3425 x 10 ^ -2"),
                JSBigInteger.Parse("23.45x10^5"),
                JSBigInteger.Parse("3425e-12"),
                JSBigInteger.Parse("-3425e8"),
                JSBigInteger.Parse("+3425e0"),
                JSBigInteger.Parse("0xDeadBeef"),
                JSBigInteger.Parse("-0c715"),
                JSBigInteger.Parse("+0b1101"),
                JSBigInteger.Parse("+42", 5),
                JSBigInteger.Parse("+42", 5),
                JSBigInteger.Parse("12abz", 36),
                JSBigInteger.Parse("-0c715")
            };
            var jsarray = new JSNumber[]{
                0,
                -1,
                -123,
                456,
                42,
                JSNumber.ParseInt("2300000", 10),
                JSNumber.ParseInt("3", 10),
                JSNumber.ParseInt("-2300000", 10),
                JSNumber.ParseInt("-34", 10),
                JSNumber.ParseInt("2345000", 10),
                JSNumber.ParseInt("0", 10),
                JSNumber.ParseInt("-342500000000", 10),
                JSNumber.ParseInt("3425", 10),
                JSNumber.ParseInt("DeadBeef", 16),
                JSNumber.ParseInt("-715", 8),
                JSNumber.ParseInt("1101", 2),
                JSNumber.ParseInt("22", 10),
                JSNumber.ParseInt("42", 5),
                JSNumber.ParseInt("12ABZ", 36),
                JSNumber.ParseInt("-461", 10)
            };

            CollectionAssert.AreEqual(new List<JSNumber>(jsarray), new List<JSBigInteger>(narray).Select(t => t.ValueOf()).ToList());
        }

        [TestMethod]
        public void JSBigInteger_Add() {
            runBinaryOperationTest(JSBigIntegerTestValues.addResults, (a, b) => {
                return JSBigInteger.Parse(a).Add(JSBigInteger.Parse(b)).ToString();
            });
        }

        [TestMethod]
        public void JSBigInteger_Subtract() {
            runBinaryOperationTest(JSBigIntegerTestValues.subtractResults, (a, b) =>
            {
                return JSBigInteger.Parse(a).Subtract(JSBigInteger.Parse(b)).ToString();
            });
        }

        [TestMethod]
        public void JSBigInteger_Multiply() {
            runBinaryOperationTest(JSBigIntegerTestValues.multiplyResults, (a, b) =>
            {
                return JSBigInteger.Parse(a).Multiply(JSBigInteger.Parse(b)).ToString();
            });
        }

        [TestMethod]
        public void JSBigInteger_DivRem() {

            var a0 = JSBigInteger.Parse(JSBigIntegerTestValues.testValues[3]);
            var b0 = JSBigInteger.Parse(JSBigIntegerTestValues.testValues[10]);
            var result0 = a0.DivRem(b0);
            Assert.AreEqual(JSBigIntegerTestValues.divRemResults[3 * JSBigIntegerTestValues.testValues.Length + 10], result0[0] + "," + result0[1]);

            runBinaryOperationTest(JSBigIntegerTestValues.divRemResults, (a, b) => {
                try {
                    var result = JSBigInteger.Parse(a).DivRem(JSBigInteger.Parse(b));
                    return result[0].ToString() + "," + result[1].ToString();
                }
                catch (Exception e) {
                    return e.Message;
                }
            });
        }

        //function testNegate() {
        //    runUnaryOperationTest(negateResults, function(a) {
        //        return a.negate().toString();
        //    });
        //};

        //function testNext() {
        //    runUnaryOperationTest(nextResults, function(a) {
        //        return a.next().toString();
        //    });
        //};

        //function testPrev() {
        //    runUnaryOperationTest(prevResults, function(a) {
        //        return a.prev().toString();
        //    });
        //};

        //function testAbs() {
        //    runUnaryOperationTest(absResults, function(a) {
        //        return a.abs().toString();
        //    });
        //};

        //function testCompareAbs() {
        //    runBinaryOperationTest(compareAbsResults, function(a, b) {
        //        return a.compareAbs(b);
        //    });
        //};

        //function testCompare() {
        //    runBinaryOperationTest(compareResults, function(a, b) {
        //        return a.compare(b);
        //    });
        //};

        //function testIsUnit() {
        //    runUnaryOperationTest(isUnitResults, function(a) {
        //        return a.isUnit();
        //    });
        //};

        //function testIsZero() {
        //    runUnaryOperationTest(isZeroResults, function(a) {
        //        return a.isZero();
        //    });
        //};

        //function testIsPositive() {
        //    runUnaryOperationTest(isPositiveResults, function(a) {
        //        return a.isPositive();
        //    });
        //};

        //function testIsNegative() {
        //    runUnaryOperationTest(isNegativeResults, function(a) {
        //        return a.isNegative();
        //    });
        //};

        //function testSquare() {
        //    runUnaryOperationTest(squareResults, function(a) {
        //        return a.square().toString();
        //    });
        //};

        //function testIsEven() {
        //    runUnaryOperationTest(isEvenResults, function(a) {
        //        return a.isEven();
        //    });
        //};

        //function testIsOdd() {
        //    runUnaryOperationTest(isOddResults, function(a) {
        //        return a.isOdd();
        //    });
        //};

        //function testSign() {
        //    runUnaryOperationTest(signResults, function(a) {
        //        return a.sign();
        //    });
        //};

        //function testExp10() {
        //    runShortBinaryOperationTest(exp10Results, function(a, b) {
        //        if (Math.abs(Number(b)) > 1000) {
        //            b = Number(BigInteger.MAX_EXP.next());
        //        }
        //        try {
        //            return a.exp10(b).toString();
        //        }
        //        catch (e) {
        //            return e.message;
        //        }
        //    });
        //};

        //function testPow() {
        //    runBinaryOperationTest(powResults, function(a, b) {
        //        try {
        //            return a.pow(b).toString();
        //        }
        //        catch (e) {
        //            return e.message;
        //        }
        //    },
        //    powValues, powValues);
        //}

        //void testModPow() {
        //    runTrinaryOperationTest(modPowResults, delegate(a, b, c) {
        //        try {
        //            return a.modPow(b, c).toString();
        //        }
        //        catch (e) {
        //            return e.message;
        //        }
        //    },
        //    powValues, powValues, powValues);
        //}

    }
}
