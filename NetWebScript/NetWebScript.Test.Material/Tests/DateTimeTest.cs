using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NetWebScript.Test.Material.Tests
{
    [ScriptAvailable]
    [TestClass]
    public class DateTimeTest
    {

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
