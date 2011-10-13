using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetWebScript;

namespace NetWebScript.Test.Material.Tests.Collections.Generic
{
    [TestClass, ScriptAvailable]
    public class ListTest
	{
        [TestMethod]
        public void Initialize()
        {
            List<String> list = new List<String>() { "a", "b" };
            Assert.AreEqual(2, list.Count);
            Assert.AreEqual("a", list[0]);
            Assert.AreEqual("b", list[1]);
        }

        [TestMethod]
        public void Add()
        {
            List<String> list = new List<String>();
            list.Add("a");
            list.Add("b");
            Assert.AreEqual(2, list.Count);
            Assert.AreEqual("a", list[0]);
            Assert.AreEqual("b", list[1]);
        }

        [TestMethod]
        public void RemoveAt()
        {
            List<String> list = new List<String>();
            list.Add("a");
            list.Add("b");
            list.Add("c");
            list.RemoveAt(1);
            Assert.AreEqual(2, list.Count);
            Assert.AreEqual("a", list[0]);
            Assert.AreEqual("c", list[1]);
        }

        [TestMethod]
        public void Set()
        {
            List<String> list = new List<String>();
            list.Add("a");
            list.Add("b");
            list.Add("c");
            list[1] = "x";
            Assert.AreEqual(3, list.Count);
            Assert.AreEqual("a", list[0]);
            Assert.AreEqual("x", list[1]);
            Assert.AreEqual("c", list[2]);
        }


        [TestMethod]
        public void Foreach()
        {
            List<String> list = new List<String>();
            list.Add("a");
            list.Add("b");
            list.Add("c");
            int idx = 0;
            foreach (String value in list)
            {
                Assert.AreEqual(list[idx], value);
                idx++;
            }
            Assert.AreEqual(3, idx);
        }

    }
}
