﻿using System;
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
using NetWebScript.Page;
using NetWebScript.Remoting.Serialization;
using NetWebScript.Script;

namespace NetWebScript.JsClr.Compiler
{
    /// <summary>
    /// IL to JavaScript compiler (using System.Reflection)
    /// </summary>
    public sealed class ModuleCompiler : IScriptDependencies
    {
        internal static string NetWebScriptVersion
        {
            get 
            {
                var attr = (AssemblyFileVersionAttribute)Attribute.GetCustomAttribute(typeof(ModuleCompiler).Assembly, typeof(AssemblyFileVersionAttribute));
                return attr.Version;
            }
        }

        private readonly ScriptSystem system;
        private readonly RuntimeAstFilter runtimeFilter;
        private readonly List<InternalMessage> messages = new List<InternalMessage>();
        private readonly List<Type> typesToBeExported = new List<Type>();
        private readonly HashSet<Assembly> assemblies = new HashSet<Assembly>();
        private readonly Instrumentation instrumentation;

        /// <summary>
        /// Instrument generated script to be debuggable
        /// </summary>
        public Instrumentation Instrumentation { get { return instrumentation; } }

        /// <summary>
        /// Indent and add some commands on generated script to be human freindly
        /// </summary>
        public bool PrettyPrint { get; private set; }

        /// <summary>
        /// Metadata of generated script
        /// </summary>
        public ModuleMetadata Metadata { get { return system.Metadata; } }

        /// <summary>
        /// Module full name (source assembly name)
        /// </summary>
        public string ModuleName { get; set; }

        /// <summary>
        /// Module file name (target javascript file)
        /// </summary>
        public string ModuleFilename { get; set; }

        /// <summary>
        /// Creates a compiler using default script system.
        /// </summary>
        public ModuleCompiler(bool debuggable, bool prettyPrint)
            : this(new ScriptSystem(), debuggable, prettyPrint)
        {

        }

        /// <summary>
        /// Created a compiler using a custom script system
        /// </summary>
        /// <param name="system">Script system to use</param>
        public ModuleCompiler(ScriptSystem system, bool debuggable, bool prettyPrint)
        {
            this.system = system;
            runtimeFilter = new RuntimeAstFilter(system, messages);
            Metadata.Timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");

            AddAssemblyPrivate(typeof(TypeSystemHelper).Assembly);
            AddEntryPoint(typeof(TypeSystemHelper));
            AddEntryPoint(typeof(Unsafe));

            if (debuggable)
            {
                instrumentation = new Instrumentation(this, typeof(NetWebScript.Diagnostics.Debugger));
                PrettyPrint = true;
            }
            else if (prettyPrint)
            {
                PrettyPrint = true;
            }
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
            ScriptTypeHelper.EnsureAllPublicMembers(scriptType);
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
            while (system.AstToGenerate.Count > 0)
            {
                GenerateAst(system.AstToGenerate.Dequeue());
            }
        }

        private void GenerateAst(ScriptMethodBase method)
        {
            if (!method.Method.IsAbstract && !method.HasNativeBody)
            {
                try
                {
                    var netAst = MethodAst.GetMethodAst(method.Method);
                    method.Ast = runtimeFilter.Visit(netAst);
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
                    publicMessage.EndLineNumber = point.EndRow;
                    publicMessage.EndLinePosition = point.EndCol;
                }
                else
                {
                    publicMessage.Message = string.Format("In method '{0}': {1}", error.Method, error.Message);
                }
                list.Add(publicMessage);
            }
            return list;
        }



        private void WriteTypes(TextWriter writer)
        {
            var exports = system.Exports.ToList();

            var moduleWriter = new ModuleWriter(writer, system, PrettyPrint, instrumentation);

            foreach (var type in system.TypesToDeclare.ToArray())
            {
                moduleWriter.WriteType(type);
                SanityCheck(type.PrettyName);
            }

            moduleWriter.WriteExports(exports);
        }

        private void WriteStaticCtorsCalls(TextWriter writer)
        {
            if (PrettyPrint)
            {
                writer.WriteLine();
                writer.WriteLine("// ### Static constructors");
            }
            foreach (var ctor in system.StaticConstructors.ToArray())
            {
                if (PrettyPrint)
                {
                    writer.WriteLine("// {0}", ctor.OwnerScriptType.Type.FullName);
                }
                writer.Write(ctor.Invoker.WriteMethodReference(ctor).Text);
                writer.WriteLine("();");
            }
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

            writer.WriteLine("// Generated by NetWebScript {0}", NetWebScriptVersion);

            system.Seal();

            WriteTypes(writer);

            WriteStaticCtorsCalls(writer);

            writer.WriteLine("Modules.Reg('{0}','0.0.0.0','{1}.js','{2}');", ModuleName, ModuleFilename, Metadata.Timestamp);

            if (Instrumentation != null && Instrumentation.Start != null)
            {
                writer.WriteLine("$(document).ready(function(){");
                writer.WriteLine("{0}.{1}();", Instrumentation.Start.OwnerScriptType.TypeId, Instrumentation.Start.ImplId);
                writer.WriteLine("});");
            }

            MetadataProvider.Current = new MetadataProvider(Metadata);
        }

        private void SanityCheck(string previousName)
        {
            if (system.AstToGenerate.Count > 0)
            {
                throw new InvalidOperationException(string.Format("Type '{0}' have been discovered while writing '{1}' !", system.AstToGenerate.Peek().Method.DeclaringType.FullName, previousName));
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
                GenerateAst();
            }
        }

        void IScriptDependencies.AddType(Type info)
        {
            AddEntryPoint(info);
        }
    }
}
