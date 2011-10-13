using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using NetWebScript.JsClr.Compiler;
using NetWebScript.Test.Client;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetWebScript;

namespace NetWebScript.UnitTestFramework.Compiler
{
    public class UnitTestCompiler
    {
        private readonly ModuleCompiler compiler;

        public UnitTestCompiler()
        {
            compiler = new ModuleCompiler();
            compiler.ImportAssemblyMappings(typeof(UnitTestCompiler).Assembly);
            compiler.AddEntryPoint(typeof(Channel));
        }

        public void Compile(Assembly assembly)
        {
            foreach (var type in assembly.GetTypes())
            {
                if (Attribute.IsDefined(type, typeof(TestClassAttribute))
                    && Attribute.IsDefined(type, typeof(ScriptAvailableAttribute)))
                {
                    compiler.AddEntryPoint(type);
                }
            }
        }

        public void Write(TextWriter writer)
        {
            compiler.Write(writer);
        }
    }
}
