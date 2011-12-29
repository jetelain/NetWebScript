using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Globalization;

namespace NetWebScript.Test.Material.Tests
{
    [ScriptAvailable]
    [TestClass]
    public class DateTimeTest
    {
        [TestMethod]
        public void DateTime_Equals()
        {
            var a = new DateTime(2011, 12, 29, 13, 34, 49, 639, DateTimeKind.Local);
            var b = new DateTime(2011, 12, 29, 13, 34, 49, 639, DateTimeKind.Local);
            var c = new DateTime(2011, 12, 30, 13, 34, 49, 639, DateTimeKind.Local);

            Assert.IsTrue(a.Equals(b));
            Assert.IsFalse(a.Equals(c));
            Assert.IsTrue(b.Equals(a));
            Assert.IsFalse(b.Equals(c));

            Assert.IsTrue(object.Equals(a, b));
            Assert.IsFalse(object.Equals(a, c));
            Assert.IsTrue(object.Equals(b, a));
            Assert.IsFalse(object.Equals(b, c));
        }

        [TestMethod]
        public void DateTime_ToString_Local()
        {
            var f = DateTimeFormatInfo.InvariantInfo;
            var a = new DateTime(2011, 12, 29, 13, 34, 49, 639, DateTimeKind.Local);
            Assert.AreEqual("12/29/2011 13:34:49", a.ToString(f));
            Assert.AreEqual("12/29/2011 13:34:49", a.ToString(null, f));
            Assert.AreEqual("Thursday, 29 December 2011 13:34", a.ToString("f", f));
            Assert.AreEqual("Thursday, 29 December 2011 13:34:49", a.ToString("F", f));
            Assert.AreEqual("12/29/2011", a.ToString("d", f));
            Assert.AreEqual("Thursday, 29 December 2011", a.ToString("D", f));
            Assert.AreEqual("13:34", a.ToString("t", f));
            Assert.AreEqual("13:34:49", a.ToString("T", f));
            Assert.AreEqual("12/29/2011 13:34", a.ToString("g", f));
            Assert.AreEqual("12/29/2011 13:34:49", a.ToString("G", f));
            Assert.AreEqual("2011-12-29T13:34:49", a.ToString("s", f));
            Assert.AreEqual("2011-12-29T13:34:49", a.ToString("s", f));
            Assert.AreEqual("ddddThursday|dddThu|dd29|d29|December|Dec|12|12|2011|11|11|01|1|13|13|34|34|49|49|PM|P|639|63|6", a.ToString("'dddd'dddd|'ddd'ddd|'dd'dd|'d'd|MMMM|MMM|MM|M|yyyy|yy|y|hh|h|HH|H|mm|m|ss|s|tt|t|fff|ff|f", f));
        }


        [TestMethod]
        public void DateTime_DaysInMonth()
        {
            Assert.AreEqual(31, DateTime.DaysInMonth(2011, 1));
            Assert.AreEqual(28, DateTime.DaysInMonth(2011, 2));
            Assert.AreEqual(31, DateTime.DaysInMonth(2011, 3));
            Assert.AreEqual(30, DateTime.DaysInMonth(2011, 4));
            Assert.AreEqual(31, DateTime.DaysInMonth(2011, 5));
            Assert.AreEqual(30, DateTime.DaysInMonth(2011, 6));
            Assert.AreEqual(31, DateTime.DaysInMonth(2011, 7));
            Assert.AreEqual(31, DateTime.DaysInMonth(2011, 8));
            Assert.AreEqual(30, DateTime.DaysInMonth(2011, 9));
            Assert.AreEqual(31, DateTime.DaysInMonth(2011, 10));
            Assert.AreEqual(30, DateTime.DaysInMonth(2011, 11));
            Assert.AreEqual(31, DateTime.DaysInMonth(2011, 12));


            Assert.AreEqual(29, DateTime.DaysInMonth(2012, 2));
            Assert.AreEqual(29, DateTime.DaysInMonth(2004, 2));
            Assert.AreEqual(29, DateTime.DaysInMonth(2000, 2));
            Assert.AreEqual(28, DateTime.DaysInMonth(2100, 2));
        }
    }
}
