using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Reflection;
using NetWebScript.JsClr.JsBuilder.JsSyntax;
using NetWebScript.JsClr.TypeSystem;
using NetWebScript.JsClr.TypeSystem.Imported;
using NetWebScript.JsClr.TypeSystem.Standard;
using NetWebScript.Metadata;
using NetWebScript.Script;
using NetWebScript.JsClr.ScriptWriter;
using NetWebScript.JsClr.ScriptWriter.Declaration;
using NetWebScript.JsClr.ScriptAst;

namespace NetWebScript.JsClr.Compiler
{
    class ModuleWriter
    {
        private readonly TextWriter writer;
        private readonly ScriptSystem system;
        private readonly bool pretty;
        private readonly IScriptMethodBase createTypeMethod;
        private readonly Instrumentation instrumentation;
        private readonly TypeDeclarationWriter declarationWriter;
        private readonly WriterContext context;

        public ModuleWriter(TextWriter writer, ScriptSystem system, bool pretty, Instrumentation instrumentation)
        {
            declarationWriter = new TypeDeclarationWriter(system);

            context = new WriterContext() 
            { 
                Instrumentation = instrumentation, 
                IsPretty = pretty, 
                StandardDeclarationWriter = declarationWriter 
            };


            this.writer = writer;
            this.system = system;
            this.pretty = pretty;
            this.instrumentation = instrumentation;
            createTypeMethod = system.GetScriptMethod(new Func<string, Type, Type[], Type>(TypeSystemHelper.CreateType).Method);
        }

        internal void WriteType(IScriptTypeDeclarationWriter declaration)
        {
            if (context.IsPretty)
            {
                writer.WriteLine();
                writer.WriteLine("//### Type {0}", declaration.PrettyName);
            }
            declaration.WriteDeclaration(writer, context);
        }

        internal void WriteExports(IEnumerable<ExportDefinition> iEnumerable)
        {
            foreach (var type in iEnumerable.OrderBy(t => t.ExportNamespace))
            {
                WriteExports(type);
            }
        }

        private void WriteExports(ExportDefinition export)
        {
            if (pretty)
            {
                writer.WriteLine();
                writer.WriteLine("//### Exports of {0}", export.PrettyName);
            }
            if (!string.IsNullOrEmpty(export.ExportNamespace))
            {
                EnsureNamespace(export.ExportNamespace);
                writer.Write("{0}.", export.ExportNamespace);
            }
            else
            {
                writer.Write("var ");
            }

            var ctor = export.ExportedConstructor;
            if (ctor != null)
            {
                var argsCount = ctor.Method.GetParameters().Length;
                var body = ctor.CreationInvoker.WriteObjectCreation(ctor, 
                    new ScriptObjectCreationExpression(
                        ctor,
                        Enumerable.
                            Range(0, argsCount).
                            Select(i => (ScriptExpression)new ScriptArgumentReferenceExpression(new ScriptArgument() { Index = i, Name = string.Format("a{0}", i) })).ToList()
                    ), 
                    new AstScriptWriter());

                writer.Write("{0}=function(", export.ExportName);
                for (int i = 0; i < argsCount; ++i) { if (i > 0) { writer.Write(','); } writer.Write("a{0}", i); }
                writer.Write("){return ");
                writer.Write(body);
                writer.WriteLine(";};");
            }
            else
            {
                writer.WriteLine("{0}={{}};", export.ExportName);
            }
            foreach (var method in export.ExportedStaticMethods)
            {
                if (!string.IsNullOrEmpty(export.ExportNamespace))
                {
                    writer.Write("{0}.", export.ExportNamespace);
                }
                writer.WriteLine("{0}.{1}={2};",
                    export.ExportName, CaseToolkit.GetMemberName(export.ExportCaseConvention, method.Method.Name),
                    method.Invoker.WriteMethodReference(method).Text);
            }
        }

        private readonly HashSet<string> namespaces = new HashSet<string>();
        private void EnsureNamespace(string p)
        {
            if (!namespaces.Contains(p))
            {
                int i = p.LastIndexOf('.');
                if (i != -1)
                {
                    EnsureNamespace(p.Substring(0, i));
                    writer.WriteLine("{0}={{}};", p);
                }
                else
                {
                    writer.WriteLine("var {0}={{}};", p);
                }
                namespaces.Add(p);
            }
        }

    }
}
