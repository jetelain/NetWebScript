﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Globalization;

namespace NetWebScript.Test.Material.Tests
{
    [TestClass]
    [ScriptAvailable]
    public class Int32Test
    {

        [TestMethod]
        public void Int32_Format_Decimal()
        {
            int a = 32;
            int b = -4;
            int c = 328923876;
            int d = -349780423;

            NumberFormatInfo info = new NumberFormatInfo();
            info.NegativeSign = "-";
            Assert.AreEqual("32", a.ToString(null, info));
            Assert.AreEqual("-4", b.ToString(null, info));
            Assert.AreEqual("328923876", c.ToString(null, info));
            Assert.AreEqual("-349780423", d.ToString(null, info));

            Assert.AreEqual("32", a.ToString("d", info));
            Assert.AreEqual("-4", b.ToString("d", info));
            Assert.AreEqual("328923876", c.ToString("d", info));
            Assert.AreEqual("-349780423", d.ToString("d", info));

            Assert.AreEqual("00032", a.ToString("d5", info));
            Assert.AreEqual("-00004", b.ToString("d5", info));
            Assert.AreEqual("328923876", c.ToString("d5", info));
            Assert.AreEqual("-349780423", d.ToString("d5", info));

            info.NegativeSign = "!";
            Assert.AreEqual("32", a.ToString(null, info));
            Assert.AreEqual("!4", b.ToString(null, info));
            Assert.AreEqual("328923876", c.ToString(null, info));
            Assert.AreEqual("!349780423", d.ToString(null, info));

            Assert.AreEqual("32", a.ToString("d", info));
            Assert.AreEqual("!4", b.ToString("d", info));
            Assert.AreEqual("328923876", c.ToString("d", info));
            Assert.AreEqual("!349780423", d.ToString("d", info));

            Assert.AreEqual("00032", a.ToString("d5", info));
            Assert.AreEqual("!00004", b.ToString("d5", info));
            Assert.AreEqual("328923876", c.ToString("d5", info));
            Assert.AreEqual("!349780423", d.ToString("d5", info));
        }

        [TestMethod]
        public void Int32_Format_Number()
        {
            int a = 32;
            int b = -4;
            int c = 328923876;
            int d = -349780423;

            NumberFormatInfo info = new NumberFormatInfo();
            info.NegativeSign = "-";
            info.NumberGroupSeparator = " ";
            info.NumberGroupSizes = new[] { 3 };
            info.NumberDecimalDigits = 2;
            info.NumberDecimalSeparator = ".";
            info.NumberNegativePattern = 1;

            Assert.AreEqual("32.00", a.ToString("n", info));
            Assert.AreEqual("-4.00", b.ToString("n", info));
            Assert.AreEqual("328 923 876.00", c.ToString("n", info));
            Assert.AreEqual("-349 780 423.00", d.ToString("n", info));

            Assert.AreEqual("32.00000", a.ToString("n5", info));
            Assert.AreEqual("-4.00000", b.ToString("n5", info));
            Assert.AreEqual("328 923 876.00000", c.ToString("n5", info));
            Assert.AreEqual("-349 780 423.00000", d.ToString("n5", info));

            info.NegativeSign = "!";
            info.NumberDecimalSeparator = ",";

            Assert.AreEqual("32,00", a.ToString("n", info));
            Assert.AreEqual("!4,00", b.ToString("n", info));
            Assert.AreEqual("328 923 876,00", c.ToString("n", info));
            Assert.AreEqual("!349 780 423,00", d.ToString("n", info));

            Assert.AreEqual("32,00000", a.ToString("n5", info));
            Assert.AreEqual("!4,00000", b.ToString("n5", info));
            Assert.AreEqual("328 923 876,00000", c.ToString("n5", info));
            Assert.AreEqual("!349 780 423,00000", d.ToString("n5", info));

            info.NumberNegativePattern = 0;
            info.NegativeSign = "-";
            Assert.AreEqual("32,00", a.ToString("n", info));
            Assert.AreEqual("(4,00)", b.ToString("n", info));

            info.NumberNegativePattern = 2;
            Assert.AreEqual("32,00", a.ToString("n", info));
            Assert.AreEqual("- 4,00", b.ToString("n", info));

            info.NumberNegativePattern = 3;
            Assert.AreEqual("32,00", a.ToString("n", info));
            Assert.AreEqual("4,00-", b.ToString("n", info));

            info.NumberNegativePattern = 4;
            Assert.AreEqual("32,00", a.ToString("n", info));
            Assert.AreEqual("4,00 -", b.ToString("n", info));

            info.NumberDecimalDigits = 0;
            info.NumberNegativePattern = 1;
            Assert.AreEqual("32", a.ToString("n", info));
            Assert.AreEqual("-4", b.ToString("n", info));
        }

        [TestMethod]
        public void Int32_Format_Hexa()
        {
            int a = 32;
            int b = -4;
            int c = 328923876;
            int d = -349780423;

            NumberFormatInfo info = new NumberFormatInfo();
            
            Assert.AreEqual("20", a.ToString("x", info));
            Assert.AreEqual("fffffffc", b.ToString("x", info));
            Assert.AreEqual("139afae4", c.ToString("x", info));
            Assert.AreEqual("eb26c639", d.ToString("x", info));

            Assert.AreEqual("00020", a.ToString("x5", info));
            Assert.AreEqual("fffffffc", b.ToString("x5", info));
            Assert.AreEqual("139afae4", c.ToString("x5", info));
            Assert.AreEqual("eb26c639", d.ToString("x5", info));

            Assert.AreEqual("20", a.ToString("X", info));
            Assert.AreEqual("FFFFFFFC", b.ToString("X", info));
            Assert.AreEqual("139AFAE4", c.ToString("X", info));
            Assert.AreEqual("EB26C639", d.ToString("X", info));

            Assert.AreEqual("00020", a.ToString("X5", info));
            Assert.AreEqual("FFFFFFFC", b.ToString("X5", info));
            Assert.AreEqual("139AFAE4", c.ToString("X5", info));
            Assert.AreEqual("EB26C639", d.ToString("X5", info));
        }

        [TestMethod]
        public void Int32_Parse_Integer()
        {
            Assert.AreEqual(32, int.Parse("32"));
            Assert.AreEqual(32, int.Parse("+32"));
            Assert.AreEqual(-4, int.Parse("-4"));
            Assert.AreEqual(328923876, int.Parse("328923876"));
            Assert.AreEqual(328923876, int.Parse("+328923876"));
            Assert.AreEqual(-349780423, int.Parse("-349780423"));

            NumberFormatInfo info = new NumberFormatInfo();
            info.NegativeSign = "-";
            info.PositiveSign = "+";
            Assert.AreEqual(32, int.Parse("32", info));
            Assert.AreEqual(32, int.Parse("+32", info));
            Assert.AreEqual(-4, int.Parse("-4", info));
            Assert.AreEqual(328923876, int.Parse("328923876", info));
            Assert.AreEqual(328923876, int.Parse("+328923876", info));
            Assert.AreEqual(-349780423, int.Parse("-349780423", info));

            info = new NumberFormatInfo();
            info.NegativeSign = "!";
            info.PositiveSign = "@";
            Assert.AreEqual(32, int.Parse("32", info));
            Assert.AreEqual(32, int.Parse("@32", info));
            Assert.AreEqual(-4, int.Parse("!4", info));
            Assert.AreEqual(328923876, int.Parse("328923876", info));
            Assert.AreEqual(328923876, int.Parse("@328923876", info));
            Assert.AreEqual(-349780423, int.Parse("!349780423", info));
        }
    }
}
