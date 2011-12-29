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

            var typeMetadata = compiler.Metadata.Types.First(t => t.CRef == "T:" + type.Name);
            var ctor = typeMetadata.Methods.First(m => m.CRef == "M:" + type.Name + @".#ctor");
            var method = typeMetadata.Methods.First(m => m.CRef == "M:" + type.Name + @".Test01(System.Int32)");

            StringAssert.Contains(writer.ToString(), string.Format(@"{0}=aIa('{0}');
{0}.prototype.{1}=function(){{
return this;
}};
{0}.prototype.{2}=function(a0){{
return a0+1;
}};", typeMetadata.Name, ctor.Name, method.Name));

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
            compiler.AddEntryPoint(typeof(BodyTest));

            var writer = new StringWriter();
            compiler.Write(writer);

            var metaWriter = new StringWriter();
            compiler.WriteMetadata(metaWriter);

            var typeMetadata = compiler.Metadata.Types.First(t => t.CRef == "T:NetWebScript.Test.Compiler.ModuleCompilerTest.BodyTest");
            var ctor = typeMetadata.Methods.First(m => m.CRef == "M:NetWebScript.Test.Compiler.ModuleCompilerTest.BodyTest.#ctor");
            var methodA = typeMetadata.Methods.First(m => m.CRef == "M:NetWebScript.Test.Compiler.ModuleCompilerTest.BodyTest.A");
            var methodB = typeMetadata.Methods.First(m => m.CRef == "M:NetWebScript.Test.Compiler.ModuleCompilerTest.BodyTest.B");

            StringAssert.Contains(writer.ToString(), string.Format(@"{0}=aIa('{0}');
{0}.prototype.{1}=function(){{
return this;
}};
{0}.prototype.{2}=function(){{/*A*/}};
{0}.{3}=function(){{/*B*/}};", typeMetadata.Name, ctor.Name, methodA.Name, methodB.Name));

        }
    }
}
