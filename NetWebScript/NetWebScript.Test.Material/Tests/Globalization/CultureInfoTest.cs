using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Globalization;

namespace NetWebScript.Test.Material.Tests.Globalization
{
    /// <summary>
    /// <see cref="CultureInfo"/> unit tests.
    /// </summary>
    [TestClass]
    [ScriptAvailable]
    public class CultureInfoTest
    {
        [TestMethod]
        public void CultureInfo_NameCtor()
        {
            var frFr = new CultureInfo("fr-FR");
            Assert.AreEqual("fr-FR", frFr.Name);
            Assert.AreEqual(",", frFr.NumberFormat.NumberDecimalSeparator);

            var enGB = new CultureInfo("en-GB");
            Assert.AreEqual("en-GB", enGB.Name);
            Assert.AreEqual(".", enGB.NumberFormat.NumberDecimalSeparator);

            var enUS = new CultureInfo("en-US");
            Assert.AreEqual("en-US", enUS.Name);
            Assert.AreEqual(".", enUS.NumberFormat.NumberDecimalSeparator);
        }
    }
}
