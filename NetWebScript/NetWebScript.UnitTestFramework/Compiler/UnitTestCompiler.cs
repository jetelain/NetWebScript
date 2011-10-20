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
using NetWebScript.UnitTestFramework.Client;
using NetWebScript.Remoting;
using NetWebScript.Remoting.Serialization;
using NetWebScript.JsClr.Runtime;

namespace NetWebScript.UnitTestFramework.Compiler
{
    public class UnitTestCompiler
    {
        private readonly ModuleCompiler compiler;
        private readonly List<Type> tests = new List<Type>();

        public UnitTestCompiler()
        {
            compiler = new ModuleCompiler();
            compiler.PrettyPrint = true;
            compiler.ImportAssemblyMappings(typeof(UnitTestCompiler).Assembly);
            compiler.AddEntryPoint(typeof(TestRunner));
            compiler.AddEntryPoint(typeof(RemoteInvoker));
            compiler.AddEntryPoint(typeof(TestRunnerPage));
        }

        public void AddTestsFrom(Assembly assembly)
        {
            foreach (var type in assembly.GetTypes())
            {
                if (Attribute.IsDefined(type, typeof(TestClassAttribute))
                    && Attribute.IsDefined(type, typeof(ScriptAvailableAttribute)))
                {
                    compiler.AddEntryPoint(type);
                    tests.Add(type);
                }
            }
        }

        public void Write(string path, string pagename)
        {
            using ( var writer = new StreamWriter(new FileStream(Path.Combine(path,pagename + ".js"), FileMode.Create, FileAccess.Write)))
            {
                CoreRuntime.WriteRuntime(writer, compiler.Debuggable);
                writer.WriteLine("NWS.$RegMod('{0}','0.0.0.0','{0}.js');", pagename);
                compiler.Write(writer);
            }
            using (var writer = new StreamWriter(new FileStream(Path.Combine(path, pagename + ".js.xml"), FileMode.Create, FileAccess.Write)))
            {
                compiler.WriteMetadata(writer);
            }
            using (var writer = new StreamWriter(new FileStream(Path.Combine(path, pagename + ".html"), FileMode.Create, FileAccess.Write)))
            {
                WritePage(writer, pagename);
            }
        }

        private void WritePage(TextWriter writer, string pagename)
        {
            var testsArray = tests.Select(t => new TestClassInfo()
            { 
                ctor = t.GetConstructor(Type.EmptyTypes), 
                name = t.Name, 
                methods = t
                    .GetMethods(BindingFlags.Instance|BindingFlags.Public)
                    .Where( m => m.GetParameters().Length == 0 && Attribute.IsDefined(m, typeof(TestMethodAttribute)))
                    .Select(m => new TestMethodInfo(){ method = m, name = m.Name})
                    .ToArray()
            }).ToArray();




            var cache = new SerializerCache(compiler.Metadata);
            var serializer = new EvaluableSerializer(cache);
            var page = new TestRunnerPage(testsArray);

            writer.WriteLine("<html>");
            writer.WriteLine("<head>");
            writer.WriteLine("<script type=\"text/javascript\" src=\"jquery-1.6.4.min.js\"></script>");
            writer.WriteLine("<script type=\"text/javascript\" src=\"{0}.js\"></script>", pagename);
            writer.WriteLine("<script type=\"text/javascript\">");
            writer.WriteLine("$(document).ready(function(){");
            serializer.Serialize(writer, new Action(page.Run));
            writer.WriteLine(".call(null);");
            writer.WriteLine("});");
            writer.WriteLine("</script>");
            writer.WriteLine("</head>");
            writer.WriteLine("<body>");
            writer.WriteLine("</body>");
            writer.WriteLine("</html>");
        }
    }
}
