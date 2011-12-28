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
            Options options = null;
            try
            {
                options = new Options();
                if (!Parse(args, options))
                {
                    return Usage();
                }
                return Execute(options);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Error: Unexpected '{0}' : {1}", e.GetType().Name, e.Message);
#if DEBUG
                if (options == null || options.interactive)
                {
                    Console.Error.WriteLine(e.ToString());
                    Console.ReadKey();
                }
#endif
                return 4;
            }
        }

        private class Options
        {
            public bool debug = false;
            public bool pretty = false;
#if DEBUG
            public bool interactive = false;
#endif
            public List<FileInfo> add = new List<FileInfo>();
            public string name = null;
            public DirectoryInfo path = null;
            public string page = null;
        }

        private static bool Parse(string[] args, Options options)
        {
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
                    options.debug = true;
                }
#if DEBUG
                else if (string.Equals(option, "/interactive", StringComparison.OrdinalIgnoreCase))
                {
                    options.interactive = true;
                }
#endif
                else if (string.Equals(option, "/pretty", StringComparison.OrdinalIgnoreCase))
                {
                    options.pretty = true;
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
                    options.add.Add(info);
                }
                else if (string.Equals(option, "/name", StringComparison.OrdinalIgnoreCase))
                {
                    if (!e.MoveNext())
                    {
                        Console.Error.WriteLine("Error: Missing name.", option);
                        return false;
                    }
                    options.name = e.Current;
                }
                else if (string.Equals(option, "/page", StringComparison.OrdinalIgnoreCase))
                {
                    if (!e.MoveNext())
                    {
                        Console.Error.WriteLine("Error: Missing page class name.", option);
                        return false;
                    }
                    options.page = e.Current;
                }
                else if (string.Equals(option, "/path", StringComparison.OrdinalIgnoreCase))
                {
                    if (!e.MoveNext())
                    {
                        Console.Error.WriteLine("Error: Missing path.", option);
                        return false;
                    }
                    options.path = new DirectoryInfo(e.Current);
                    if (!options.path.Exists)
                    {
                        options.path.Create();
                    }
                }
                else
                {
                    Console.Error.WriteLine("Error: Unknown option '{0}'", option);
                    return false;
                }
            }

            if (options.add.Count == 0)
            {
                Console.Error.WriteLine("Error: Supply at least one '/add' option.");
                return false;
            }

            if (options.name == null)
            {
                if (options.add.Count == 1)
                {
                    options.name = Path.GetFileNameWithoutExtension(options.add[0].Name);
                }
                else
                {
                    Console.Error.WriteLine("Error: Please specify '/name' option if you use multiple '/add' options.");
                    return false;
                }
            }

            if (options.path == null)
            {
                options.path = new DirectoryInfo(Directory.GetCurrentDirectory());
            }

            return true;
        }

        private static int Execute(Options options)
        {
            Console.WriteLine("NetWebScript Compiler");
            Type pageType = null;
            IScriptPageFactory factory = null;
            var assemblies = options.add.Select(n => Assembly.LoadFrom(n.FullName)).ToArray();

            var compiler = new ModuleCompiler();
            compiler.PrettyPrint = options.pretty;
            compiler.Debuggable = options.debug;

            foreach( var assembly in assemblies )
            {
                compiler.AddAssembly(assembly);
            }

            if (!string.IsNullOrEmpty(options.page))
            {
                pageType = assemblies.Select(a => a.GetType(options.page, false)).FirstOrDefault(t => t != null);
                if (pageType == null)
                {
                    Console.Error.WriteLine("Error: Type '{0}' not found.", options.page);
#if DEBUG
                    if (options.interactive)
                    {
                        Console.ReadKey();
                    }
#endif
                    return 5;
                }
                if (typeof(IScriptPageFactory).IsAssignableFrom(pageType))
                {
                    factory = (IScriptPageFactory)Activator.CreateInstance(pageType);
                    factory.Prepare(assemblies, compiler);
                }
                else
                {
                    compiler.AddEntryPoint(pageType);
                }
            }

            if (ReportMessage(compiler.GetMessages()))
            {
#if DEBUG
                if (options.interactive)
                {
                    Console.ReadKey();
                }
#endif
                return 3;
            }

            

            using (var writer = new StreamWriter(new FileStream(Path.Combine(options.path.FullName, CoreRuntime.JQueryFilename), FileMode.Create, FileAccess.Write)))
            {
                CoreRuntime.WriteJQuery(writer);
            }

            using (var writer = new StreamWriter(new FileStream(Path.Combine(options.path.FullName, options.name + ".js"), FileMode.Create, FileAccess.Write)))
            {
                CoreRuntime.WriteRuntime(writer, compiler.Debuggable);
                writer.WriteLine("NWS.$RegMod('{0}','0.0.0.0','{0}.js','{1}');", options.name, compiler.Metadata.Timestamp);
                compiler.Write(writer);
                if (compiler.Debuggable)
                {
                    writer.WriteLine("$(document).ready(function(){");
                    if (compiler.Debuggable)
                    {
                        writer.WriteLine("$dbgStart();");
                    }
                    writer.WriteLine("});");
                }
            }

            using (var writer = new StreamWriter(new FileStream(Path.Combine(options.path.FullName, options.name + ".js.xml"), FileMode.Create, FileAccess.Write)))
            {
                compiler.WriteMetadata(writer);
            }

            if (pageType != null)
            {
                using (var writer = new StreamWriter(new FileStream(Path.Combine(options.path.FullName, options.name + ".html"), FileMode.Create, FileAccess.Write)))
                {
                    WritePage(writer, options.name, compiler, pageType, factory);
                }
            }

            Console.WriteLine("Success.");
#if DEBUG
            if (options.interactive)
            {
                Console.ReadKey();
            }
#endif
            return 0;
        }

        private static bool ReportMessage(List<CompilerMessage> messages)
        {
            int errors = 0;
            foreach (var message in messages)
            {
                Console.WriteLine(message.ToString());
                if (message.Severity == JsClr.Compiler.MessageSeverity.Error)
                {
                    errors++;
                }
            }
            Console.WriteLine("{0} error(s).", errors);
            if (errors > 0)
            {
                return true;
            }
            return false;
        }

        private static int Usage()
        {
            Console.Error.WriteLine("Usage: nwsc [/debug] [/pretty] [/path target directory] [/name scriptname] [/page class name] (/add assembly filename)+");
#if DEBUG
            Console.ReadKey();
#endif
            
            
            return 1;
        }

        private static void WritePage(TextWriter writer, string name, ModuleCompiler compiler, Type scriptPageType, IScriptPageFactory factory)
        {

            writer.WriteLine("<!DOCTYPE html>");
            writer.WriteLine("<html>");
            writer.WriteLine("<head>");
            writer.WriteLine("<title>{0}</title>", name);
            writer.WriteLine("<script type=\"text/javascript\" src=\"{0}\"></script>", CoreRuntime.JQueryFilename);
            writer.WriteLine("<script type=\"text/javascript\" src=\"{0}.js\"></script>", name);
            writer.WriteLine("<script type=\"text/javascript\">");
            writer.WriteLine("$(document).ready(function(){");
            if (factory != null)
            {
                var page = factory.CreatePage();
                var cache = new SerializerCache(compiler.Metadata);
                var serializer = new EvaluableSerializer(cache);
                serializer.Serialize(writer, new Action(page.OnLoad));
                writer.WriteLine(".call(null);");
            }
            else
            {
                var ctor = compiler.AddEntryPoint(scriptPageType).GetScriptConstructor(scriptPageType.GetConstructor(Type.EmptyTypes));
                var method = compiler.AddEntryPoint(typeof(IScriptPage)).GetScriptMethod(typeof(IScriptPage).GetMethod("OnLoad"));
                writer.WriteLine("new {0}().{1}().{2}();", ctor.Owner.TypeId, ctor.ImplId, method.SlodId);
            }
            writer.WriteLine("});");
            writer.WriteLine("</script>");
            writer.WriteLine("</head>");
            writer.WriteLine("<body>");
            writer.WriteLine("</body>");
            writer.WriteLine("</html>");
        }

    }
}
