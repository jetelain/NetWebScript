using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using NetWebScript.JsClr.Runtime;
using NetWebScript.Page;
using NetWebScript.Remoting.Serialization;
using System.Globalization;

namespace NetWebScript.JsClr.Compiler
{
    public sealed class Compiler
    {
        private readonly IErrorReporter reporter;

        public Compiler(IErrorReporter reporter)
        {
            this.reporter = reporter;
            AssembliesPath = new List<string>();
        }

        public bool Debug { get; set; }

        public bool Pretty { get; set; }

        public List<string> AssembliesPath { get; set; }

        public string Name { get; set; }

        public string OutputPath { get; set; }

        public string Page { get; set; }

        public Uri JQueryUri { get; set; }

        public bool Compile()
        {
            Type pageType = null;
            IScriptPageFactory factory = null;

            var assemblies = AssembliesPath.Select(n => Assembly.LoadFrom(n)).ToArray();

            var compiler = new ModuleCompiler(Debug, Pretty);
            var cultures = new CulturesCompiler();

            foreach (var assembly in assemblies)
            {
                compiler.AddAssembly(assembly);
                cultures.AddAssembly(assembly);
            }

            if (!string.IsNullOrEmpty(Page))
            {
                pageType = assemblies.Select(a => a.GetType(Page, false)).FirstOrDefault(t => t != null);
                if (pageType == null)
                {
                    reporter.Error(string.Format("Error: Type '{0}' not found.", Page));
                    return false;
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
                return false;
            }

            compiler.ModuleName = Name;
            compiler.ModuleFilename = Name + ".js";

            if (JQueryUri == null)
            {
                JQueryUri = new Uri(CoreRuntime.JQueryFilename, UriKind.RelativeOrAbsolute);

                using (var output = new FileStream(Path.Combine(OutputPath, CoreRuntime.JQueryFilename), FileMode.Create, FileAccess.Write))
                {
                    CoreRuntime.WriteJQuery(output);
                }
            }
            
            using (var writer = new StreamWriter(new FileStream(Path.Combine(OutputPath, Name + ".js"), FileMode.Create, FileAccess.Write)))
            {
                compiler.Write(writer);
            }

            cultures.WriteFiles(OutputPath, Name, compiler.Metadata);

            using (var writer = new StreamWriter(new FileStream(Path.Combine(OutputPath, Name + ".js.xml"), FileMode.Create, FileAccess.Write)))
            {
                compiler.WriteMetadata(writer);
            }

            if (pageType != null)
            {
                using (var writer = new StreamWriter(new FileStream(Path.Combine(OutputPath, Name + ".html"), FileMode.Create, FileAccess.Write)))
                {
                    WritePage(writer, Name, compiler, pageType, factory, cultures.Cultures);
                }
            }

            return true;
        }


        private bool ReportMessage(List<CompilerMessage> messages)
        {
            int errors = 0;
            foreach (var message in messages)
            {
                reporter.Report(message);
                if(message.Severity == MessageSeverity.Error)
                {
                    errors++;
                }
            }
            if (errors > 0)
            {
                return true;
            }
            return false;
        }

        private void WritePage(TextWriter writer, string name, ModuleCompiler compiler, Type scriptPageType, IScriptPageFactory factory, IEnumerable<CultureInfo> cultures)
        {

            writer.WriteLine("<!DOCTYPE html>");
            writer.WriteLine("<html>");
            writer.WriteLine("<head>");
            writer.WriteLine("<title>{0}</title>", name);
            writer.WriteLine("<script type=\"text/javascript\" src=\"{0}\"></script>", JQueryUri.OriginalString);
            writer.WriteLine("<script type=\"text/javascript\" src=\"{0}.js\"></script>", name);
            foreach (var culture in cultures)
            {
                writer.WriteLine("<script type=\"text/javascript\" src=\"{0}/{1}.js\"></script>", culture.Name, name);
            }
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
