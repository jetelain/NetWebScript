
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetWebScript;

namespace NetWebScript.Test.Material.Tests
{
    [ScriptAvailable]
    public enum Quality
    {
        VeryBad,
        Normal,
        Good,
        Awesome
    }



    [ScriptAvailable]
    public class A
    {
        private String message;

        public A(String message)
        {
            this.message = message;
        }

        public virtual String WrappedMessage()
        {
            return ">" + message + "<";
        }


    }

    [ScriptAvailable]
    public class B : A
    {

        public B(String message)
            : base(message)
        {

        }

        public override String WrappedMessage()
        {
            return ">" + base.WrappedMessage() + "<";
        }


    }


    [ScriptAvailable]
    public class C : B
    {

        public C()
            : base("Hello")
        {

        }

        public override String WrappedMessage()
        {
            return ">" + base.WrappedMessage() + "<";
        }


    }

    [TestClass, ScriptAvailable]
    public class GeneratedTest
    {

        public static int Max(int a, int b)
        {
            return a > b ? a : b;
        }

        [TestMethod]
        public void GeneratedMethod()
        {
            int m = Max(10, 14);
            Assert.AreEqual(14, m);

            int a = 13;
            int b = 9;
            m = Max(a, b);
            Assert.AreEqual(13, m);
        }

        [TestMethod]
        public void GeneratedClass()
        {
            A test = new A("Hello");
            Assert.AreEqual(">Hello<", test.WrappedMessage());
        }

        [TestMethod]
        public void GeneratedSubClass()
        {
            A test = new B("Hello");
            Assert.AreEqual(">>Hello<<", test.WrappedMessage());
        }

        [TestMethod]
        public void GeneratedSubSubClass()
        {
            A test = new C();
            Assert.AreEqual(">>>Hello<<<", test.WrappedMessage());
        }

        [TestMethod]
        public void Generated_Is()
        {
            object a = new A("");
            object b = new B("");
            object c = new C();

            Assert.IsTrue(a is A);
            Assert.IsTrue(b is A);
            Assert.IsTrue(c is A);

            Assert.IsFalse(a is B);
            Assert.IsTrue(b is B);
            Assert.IsTrue(c is B);

            Assert.IsFalse(a is C);
            Assert.IsFalse(b is C);
            Assert.IsTrue(c is C);
        }

        [TestMethod]
        public void Generated_As()
        {
            object a = new A("");
            object b = new B("");
            object c = new C();

            Assert.AreEqual(a,a as A);
            Assert.AreEqual(b,b as A);
            Assert.AreEqual(c,c as A);

            Assert.AreEqual(null,a as B);
            Assert.AreEqual(b,b as B);
            Assert.AreEqual(c,c as B);

            Assert.AreEqual(null,a as C);
            Assert.AreEqual(null,b as C);
            Assert.AreEqual(c,c as C);
        }

        [TestMethod]
        public void GeneratedEnum()
        {
            Quality qual = Quality.Good;
            Assert.AreEqual("Good", qual.ToString());
        }
    }
}
