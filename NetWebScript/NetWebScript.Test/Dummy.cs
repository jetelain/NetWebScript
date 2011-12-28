using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetWebScript.JsClr.AstBuilder;
using NetWebScript.Equivalents.Linq;

namespace NetWebScript.Test
{
    [TestClass]
    public class Dummy
    {
        public static void SwitchString()
        {
            int ok = 0;
            int ko = 0;
            String a = "1";
            switch (a)
            {
                case "0": ko++; break;
                case "1": ok++; break;
                case "2": ko++; break;
                case "3": ko++; break;
                case "4": ko++; break;
                case "5": ko++; break;
                case "6": ko++; break;
                case "7": ko++; break;
                case "8": ko++; break;
                case "9": ko++; break;
                case "10": ko++; break;
                default: ko++; break;
            }
        }
        
        [TestMethod]
        public void TestMethod1()
        {
            //var ast = MethodAst.GetMethodAst(new Action(SwitchString).Method);
            var ast = MethodAst.GetMethodAst(new Func<NetWebScript.Script.JSString,System.Globalization.DateTimeFormatInfo,NetWebScript.Script.Date ,string>(NetWebScript.Equivalents.Globalization.DateTimeFormat.PartUTCTime).Method);
        }
    }
}
