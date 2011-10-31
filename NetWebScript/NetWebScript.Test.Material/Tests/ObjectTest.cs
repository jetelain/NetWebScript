using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NetWebScript.Test.Material.Tests
{
    [TestClass, ScriptAvailable]
    public class ObjectTest
    {

        [TestMethod]
        public void DefaultEquals()
        {
            var a = new C();
            var b = new C();
            Assert.IsFalse(a.Equals(b));
            Assert.IsFalse(b.Equals(a));
            Assert.IsTrue(a.Equals(a));
            Assert.IsTrue(b.Equals(b));
            Assert.IsFalse(a.Equals(null));
            Assert.IsFalse(b.Equals(null));
        }

        [TestMethod]
        public void DefaultGetHashCode()
        {
            var a = new C();
            var b = new C();
            Assert.AreEqual(a.GetHashCode(), a.GetHashCode());
            Assert.AreEqual(b.GetHashCode(), b.GetHashCode());
        }


    }
}
