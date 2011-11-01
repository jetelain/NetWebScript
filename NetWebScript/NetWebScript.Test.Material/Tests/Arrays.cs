using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NetWebScript.Test.Material.Tests
{
    [TestClass, ScriptAvailable]
    public class Arrays
    {
        [TestMethod]
        public void CreateAndLength()
        {
            int[] tab = new int[10];
            Assert.IsNotNull(tab);
            Assert.AreEqual(10, tab.Length);
        }

        [TestMethod]
        public void InitialValueInt32()
        {
            int[] tab = new int[1];
            Assert.AreEqual(0, tab[0]);
        }

        [TestMethod]
        public void InitialValueObject()
        {
            object[] tab = new object[1];
            Assert.AreEqual(null, tab[0]);
        }

        [TestMethod]
        public void InlineArrayInt32()
        {
            int[] tab = new int[] { 1, 2, 3, 4, 5 };
            Assert.AreEqual(1, tab[0]);
            Assert.AreEqual(2, tab[1]);
            Assert.AreEqual(3, tab[2]);
            Assert.AreEqual(4, tab[3]);
            Assert.AreEqual(5, tab[4]);
            Assert.AreEqual(5, tab.Length);
        }

        /*[TestMethod]
        public void Foreach3()
        {
            String[] tab = new String[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" };
            int value = 1;
            foreach (String effective in tab)
            {
                Assert.AreEqual(value.ToString(), effective);
                value++;
            }
            Assert.AreEqual(11, value);
        }*/

        [TestMethod]
        public void Foreach()
        {
            String[] tab = new String[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" };
            int idx = 0;
            foreach (String effective in tab)
            {
                Assert.AreEqual(tab[idx], effective);
                idx++;
            }
            Assert.AreEqual(10, idx);
        }
    }
}
