using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript;

namespace NetWebScript.UnitTestFramework.Equivalent
{
    [ScriptAvailable, ScriptEquivalent(typeof(Microsoft.VisualStudio.TestTools.UnitTesting.Assert))]
    public static class Assert
    {
        public static void AreEqual<T>(T expected, T actual)
        {
            var a = (object)expected;
            var b = (object)actual;
            if (!object.Equals(a,b))
            {
                throw new Exception("failed. Exepected <"+expected+"> but was <"+actual+">");
            }
        }

        public static void AreEqual(object expected, object actual)
        {
            if (!object.Equals(expected, actual))
            {
                throw new Exception("failed. Exepected <" + expected + "> but was <" + actual + ">");
            }
        }

        public static void AreSame(object expected, object actual)
        {
            if (expected != actual)
            {
                throw new Exception("failed. Exepected <" + expected + "> but was <" + actual + ">");
            }
        }

        public static void IsNotNull(object value)
        {
            if (value == null)
            {
                throw new Exception("failed. is null");
            }
        }
        public static void IsFalse(bool value)
        {
            if (value == true)
            {
                throw new Exception("failed. is true");
            }
        }
        public static void IsFalse(bool value, string message)
        {
            if (value == true)
            {
                throw new Exception("failed. is true. " + message);
            }
        }

        public static void IsTrue(bool value)
        {
            if (value == false)
            {
                throw new Exception("failed. is false");
            }
        }
        public static void IsTrue(bool value, string message)
        {
            if (value == false)
            {
                throw new Exception("failed. is false. " + message);
            }
        }

        public static void Fail()
        {
            throw new Exception("failed");
        }

        public static void Fail(string message)
        {
            throw new Exception("failed. " + message);
        }

        public static void AreEqual(object expected, object actual, string message)
        {
            if (!object.Equals(expected, actual))
            {
                throw new Exception("failed. Exepected <" + expected + "> but was <" + actual + ">. " + message);
            }
        }
    }
}
