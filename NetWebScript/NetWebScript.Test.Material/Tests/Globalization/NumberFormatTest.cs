using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetWebScript.Equivalents.Globalization;

namespace NetWebScript.Test.Material.Tests.Globalization
{
    [TestClass]
    [ScriptAvailable]
    public class NumberFormatTest
    {
        [TestMethod]
        public void NumberFormat_GroupIntegerNumber()
        {
            int[] groups = new []{ 3 };
            string groupSep = ",";

            Assert.AreEqual("1", NumberFormat.GroupIntegerNumber("1", groups, groupSep));
            Assert.AreEqual("12", NumberFormat.GroupIntegerNumber("12", groups, groupSep));
            Assert.AreEqual("123", NumberFormat.GroupIntegerNumber("123", groups, groupSep));
            Assert.AreEqual("1,234", NumberFormat.GroupIntegerNumber("1234", groups, groupSep));
            Assert.AreEqual("12,345", NumberFormat.GroupIntegerNumber("12345", groups, groupSep));
            Assert.AreEqual("123,456", NumberFormat.GroupIntegerNumber("123456", groups, groupSep));
            Assert.AreEqual("1,234,567", NumberFormat.GroupIntegerNumber("1234567", groups, groupSep));
            Assert.AreEqual("12,345,678", NumberFormat.GroupIntegerNumber("12345678", groups, groupSep));
            Assert.AreEqual("123,456,789", NumberFormat.GroupIntegerNumber("123456789", groups, groupSep));
            Assert.AreEqual("1,234,567,890", NumberFormat.GroupIntegerNumber("1234567890", groups, groupSep));

            groups = new[] { 1, 2, 1, 2, 1, 2, 1, 2 };
            Assert.AreEqual("1", NumberFormat.GroupIntegerNumber("1", groups, groupSep));
            Assert.AreEqual("1,2", NumberFormat.GroupIntegerNumber("12", groups, groupSep));
            Assert.AreEqual("12,3", NumberFormat.GroupIntegerNumber("123", groups, groupSep));
            Assert.AreEqual("1,23,4", NumberFormat.GroupIntegerNumber("1234", groups, groupSep));
            Assert.AreEqual("1,2,34,5", NumberFormat.GroupIntegerNumber("12345", groups, groupSep));
            Assert.AreEqual("12,3,45,6", NumberFormat.GroupIntegerNumber("123456", groups, groupSep));
            Assert.AreEqual("1,23,4,56,7", NumberFormat.GroupIntegerNumber("1234567", groups, groupSep));
            Assert.AreEqual("1,2,34,5,67,8", NumberFormat.GroupIntegerNumber("12345678", groups, groupSep));
            Assert.AreEqual("12,3,45,6,78,9", NumberFormat.GroupIntegerNumber("123456789", groups, groupSep));
            Assert.AreEqual("1,23,4,56,7,89,0", NumberFormat.GroupIntegerNumber("1234567890", groups, groupSep));

            groups = new[] { 1, 2, 0 };
            Assert.AreEqual("1", NumberFormat.GroupIntegerNumber("1", groups, groupSep));
            Assert.AreEqual("1,2", NumberFormat.GroupIntegerNumber("12", groups, groupSep));
            Assert.AreEqual("12,3", NumberFormat.GroupIntegerNumber("123", groups, groupSep));
            Assert.AreEqual("1,23,4", NumberFormat.GroupIntegerNumber("1234", groups, groupSep));
            Assert.AreEqual("12,34,5", NumberFormat.GroupIntegerNumber("12345", groups, groupSep));
            Assert.AreEqual("123,45,6", NumberFormat.GroupIntegerNumber("123456", groups, groupSep));
            Assert.AreEqual("1234,56,7", NumberFormat.GroupIntegerNumber("1234567", groups, groupSep));
            Assert.AreEqual("12345,67,8", NumberFormat.GroupIntegerNumber("12345678", groups, groupSep));
            Assert.AreEqual("123456,78,9", NumberFormat.GroupIntegerNumber("123456789", groups, groupSep));
            Assert.AreEqual("1234567,89,0", NumberFormat.GroupIntegerNumber("1234567890", groups, groupSep));
        }
    }
}
