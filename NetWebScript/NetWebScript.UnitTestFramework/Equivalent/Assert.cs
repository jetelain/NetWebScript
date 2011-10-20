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
            if (a != b) // FIXME: should use object.Equals(a,b), for now works by accident as we tests only use method on primitives types
            {
                throw new Exception("failed");
            }
        }

        public static void IsNotNull(object value)
        {
            if (value == null)
            {
                throw new Exception("failed");
            }
        }
        
    }
}
