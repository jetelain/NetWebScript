using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetWebScript.JsClr.Compiler;
using NetWebScript.Remoting;
using System.IO;

namespace NetWebScript.Test.Remoting
{
    [TestClass]
    public class RemotingCompilerTest
    {
        [TestMethod]
        public void RemotingCompile_RemoteMethod()
        {
            var compiler = new ModuleCompiler(new RemotingScriptSystem("a"));
            compiler.AddEntryPoint(typeof(RemoteA));

            var writer = new StringWriter();
            compiler.Write(writer);

            var metaWriter = new StringWriter();
            compiler.WriteMetadata(metaWriter);

            StringAssert.StartsWith(writer.ToString(), @"aIa=function (n, b, i) {
var t = function () { }
t.$n = n;
if (b) { t.$b = b; t.prototype = new b; t.prototype.constructor = t; }
if (i) { t.prototype.$itfs = i; } 
return t;
};
aTb=aIa('aTb');
aTb.prototype.aIc=function(){
return this;
};
aTb.prototype.aIb=function(a0,a1){
aTe.aIf(""aTb"",""aIb"",this,[a0,a1]);
};
");
        }
    }
}
