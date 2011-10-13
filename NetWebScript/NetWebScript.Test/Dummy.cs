using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetWebScript.JsClr.AstBuilder;

namespace NetWebScript.Test
{
    [TestClass]
    public class Dummy
    {
        public static void DoWhile()
        {
            //int a = 0, b = 0, c = 0;
            //do
            //{
            //    a++;
            //    if (a < 5)
            //    {
            //        b++;
            //    }
            //    else
            //    {
            //        b += 2;
            //    }
            //    c++;
            //} while (a < 10);

            //Assert.AreEqual(10, a);
            //Assert.AreEqual(10, c);
            //Assert.AreEqual(16, b);
            int ok = 1;
            do
            {
                ok++;
            } while (ok < 1);
            Assert.AreEqual(2, ok);
            do
            {
                ok++;
            } while (ok < 10);
            Assert.AreEqual(10, ok);
        }
        
        [TestMethod]
        public void TestMethod1()
        {
            var ast = MethodAst.GetMethodAst(new Action(DoWhile).Method);
        }
    }
}
