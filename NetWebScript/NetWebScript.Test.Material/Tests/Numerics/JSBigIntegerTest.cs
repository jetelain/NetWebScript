using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetWebScript.Script.Numercis;

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
        public void JSBigInteger_Parse()
        {
            Assert.AreEqual(JSBigInteger.FromInteger(1), JSBigInteger.Parse("1"));
            Assert.AreEqual(JSBigInteger.FromInteger(1234567890), JSBigInteger.Parse("1234567890"));
            Assert.AreEqual(JSBigInteger.FromInteger(2147483647), JSBigInteger.Parse("2147483647"));
        }
        [TestMethod]
        public void JSBigInteger_Add()
        {
            var mOne = JSBigInteger.Parse("-111111111111111111111111111111");
            var mTwo = JSBigInteger.Parse("-222222222222222222222222222222");
            var mThree = JSBigInteger.Parse("-333333333333333333333333333333");
            var one   = JSBigInteger.Parse("111111111111111111111111111111");
            var two   = JSBigInteger.Parse("222222222222222222222222222222");
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

    }
}
