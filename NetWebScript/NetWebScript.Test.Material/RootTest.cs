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
            compiler.Compile(typeof(RootTest).Assembly);
            StringWriter writer = new StringWriter();
            compiler.Write(writer);
            var result = writer.ToString();

        }
    }
}
