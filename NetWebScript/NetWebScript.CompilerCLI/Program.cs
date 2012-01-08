using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using NetWebScript.JsClr.Compiler;
using NetWebScript.JsClr.Runtime;
using NetWebScript.Page;
using NetWebScript.Remoting.Serialization;

namespace NetWebScript.CompilerCLI
{
    class Program
    {
        static int Main(string[] args)
        {
            var report = new ConsoleReporter();
            bool interractive = true;
            bool success = false;
            try
            {
                Compiler compiler = new Compiler(report);
                if (!Parse(args, out interractive, compiler))
                {
                    return Usage();
                }
                success = compiler.Compile();
            }
            catch (Exception e)
            {
                report.Error(string.Format(" Unexpected '{0}' : {1}", e.GetType().Name, e.Message));
#if DEBUG
                Console.WriteLine(e.ToString());
#endif
            }
            if (!success)
            {
                Console.WriteLine("Failed: {0} error(s)", report.Errors);
            }
#if DEBUG
            if (interractive)
            {
                Console.ReadKey();
            }
#endif
            return 0;
        }

        private class ConsoleReporter : IErrorReporter
        {
            int errors;

            public void Error(string p)
            {
                Console.Error.WriteLine("Error: {0}", p);
                errors++;
            }

            public void Warning(string p)
            {
                Console.Error.WriteLine("Warning: {0}", p);
            }

            public void Info(string p)
            {
                Console.Error.WriteLine("Info: {0}", p);
            }

            public int Errors
            {
                get { return errors; }
            }

            public void Report(CompilerMessage message)
            {
                Console.Error.WriteLine(message.ToString());
            }
        }


        private static bool Parse(string[] args, out bool interactive, Compiler compiler)
        {
            interactive = false;

            if (args.Length == 0)
            {
                return false;
            }

            var e = args.ToList().GetEnumerator();
            while (e.MoveNext())
            {
                string option = e.Current;

                if (string.Equals(option, "/debug", StringComparison.OrdinalIgnoreCase))
                {
                    compiler.Debug = true;
                }
#if DEBUG
                else if (string.Equals(option, "/interactive", StringComparison.OrdinalIgnoreCase))
                {
                    interactive = true;
                }
#endif
                else if (string.Equals(option, "/pretty", StringComparison.OrdinalIgnoreCase))
                {
                    compiler.Pretty = true;
                }
                else if (string.Equals(option, "/add", StringComparison.OrdinalIgnoreCase))
                {
                    if (!e.MoveNext())
                    {
                        Console.Error.WriteLine("Error: Missing assembly filename.", option);
                        return false;
                    }
                    FileInfo info = new FileInfo(e.Current);
                    if (!info.Exists)
                    {
                        Console.Error.WriteLine("Error: File '{0}' not found.", info.FullName);
                        return false;
                    }
                    compiler.AssembliesPath.Add(info.FullName);
                }
                else if (string.Equals(option, "/name", StringComparison.OrdinalIgnoreCase))
                {
                    if (!e.MoveNext())
                    {
                        Console.Error.WriteLine("Error: Missing name.", option);
                        return false;
                    }
                    compiler.Name = e.Current;
                }
                else if (string.Equals(option, "/page", StringComparison.OrdinalIgnoreCase))
                {
                    if (!e.MoveNext())
                    {
                        Console.Error.WriteLine("Error: Missing page class name.", option);
                        return false;
                    }
                    compiler.Page = e.Current;
                }
                else if (string.Equals(option, "/path", StringComparison.OrdinalIgnoreCase))
                {
                    if (!e.MoveNext())
                    {
                        Console.Error.WriteLine("Error: Missing path.", option);
                        return false;
                    }
                    var directory = new DirectoryInfo(e.Current);
                    if (!directory.Exists)
                    {
                        directory.Create();
                    }
                    compiler.OutputPath = directory.FullName;
                }
                else
                {
                    Console.Error.WriteLine("Error: Unknown option '{0}'", option);
                    return false;
                }
            }

            if (compiler.AssembliesPath.Count == 0)
            {
                Console.Error.WriteLine("Error: Supply at least one '/add' option.");
                return false;
            }

            if (compiler.Name == null)
            {
                if (compiler.AssembliesPath.Count == 1)
                {
                    compiler.Name = Path.GetFileNameWithoutExtension(compiler.AssembliesPath[0]);
                }
                else
                {
                    Console.Error.WriteLine("Error: Please specify '/name' option if you use multiple '/add' options.");
                    return false;
                }
            }

            if (compiler.OutputPath == null)
            {
                compiler.OutputPath = Directory.GetCurrentDirectory();
            }

            return true;
        }


        private static int Usage()
        {
            Console.Error.WriteLine("Usage: nwsc [/debug] [/pretty] [/path target directory] [/name scriptname] [/page class name] (/add assembly filename)+");
#if DEBUG
            Console.ReadKey();
#endif
            return 1;
        }

  
    }
}
