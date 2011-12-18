using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetWebScript.Script;

namespace NetWebScript.Test.Material.Tests
{
    [TestClass, ScriptAvailable]
    public class MathTest
    {
        [TestMethod]
        public void JSMathRound()
        {
            Assert.AreEqual(4, JSMath.Round(4.1));
            Assert.AreEqual(4, JSMath.Round(4.4));
            Assert.AreEqual(5, JSMath.Round(4.5));
            Assert.AreEqual(5, JSMath.Round(4.6));
            Assert.AreEqual(5, JSMath.Round(4.9));

            Assert.AreEqual(-4, JSMath.Round(-4.1));
            Assert.AreEqual(-4, JSMath.Round(-4.4));
            Assert.AreEqual(-4, JSMath.Round(-4.5));
            Assert.AreEqual(-5, JSMath.Round(-4.6));
            Assert.AreEqual(-5, JSMath.Round(-4.9));
        }

        [TestMethod]
        public void Round()
        {
            Assert.AreEqual(4, Math.Round(4.1));
            Assert.AreEqual(4, Math.Round(4.4));
            Assert.AreEqual(4, Math.Round(4.5));
            Assert.AreEqual(5, Math.Round(4.6));
            Assert.AreEqual(5, Math.Round(4.9));

            Assert.AreEqual(-4, Math.Round(-4.1));
            Assert.AreEqual(-4, Math.Round(-4.4));
            Assert.AreEqual(-4, Math.Round(-4.5));
            Assert.AreEqual(-5, Math.Round(-4.6));
            Assert.AreEqual(-5, Math.Round(-4.9));
        }

        [TestMethod]
        public void Round_Midpoint_AwayFromZero()
        {
            Assert.AreEqual(34, Math.Round(34.1, MidpointRounding.AwayFromZero));
            Assert.AreEqual(34, Math.Round(34.4, MidpointRounding.AwayFromZero));
            Assert.AreEqual(35, Math.Round(34.5, MidpointRounding.AwayFromZero));
            Assert.AreEqual(35, Math.Round(34.6, MidpointRounding.AwayFromZero));
            Assert.AreEqual(35, Math.Round(34.9, MidpointRounding.AwayFromZero));

            Assert.AreEqual(-34, Math.Round(-34.1, MidpointRounding.AwayFromZero));
            Assert.AreEqual(-34, Math.Round(-34.4, MidpointRounding.AwayFromZero));
            Assert.AreEqual(-35, Math.Round(-34.5, MidpointRounding.AwayFromZero));
            Assert.AreEqual(-35, Math.Round(-34.6, MidpointRounding.AwayFromZero));
            Assert.AreEqual(-35, Math.Round(-34.9, MidpointRounding.AwayFromZero));
        }

        [TestMethod]
        public void Round_Midpoint_ToEven()
        {
            Assert.AreEqual(34, Math.Round(34.1, MidpointRounding.ToEven));
            Assert.AreEqual(34, Math.Round(34.4, MidpointRounding.ToEven));
            Assert.AreEqual(34, Math.Round(34.5, MidpointRounding.ToEven));
            Assert.AreEqual(35, Math.Round(34.6, MidpointRounding.ToEven));
            Assert.AreEqual(35, Math.Round(34.9, MidpointRounding.ToEven));

            Assert.AreEqual(-34, Math.Round(-34.1, MidpointRounding.ToEven));
            Assert.AreEqual(-34, Math.Round(-34.4, MidpointRounding.ToEven));
            Assert.AreEqual(-34, Math.Round(-34.5, MidpointRounding.ToEven));
            Assert.AreEqual(-35, Math.Round(-34.6, MidpointRounding.ToEven));
            Assert.AreEqual(-35, Math.Round(-34.9, MidpointRounding.ToEven));
        }

        [TestMethod]
        public void Round_Midpoint_Decimals_AwayFromZero()
        {
            Assert.AreEqual(3.5, Math.Round(3.45, 1, MidpointRounding.AwayFromZero));
            Assert.AreEqual(-3.5, Math.Round(-3.45, 1, MidpointRounding.AwayFromZero));
        }

        [TestMethod]
        public void Round_Midpoint_Decimals_ToEven()
        {
            Assert.AreEqual(3.4, Math.Round(3.45, 1, MidpointRounding.ToEven));
            Assert.AreEqual(-3.4, Math.Round(-3.45, 1, MidpointRounding.ToEven));
        }

        [TestMethod]
        public void Truncate()
        {
            Assert.AreEqual(1, Math.Truncate(1.1));
            Assert.AreEqual(1, Math.Truncate(1.4));
            Assert.AreEqual(1, Math.Truncate(1.5));
            Assert.AreEqual(1, Math.Truncate(1.6));
            Assert.AreEqual(1, Math.Truncate(1.9));

            Assert.AreEqual(-1, Math.Truncate(-1.1));
            Assert.AreEqual(-1, Math.Truncate(-1.4));
            Assert.AreEqual(-1, Math.Truncate(-1.5));
            Assert.AreEqual(-1, Math.Truncate(-1.6));
            Assert.AreEqual(-1, Math.Truncate(-1.9));
        }
    }
}
