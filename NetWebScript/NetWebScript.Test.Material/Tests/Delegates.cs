using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetWebScript;

namespace NetWebScript.Test.Material.Tests
{
    [TestClass, ScriptAvailable]
    public class Delegates
    {
        public static bool Method1()
        {
            return true;
        }

        public bool Method2()
        {
            return true;
        }

        
        [TestMethod]
        public void AnonymousDelegate()
        {
            bool sucess = false;
            Action action = delegate()
            {
                sucess = true;
            };
            action();
            Assert.AreEqual(true, sucess);

            int value = 0;
            action = delegate()
            {
                value++;
            };
            action();
            action();
            action();
            Assert.AreEqual(3, value);
        }

        [TestMethod]
        public void StaticDelegate()
        {
            Func<bool> action = new Func<bool>(Method1);
            Assert.AreEqual(true, action());
        }

        [TestMethod]
        public void InstanceDelegate()
        {
            Func<bool> action = new Func<bool>(Method2);
            Assert.AreEqual(true, action());
        }

    }
}
