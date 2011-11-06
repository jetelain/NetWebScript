using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.JsClr.TypeSystem;
using NetWebScript.JsClr.TypeSystem.Standard;
using NetWebScript.JsClr.AstBuilder;
using System.IO;
using NetWebScript.Metadata;
using System.Reflection;
using System.Xml.Serialization;
using NetWebScript.JsClr.AstBuilder.PdbInfo;
using NetWebScript.Remoting.Serialization;

namespace NetWebScript.JsClr.Compiler
{
    public class ModuleCompiler
    {
        private readonly ScriptSystem system;
        private readonly RuntimeAstFilter runtimeFilter;
        private readonly List<InternalMessage> messages = new List<InternalMessage>();
        private int index;
        private int indexMethods;

        /// <summary>
        /// Instrument generated script to be debuggable
        /// </summary>
        public bool Debuggable { get; set; }

        public bool PrettyPrint { get; set; }

        public ModuleMetadata Metadata { get; set; }

        public ModuleCompiler() : this(new ScriptSystem())
        {

        }

        public ModuleCompiler(ScriptSystem system)
        {
            this.system = system;
            runtimeFilter = new RuntimeAstFilter(system, messages);
            Metadata = new ModuleMetadata();
        }

        public IScriptType AddEntryPoint(Type type)
        {
            var scriptType = system.GetScriptType(type);
            if (scriptType == null)
            {
                throw new Exception(String.Format("{0} is not available in script",type));
            }
            GenerateAst();
            return scriptType;
        }

        private string BuildReport()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("Compilation to JavaScript FAILED.");
            foreach( var error in GetMessages() )
            {
                builder.AppendLine(error.ToString());
            }
            return builder.ToString();
        }

        private void GenerateAst()
        {
            while (index < system.TypesToGenerate.Count || indexMethods < system.MethodsToGenerate.Count)
            {
                while (index < system.TypesToGenerate.Count)
                {
                    GenerateAst(system.TypesToGenerate[index]);
                    index++;
                }
                while (indexMethods < system.MethodsToGenerate.Count)
                {
                    GenerateAst(system.MethodsToGenerate[indexMethods]);
                    indexMethods++;
                }
            }
        }

        private void GenerateAst(ScriptType type)
        {
            foreach (var method in type.Methods)
            {
                GenerateAst(method);
            }
        }

        private void GenerateAst(ScriptMethodBase method)
        {
            if (!method.Method.IsAbstract && !method.HasNativeBody)
            {
                try
                {
                    method.Ast = system.GetMethodAst(method, method.Method);
                    runtimeFilter.Visit(method.Ast);
                }
                catch (AstBuilderException e)
                {
                    messages.Add(new InternalMessage() { Severity = MessageSeverity.Error, Method = method.Method, IlOffset = e.IlOffset, Message = e.Message });
                }
                catch (Exception e)
                {
                    messages.Add(new InternalMessage() { Severity = MessageSeverity.Error, Method = method.Method, Message = e.ToString() });
                }
            }
        }

        public List<CompilerMessage> GetMessages()
        {
            var list = new List<CompilerMessage>(messages.Count);

            foreach (var error in messages)
            {
                PdbSequencePoint point = null;
                if (error.IlOffset != null)
                {
                    PdbMethod pdb = PdbCatalog.GetPdbMethod(error.Method);
                    if (pdb != null)
                    {
                        int offset = error.IlOffset.Value;
                        point = pdb.SequencePointList.LastOrDefault(p => p.Offset <= offset);
                    }
                }
                var publicMessage = new CompilerMessage();
                publicMessage.Severity = error.Severity;
                if (point != null && !string.IsNullOrEmpty(error.Message))
                {
                    publicMessage.Message = error.Message;
                    publicMessage.Filename = point.Filename;
                    publicMessage.LineNumber = point.StartRow;
                    publicMessage.LinePosition = point.StartCol;
                }
                else
                {
                    publicMessage.Message = string.Format("In method '{0}': {1}", error.Method, error.Message);
                }
                list.Add(publicMessage);
            }
            return list;
        }

        public void Write(TextWriter writer)
        {
            if (messages.Count(e => e.Severity == MessageSeverity.Error) > 0)
            {
                throw new Exception(BuildReport());
            }
            Metadata.Assemblies.Clear();
            Metadata.Documents.Clear();
            Metadata.Types.Clear();
            Metadata.Name = system.ModuleId;
            HashSet<Assembly> assemblies = new HashSet<Assembly>();
            var moduleWriter = new ModuleWriter(writer, system, Debuggable, PrettyPrint);
            foreach (var type in system.TypesToGenerate.ToArray())
            {
                assemblies.Add(type.Type.Assembly);
                moduleWriter.WriteType(Metadata, type);
                SanityCheck(type);
            }
            foreach (var type in system.EnumToGenerate.ToArray())
            {
                assemblies.Add(type.Type.Assembly);
                moduleWriter.WriteEnumType(Metadata, type);
                SanityCheck(type);
            }
            foreach (var type in system.ImportedTypes.ToArray())
            {
                if (type.ExtensionMethods.Count > 0)
                {
                    moduleWriter.WriteTypeExtensions(Metadata, type);
                    SanityCheck(type);
                }
            }

            moduleWriter.WriteExports(system.TypesToGenerate.Where(t => t.IsExported));
            
            foreach (var type in system.Equivalents)
            {
                Metadata.Equivalents.Add(new EquivalentMetadata() { CRef = CRefToolkit.GetCRef(type.Type), EquivalentCRef = CRefToolkit.GetCRef(type.Equivalent.Type) });
            }
            Metadata.Assemblies.AddRange(assemblies.Select(a => a.FullName));
            MetadataProvider.Current = new MetadataProvider(Metadata);
        }

        private void SanityCheck(IScriptType previous)
        {
            if (index != system.TypesToGenerate.Count)
            {
                throw new InvalidOperationException(string.Format("Type '{0}' have been discovered while writing '{1}' !", system.TypesToGenerate[index].Type.FullName, previous.Type.FullName));
            }
        }

        public void WriteMetadata(TextWriter writer)
        {
            ModuleMetadataSerializer.Write(writer, Metadata);
        }

        public void ImportAssemblyMappings(Assembly assembly)
        {
            system.ImportAssemblyMappings(assembly);
        }
    }
}
