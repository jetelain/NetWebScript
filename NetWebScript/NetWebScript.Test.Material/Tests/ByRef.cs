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

        private static void Append(string value, ref string target)
        {
            target = target + value;
        }

        private string field;
        private static string sfield;

        [TestMethod]
        public void InstanceFieldClassRef()
        {
            sfield = null;
            field = "OldValue";
            SetValue("NewValue", out field);
            Assert.AreEqual("NewValue", field);

            Append("Suffix", ref field);
            Assert.AreEqual("NewValueSuffix", field);
        }

        [TestMethod]
        public void StaticFieldClassRef()
        {
            field = null;
            sfield = "OldValue";
            SetValue("NewValue", out sfield);
            Assert.AreEqual("NewValue", sfield);
            
            Append("Suffix", ref sfield);
            Assert.AreEqual("NewValueSuffix", sfield);
        }

        [TestMethod]
        public void VariableClassRef()
        {
            field = null;
            sfield = null;
            string variable = "OldValue";
            SetValue("NewValue", out variable);
            Assert.AreEqual("NewValue", variable);

            Append("Suffix", ref variable);
            Assert.AreEqual("NewValueSuffix", variable);
        }
    }
}
