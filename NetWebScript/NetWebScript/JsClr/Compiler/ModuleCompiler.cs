using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using NetWebScript.JsClr.AstBuilder;
using NetWebScript.JsClr.AstBuilder.PdbInfo;
using NetWebScript.JsClr.TypeSystem;
using NetWebScript.JsClr.TypeSystem.Standard;
using NetWebScript.Metadata;
using NetWebScript.Remoting.Serialization;
using NetWebScript.Script;
using NetWebScript.Page;

namespace NetWebScript.JsClr.Compiler
{
    /// <summary>
    /// IL to JavaScript compiler (using System.Reflection)
    /// </summary>
    public sealed class ModuleCompiler : IScriptDependencies
    {
        private readonly ScriptSystem system;
        private readonly RuntimeAstFilter runtimeFilter;
        private readonly List<InternalMessage> messages = new List<InternalMessage>();
        private readonly List<Type> typesToBeExported = new List<Type>();
        private readonly HashSet<Assembly> assemblies = new HashSet<Assembly>();
        private int index;
        private int indexMethods;

        /// <summary>
        /// Instrument generated script to be debuggable
        /// </summary>
        public bool Debuggable { get; set; }

        /// <summary>
        /// Indent and add some commands on generated script to be human freindly
        /// </summary>
        public bool PrettyPrint { get; set; }

        /// <summary>
        /// Metadata of generated script
        /// </summary>
        public ModuleMetadata Metadata { get; set; }

        /// <summary>
        /// Creates a compiler using default script system.
        /// </summary>
        public ModuleCompiler() : this(new ScriptSystem())
        {

        }

        /// <summary>
        /// Created a compiler using a custom script system
        /// </summary>
        /// <param name="system">Script system to use</param>
        public ModuleCompiler(ScriptSystem system)
        {
            this.system = system;
            runtimeFilter = new RuntimeAstFilter(system, messages);
            Metadata = new ModuleMetadata();

            AddAssemblyPrivate(typeof(TypeSystemHelper).Assembly);
            AddEntryPoint(typeof(TypeSystemHelper));
        }

        /// <summary>
        /// Add a type to generated script.
        /// All referenced assemblies (including type.Assembly) will be added.
        /// </summary>
        /// <param name="type">Type to add</param>
        /// <returns>Script version of type</returns>
        public IScriptType AddEntryPoint(Type type)
        {
            if (!assemblies.Contains(type.Assembly))
            {
                AddAssemblyPrivate(type.Assembly);
            }
            var scriptType = system.GetScriptType(type);
            if (scriptType == null)
            {
                throw new Exception(String.Format("{0} is not available in script",type));
            }
            GenerateAst();
            EnsureExports();
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

        /// <summary>
        /// Procude all compilation messages, including errors and warnings, with original source code references.
        /// </summary>
        /// <returns>Compilation messages</returns>
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

        /// <summary>
        /// Write script to the provided writer.
        /// </summary>
        /// <param name="writer">Target</param>
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



        private void AddAssemblyPrivate(Assembly assembly)
        {
            if (assemblies.Contains(assembly))
            {
                return;
            }
            assemblies.Add(assembly);
            foreach (var dependency in assembly.GetReferencedAssemblies())
            {
                AddAssemblyPrivate(Assembly.Load(dependency));
            }
            system.ImportAssemblyMappings(assembly);
            typesToBeExported.AddRange(assembly.GetTypes().Where(t => Attribute.IsDefined(t, typeof(ScriptAvailableAttribute)) && Attribute.IsDefined(t, typeof(ExportedAttribute))));
        }

        /// <summary>
        /// Add an assembly yo generated script. This imply to add all exported types of the assembly.
        /// All referenced assemblies will be added too.
        /// </summary>
        /// <param name="assembly"></param>
        public void AddAssembly(Assembly assembly)
        {
            AddAssemblyPrivate(assembly);
            EnsureExports();
        }

        private void EnsureExports()
        {
            while (typesToBeExported.Count > 0)
            {
                var lastIdx = typesToBeExported.Count - 1;
                system.GetScriptType(typesToBeExported[lastIdx]);
                typesToBeExported.RemoveAt(lastIdx);
            }
        }

        void IScriptDependencies.AddType(Type info)
        {
            AddEntryPoint(info);
        }
    }
}
