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
            var compiler = new ModuleCompiler(new RemotingScriptSystem("a"), false, false);
            compiler.AddEntryPoint(typeof(RemoteA));

            var writer = new StringWriter();
            compiler.Write(writer);

            var metaWriter = new StringWriter();
            compiler.WriteMetadata(metaWriter);

            var typeMetadata = compiler.Metadata.Types.First(t => t.CRef == "T:NetWebScript.Test.Remoting.RemoteA");
            var ctor = typeMetadata.Methods.First(m => m.CRef == "M:NetWebScript.Test.Remoting.RemoteA.#ctor");
            var method = typeMetadata.Methods.First(m => m.CRef == "M:NetWebScript.Test.Remoting.RemoteA.Remote(NetWebScript.Test.Remoting.ClassA,System.String)");

            var invokerMetadata = compiler.Metadata.Types.First(t => t.CRef == "T:NetWebScript.Remoting.RemoteInvoker");
            var invoke = invokerMetadata.Methods.First(m => m.CRef == "M:NetWebScript.Remoting.RemoteInvoker.Invoke(System.String,System.String,System.Object,System.Object[])");
            

            StringAssert.Contains(writer.ToString(), string.Format(@"{0}=aIa('{0}');
{0}.prototype.{1}=function(){{
return this;
}};
{0}.prototype.{2}=function(a0,a1){{
{3}.{4}(""{0}"",""{2}"",this,[a0,a1]);
}};", typeMetadata.Name, ctor.Name, method.Name, invokerMetadata.Name, invoke.Name));

        }
    }
}
