using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using NetWebScript.UnitTestFramework.Compiler;

namespace NetWebScript.TestCompiler
{
    class Program
    {
        static int Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Error: Usage: NetWebScript.TestCompiler.exe testassembly.dll");
                return 1;
            }
            var file = new FileInfo(args[0]);
            if (!file.Exists)
            {
                Console.Error.WriteLine("Error: File '{0}' does not exists.", args[0]);
                return 2;
            }
            try
            {
                var targetPath = new DirectoryInfo(Path.Combine(file.Directory.FullName, "netwebscript"));
                if (!targetPath.Exists)
                {
                    targetPath.Create();
                }
                
                var assembly = Assembly.LoadFrom(file.FullName);
                var compiler = new UnitTestCompiler();
                compiler.AddTestsFrom(assembly);

                var messages = compiler.GetMessages();
                int errors = 0;
                foreach (var message in messages)
                {
                    Console.WriteLine(message.ToString());
                    if (message.Severity == JsClr.Compiler.MessageSeverity.Error)
                    {
                        errors++;
                    }
                }

                if (errors > 0)
                {
                    return 4;
                }
                compiler.Write(targetPath.FullName, Path.GetFileNameWithoutExtension(file.Name));
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Error: {0}", e.ToString());
                return 3;
            }
            return 0;
        }
    }
}
