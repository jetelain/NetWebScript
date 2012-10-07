using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace NetWebScript.UnitTestFramework.Equivalent
{
    [ScriptAvailable, ScriptEquivalent(typeof(Microsoft.VisualStudio.TestTools.UnitTesting.CollectionAssert))]
    public static class CollectionAssertEquiv
    {
        public static void AreEqual(ICollection expected, ICollection actual)
        {
            //Assert.AreEqual(expected.Count, actual.Count, "Count does not match");
            var enumA = expected.GetEnumerator();
            var enumB = actual.GetEnumerator();
            int index = 0;
            while (enumA.MoveNext())
            {
                Assert.IsTrue(enumB.MoveNext(), "Expected one element at index " + index);
                Assert.AreEqual(enumA.Current, enumB.Current, "Index " + index);
                index++;
            }
            Assert.IsFalse(enumB.MoveNext(), "Expected no element at index " + index);
        }
    }
}
