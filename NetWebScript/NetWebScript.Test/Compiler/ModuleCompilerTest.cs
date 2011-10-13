using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetWebScript.JsClr.Compiler;
using NetWebScript;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;

namespace NetWebScript.Test.Compiler
{
    [TestClass]
    public class ModuleCompilerTest
    {
        /*[ScriptAvailable]
        private class SimpleClass
        {
            public int Test01( int a )
            {
                return a + 1;
            }
        }*/

        private Type CreateSimpleClass()
        {
            lock (Tester.Lock)
            {
                var type = Tester.CreateTestType(TypeAttributes.Class, typeof(object));
                type.SetCustomAttribute(new CustomAttributeBuilder(typeof(ScriptAvailableAttribute).GetConstructor(Type.EmptyTypes), new object[] { }));
                type.DefineDefaultConstructor(MethodAttributes.Public);
                var method = type.DefineMethod("Test01", MethodAttributes.Public, typeof(int), new Type[] { typeof(int) });
                method.DefineParameter(1, ParameterAttributes.None, "a");
                var il = method.GetILGenerator();
                il.Emit(OpCodes.Ldarg_1);
                il.Emit(OpCodes.Ldc_I4_1);
                il.Emit(OpCodes.Add);
                il.Emit(OpCodes.Ret);
                return type.CreateType();
            }
        }


        [TestMethod]
        public void Compile_SimpleClass()
        {
            var compiler = new ModuleCompiler();
            var type = CreateSimpleClass();
            compiler.AddEntryPoint(type);

            var writer = new StringWriter();
            compiler.Write(writer);

            var metaWriter = new StringWriter();
            compiler.WriteMetadata(metaWriter);

            Assert.AreEqual(@"aIa=function (n, b, i) {
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
aTb.prototype.aIb=function(a0){
return a0+1;
};
", writer.ToString());

            Assert.AreEqual(@"<?xml version=""1.0"" encoding=""utf-16""?>
<Module xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" Name=""a"" xmlns=""http://codeplex.com/nws"">
  <Assembly>NetWebScript.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null</Assembly>
  <Assembly>Tester, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null</Assembly>
  <Type Name=""aTa"" BaseTypeName=""Object"" CRef=""T:NetWebScript.Script.TypeSystemHelper"">
    <Method Name=""aIa"" CRef=""M:NetWebScript.Script.TypeSystemHelper.CreateType(System.String,System.Type,System.Type[])"" />
  </Type>
  <Type Name=""aTb"" BaseTypeName=""Object"" CRef=""T:" + type.Name + @""">
    <Method Name=""aIc"" CRef=""M:" + type.Name + @".#ctor"" />
    <Method Name=""aIb"" CRef=""M:" + type.Name + @".Test01(System.Int32)"" />
  </Type>
</Module>", metaWriter.ToString());
        }

        [ScriptAvailable]
        private class BodyTest
        {
            [ScriptBody(Body="function(){/*A*/}")]
            public void A()
            {
            }
            [ScriptBody(Body="function(){/*B*/}")]
            public static void B()
            {
            }
        }

        [TestMethod]
        public void Compile_ScriptBody()
        {
            var compiler = new ModuleCompiler();
            var type = CreateSimpleClass();
            compiler.AddEntryPoint(typeof(BodyTest));

            var writer = new StringWriter();
            compiler.Write(writer);

            var metaWriter = new StringWriter();
            compiler.WriteMetadata(metaWriter);

            Assert.AreEqual(@"aIa=function (n, b, i) {
var t = function () { }
t.$n = n;
if (b) { t.$b = b; t.prototype = new b; t.prototype.constructor = t; }
if (i) { t.prototype.$itfs = i; } 
return t;
};
aTb=aIa('aTb');
aTb.prototype.aId=function(){
return this;
};
aTb.prototype.aIb=function(){/*A*/};
aTb.aIc=function(){/*B*/};
", writer.ToString());

            Assert.AreEqual(@"<?xml version=""1.0"" encoding=""utf-16""?>
<Module xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" Name=""a"" xmlns=""http://codeplex.com/nws"">
  <Assembly>NetWebScript.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null</Assembly>
  <Assembly>NetWebScript.Test, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null</Assembly>
  <Type Name=""aTa"" BaseTypeName=""Object"" CRef=""T:NetWebScript.Script.TypeSystemHelper"">
    <Method Name=""aIa"" CRef=""M:NetWebScript.Script.TypeSystemHelper.CreateType(System.String,System.Type,System.Type[])"" />
  </Type>
  <Type Name=""aTb"" BaseTypeName=""Object"" CRef=""T:NetWebScript.Test.Compiler.ModuleCompilerTest.BodyTest"">
    <Method Name=""aId"" CRef=""M:NetWebScript.Test.Compiler.ModuleCompilerTest.BodyTest.#ctor"" />
    <Method Name=""aIb"" CRef=""M:NetWebScript.Test.Compiler.ModuleCompilerTest.BodyTest.A"" />
    <Method Name=""aIc"" CRef=""M:NetWebScript.Test.Compiler.ModuleCompilerTest.BodyTest.B"" />
  </Type>
</Module>", metaWriter.ToString());
        }
    }
}
