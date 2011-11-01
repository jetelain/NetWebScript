using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NetWebScript.Test.Material.Tests.Collections.Generic
{
    [TestClass, ScriptAvailable]
    public class DictionaryTest
    {

        [TestMethod]
        public void Add()
        {
            var dict = new Dictionary<object, object>();
            var a = new object();
            var b = new object();
            var c = new object();
            var d = new object();
            Assert.AreEqual(0, dict.Count);
            dict.Add(a, b);
            Assert.AreEqual(1, dict.Count);
            Assert.AreSame(b, dict[a]);
            dict.Add(c, d);
            Assert.AreEqual(2, dict.Count);
            Assert.AreSame(b, dict[a]);
            Assert.AreSame(d, dict[c]);

        }

    }
}
