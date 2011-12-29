using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetWebScript.JsClr.AstBuilder;
using NetWebScript.Equivalents.Linq;
using System.Globalization;
using NetWebScript.Equivalents.Globalization;
using NetWebScript.Script;

namespace NetWebScript.Test
{
    [TestClass]
    public class Dummy
    {
        public static void SwitchString()
        {
            var a = new DateTime(2011, 12, 29, 13, 34, 49, 639, DateTimeKind.Local);
        }
        
        [TestMethod]
        public void TestMethod1()
        {
            //var culture = CultureInfo.InvariantCulture;
            var ast = MethodAst.GetMethodAst(new Func<JSString,int[],string,string>(NumberFormat.GroupIntegerNumber).Method);
            //var ast = MethodAst.GetMethodAst(new Func<NetWebScript.Script.JSString,System.Globalization.DateTimeFormatInfo,NetWebScript.Script.Date ,string>(NetWebScript.Equivalents.Globalization.DateTimeFormat.PartUTCTime).Method);
        }
    }
}
