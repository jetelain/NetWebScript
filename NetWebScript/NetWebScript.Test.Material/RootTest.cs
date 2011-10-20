using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.UnitTestFramework.Compiler;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NetWebScript.Test.Material
{
    [TestClass]
    public class RootTest
    {
        [TestMethod]
        public void Root()
        {
            var compiler = new UnitTestCompiler();
            compiler.AddTestsFrom(typeof(RootTest).Assembly);
            compiler.Write(@"C:\Users\Julien\Test","Tests");
        }
    }
}
