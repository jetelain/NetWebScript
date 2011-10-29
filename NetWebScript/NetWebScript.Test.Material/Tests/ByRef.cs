using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NetWebScript.Test.Material.Tests
{
    [TestClass, ScriptAvailable]
    public class ByRef
    {

        private static void SetValue(string value, out string target)
        {
            target = value;
        }

        private string field;

        [TestMethod]
        public void InstanceFieldRef()
        {
            field = "OldValue";
            SetValue("NewValue", out field);
            Assert.AreEqual("NewValue", field);
        }


        private static string sfield;

        [TestMethod]
        public void StaticFieldRef()
        {
            sfield = "OldValue";
            SetValue("NewValue", out sfield);
            Assert.AreEqual("NewValue", sfield);
        }
    }
}
