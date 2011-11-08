using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetWebScript.Script;

namespace NetWebScript.Test.Material.Tests
{
    [ScriptAvailable, Exported(IgnoreNamespace=true)]
    public class ExportedA
    {
        private readonly string info;

        public ExportedA(string info)
        {
            this.info = info;
        }

        public string Info
        {
            get { return info; }
        }

        public static string DefaultInfo
        {
            get { return "default info"; }
        }
    }

    [TestClass, ScriptAvailable]
    public class ExportedTest
    {
        [ScriptBody(Inline="new ExportedA(info)")]
        private static ExportedA CreateA(string info)
        {
            return new ExportedA(info);
        }

        [ScriptBody(Inline = "a.get_Info()")]
        private static string GetInfo(ExportedA a)
        {
            return a.Info;
        }

        [ScriptBody(Inline = "ExportedA.get_DefaultInfo()")]
        private static string GetDefaultInfo()
        {
            return ExportedA.DefaultInfo;
        }

        [TestMethod]
        public void ExportedConstructor()
        {
            var a = CreateA("Hello world !");
            object b = a;
            Assert.AreEqual("Hello world !", a.Info);
            Assert.AreEqual(typeof(ExportedA), a.GetType());
            Assert.IsTrue((b as ExportedA)!=null);
        }

        [TestMethod]
        public void ExportedInstanceMethod()
        {
            var a = new ExportedA("This is a string");
            Assert.AreEqual("This is a string", GetInfo(a));
        }

        [TestMethod]
        public void ExportedStaticMethod()
        {
            Assert.AreEqual("default info", GetDefaultInfo());
        }
    }
}
