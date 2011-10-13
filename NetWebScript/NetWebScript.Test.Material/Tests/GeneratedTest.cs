
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetWebScript;

namespace NetWebScript.Test.Material.Tests
{
    [ScriptAvailable]
    public enum Quality
    {
        Moisi,
        Normal,
        Bien,
        Ultime
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
        public void GeneratedEnum()
        {
            Quality qual = Quality.Bien;
            Assert.AreEqual("Bien", qual.ToString());
        }
    }
}
