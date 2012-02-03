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

        internal void WriteExports(IEnumerable<ScriptType> iEnumerable)
        {
            foreach (var type in iEnumerable.OrderBy(t => t.ExportNamespace))
            {
                WriteExports(type);
            }
        }

        private void WriteExports(ScriptType type)
        {
            if (pretty)
            {
                writer.WriteLine();
                writer.WriteLine();
                writer.WriteLine();
                writer.WriteLine("//### Exports of {0}", type.Type.FullName);
            }
            if (!string.IsNullOrEmpty(type.ExportNamespace))
            {
                EnsureNamespace(type.ExportNamespace);
                writer.Write("{0}.", type.ExportNamespace);
            }
            else
            {
                writer.Write("var ");
            }

            var ctor = type.ExportedConstructor;
            if (ctor != null)
            {
                var argsCount = ctor.Method.GetParameters().Length;
                writer.Write("{0}=function(", type.ExportName);
                for (int i = 0; i < argsCount; ++i) { if (i > 0) { writer.Write(','); } writer.Write("a{0}", i); }
                writer.Write("){{return new {0}().{1}(", type.TypeId, ctor.ImplId);
                for (int i = 0; i < argsCount; ++i) { if (i > 0) { writer.Write(','); } writer.Write("a{0}", i); }
                writer.WriteLine(");};");
            }
            else
            {
                writer.WriteLine("{0}={{}};", type.ExportName);
            }
            foreach (var method in type.MethodsToWrite.OfType<ScriptMethod>().Where(m => m.Method.IsStatic && m.Method.IsPublic))
            {
                if (!string.IsNullOrEmpty(type.ExportNamespace))
                {
                    writer.Write("{0}.", type.ExportNamespace);
                }
                writer.WriteLine("{0}.{1}={2}.{3};", type.ExportName, CaseToolkit.GetMemberName(type.ExportCaseConvention, method.Method.Name), type.TypeId, method.ImplId);
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
