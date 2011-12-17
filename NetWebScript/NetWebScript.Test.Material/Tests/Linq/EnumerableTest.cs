using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NetWebScript.Test.Material.Tests.Linq
{
    [TestClass]
    [ScriptAvailable]
    public class EnumerableTest
    {
        [TestMethod]
        public void ToArray()
        {
            int[] tab = new int[] { 1, 2, 3, 4 };
            int[] tab2 = tab.ToArray();

            Assert.AreEqual(4, tab2.Length);
            Assert.AreEqual(1, tab2[0]);
            Assert.AreEqual(2, tab2[1]);
            Assert.AreEqual(3, tab2[2]);
            Assert.AreEqual(4, tab2[3]);
        }

        [TestMethod]
        public void Where()
        {
            int[] tab = new int[] { 6, 7, 8, 1, 2, 3, 4, 5, 9, 10 };
            int[] tab2 = tab.Where(i => i < 5).ToArray();

            Assert.AreEqual(4, tab2.Length);
            Assert.AreEqual(1, tab2[0]);
            Assert.AreEqual(2, tab2[1]);
            Assert.AreEqual(3, tab2[2]);
            Assert.AreEqual(4, tab2[3]);
        }


        [TestMethod]
        public void Select()
        {
            int[] tab = new int[] { 1, 2, 3, 4 };
            int[] tab2 = tab.Select(i => i + 5).ToArray();

            Assert.AreEqual(4, tab2.Length);
            Assert.AreEqual(6, tab2[0]);
            Assert.AreEqual(7, tab2[1]);
            Assert.AreEqual(8, tab2[2]);
            Assert.AreEqual(9, tab2[3]);
        }

        [TestMethod]
        public void All()
        {
            int[] tab = new int[] { 1, 2, 3, 4 };
            int[] tab0 = new int[] { };
            Assert.IsTrue(tab.All(i => i < 5));
            Assert.IsTrue(tab0.All(i => i != 2));
            Assert.IsFalse(tab.All(i => i != 2));
        }

        [TestMethod]
        public void AnyPredicate()
        {
            int[] tab = new int[] { 1, 2, 3, 4 };
            int[] tab0 = new int[] { };
            Assert.IsTrue(tab.Any(i => i < 5));
            Assert.IsTrue(tab.Any(i => i != 1));
            Assert.IsTrue(tab.Any(i => i == 1));
            Assert.IsFalse(tab.Any(i => i > 5));
            Assert.IsFalse(tab0.Any(i => i < 5));
        }

        [TestMethod]
        public void Any()
        {
            int[] tab1 = new int[] { 1, 2, 3, 4 };
            int[] tab0 = new int[] {};
            Assert.IsTrue(tab1.Any());
            Assert.IsFalse(tab0.Any());
        }
    }
}
