using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Reflection;
using NetWebScript.JsClr.TypeSystem;
using NetWebScript.JsClr.TypeSystem.Standard;
using NetWebScript.Script;
using NetWebScript.Metadata;
using NetWebScript.JsClr.TypeSystem.Imported;
using NetWebScript.JsClr.JsBuilder.JsSyntax;
using System.Collections.Generic;

namespace NetWebScript.JsClr.Compiler
{
    class ModuleWriter
    {
        private readonly TextWriter writer;
        private readonly ScriptSystem system;
        private readonly bool debuggable;
        private readonly bool pretty;
        private readonly IScriptMethodBase createTypeMethod;

        public ModuleWriter(TextWriter writer, ScriptSystem system, bool debuggable, bool pretty)
        {
            this.writer = writer;
            this.system = system;
            this.debuggable = debuggable;
            this.pretty = pretty;
            createTypeMethod = system.GetScriptMethod(new Func<string, Type, Type[], Type>(TypeSystemHelper.CreateType).Method);
        }

        private TypeMetadata CreateMetadata(ModuleMetadata module, ScriptType scriptType)
        {
            var meta = new TypeMetadata();
            meta.Module = module;
            meta.Name = scriptType.TypeId;
            meta.CRef = CRefToolkit.GetCRef(scriptType.Type);
            if ( scriptType.BaseType != null )
            {
                meta.BaseTypeName = scriptType.BaseType.TypeId;
            }
            module.Types.Add(meta);
            return meta;
        }

        private FieldMetadata CreateMetadata(TypeMetadata type, ScriptField field)
        {
            var meta = new FieldMetadata();
            meta.Name = field.SlodId;
            meta.CRef = CRefToolkit.GetCRef(field.Field);
            type.Fields.Add(meta);
            return meta;
        }

        private MethodBaseMetadata CreateMetadata(TypeMetadata type, ScriptMethodBase methodBase)
        {
            if (type == null)
            {
                return null;
            }
            var meta = new MethodBaseMetadata();
            meta.Type = type;
            meta.Name = methodBase.ImplId;
            meta.CRef = CRefToolkit.GetCRef(methodBase.Method);
            type.Methods.Add(meta);
            return meta;
        }

        internal void WriteType(ModuleMetadata moduleMeta, ScriptType type)
        {
            var classMeta = CreateMetadata(moduleMeta, type);

            if (pretty)
            {
                writer.WriteLine();
                writer.WriteLine();
                writer.WriteLine();
                writer.WriteLine("//### Type {0}", type.Type.ToString());
            }

            if (!type.IsGlobals)
            {
                if (type.BaseType != null && (type.BaseType.TypeId != "Object" || type.Interfaces.Count > 0))
                {
                    writer.WriteLine("{1}={0}('{1}',{2},[{3}]);", createTypeMethod.ImplId, type.TypeId, type.BaseType.TypeId, string.Join(",", type.Interfaces.Select(i => i.TypeId)));
                }
                else
                {
                    writer.WriteLine("{1}={0}('{1}');", createTypeMethod.ImplId, type.TypeId);
                }
            }

            foreach (var field in type.Fields)
            {
                CreateMetadata(classMeta, field);
            }

            foreach (var ctor in type.MethodsToWrite.OfType<ScriptConstructor>())
            {
                if (ctor.Method.IsStatic)
                {
                    WriteStaticMethod(type, classMeta, ctor);
                }
                else
                {
                    WriteInstanceMethodBase(type, classMeta, ctor);
                }
            }

            foreach (var method in type.MethodsToWrite.OfType<ScriptMethod>())
            {
                if (!method.Method.IsAbstract)
                {
                    if (method.Method.IsStatic)
                    {
                        WriteStaticMethod(type, classMeta, method);
                    }
                    else
                    {
                        WriteInstanceMethod(type, classMeta, method);
                    }
                }
                else
                {
                    CreateMetadata(classMeta, method);
                }
            }
            var map = type.GetMapping();
            foreach (var pair in map.Mapping)
            {
                WriteSlot(pair.Key, type, pair.Value);
            }

            foreach (var field in type.Fields)
            {
                writer.Write(type.TypeId);
                writer.Write('.');
                if (!field.Field.IsStatic)
                {
                    writer.Write("prototype.");
                }
                writer.Write(field.SlodId);
                if (field.Field.IsLiteral)
                {
                    var value = field.Field.GetRawConstantValue();
                    var strValue = value as string;
                    if (strValue != null)
                    {
                        writer.WriteLine("={0}; // RAW", JsToken.LiteralString(strValue));
                    }
                    else if (value != null)
                    {
                        writer.WriteLine("=null; // RAW");
                    }
                    else
                    {
                        writer.WriteLine("={0}; // RAW", value);
                    }
                }
                else if (field.Field.FieldType.IsValueType)
                {
                    writer.WriteLine("=0;");
                }
                else
                {
                    writer.WriteLine("=null;");
                }
            }
        }

        private void WriteStaticMethod(ImportedType stype, TypeMetadata classMeta, ScriptMethod method)
        {
            if (pretty)
            {
                writer.WriteLine();
                writer.WriteLine("// Static {0}", method.Method.ToString());
            }
            writer.Write(method.ImplId);
            writer.Write('=');
            WriteMethodBody(classMeta, method);
            writer.WriteLine(';');
        }

        private void WriteStaticMethod(ScriptType stype, TypeMetadata classMeta, ScriptMethodBase method)
        {
            if (pretty)
            {
                writer.WriteLine();
                writer.WriteLine("// Static {0}", method.Method.ToString());
            }
            if (!stype.IsGlobals)
            {
                writer.Write(stype.TypeId);
                writer.Write('.');
            }
            writer.Write(method.ImplId);
            writer.Write('=');
            WriteMethodBody(classMeta, method);
            writer.WriteLine(';');
        }
        private void WriteInstanceMethodBase(IScriptType stype, TypeMetadata classMeta, ScriptMethodBase method)
        {
            if (pretty)
            {
                writer.WriteLine();
                writer.WriteLine("// Instance {0}", method.Method.ToString());
            } 
            writer.Write(stype.TypeId);
            writer.Write(".prototype.");
            writer.Write(method.ImplId);
            writer.Write('=');
            WriteMethodBody(classMeta, method);
            writer.WriteLine(';');
        }

        private void WriteSlot(string slotId, IScriptType stype, IScriptMethod method)
        {
            Contract.Requires(method.SlodId != null);
            writer.Write(stype.TypeId);
            writer.Write(".prototype.");
            writer.Write(slotId);
            writer.Write('=');
            writer.Write(stype.TypeId);
            writer.Write(".prototype.");
            writer.Write(method.ImplId);
            writer.WriteLine(';');
        }


        private void WriteInstanceMethod(IScriptType stype, TypeMetadata classMeta, ScriptMethod method)
        {
            WriteInstanceMethodBase(stype, classMeta, method);
            if (method.SlodId != null)
            {
                WriteSlot(method.SlodId, stype, method);
            }
        }

        private void WriteMethodBody(TypeMetadata typeMeta, ScriptMethodBase method)
        {
            var meta = CreateMetadata(typeMeta, method);

            if (method.HasNativeBody)
            {
                writer.Write(method.NativeBody);
                return;
            }

            var astWriter = new AstScriptWriter(system, method.Ast, debuggable ? meta : null, pretty);

            writer.Write("function(");
            bool first = true;
            foreach (var arg in method.Ast.Arguments)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    writer.Write(',');
                }
                writer.Write(astWriter.ArgumentName(arg));
            }
            writer.Write(")");
            
            try
            {
                astWriter.WriteBody(writer);
            }
            catch (Exception e)
            {
                throw new Exception(String.Format("Error generating method '{0}' declared by '{1}' : {2}.", method.Method.ToString(), method.Method.DeclaringType.FullName, e.Message), e);
            }
        }

        internal void WriteEnumType(ModuleMetadata Metadata, ScriptEnumType type)
        {
            //var classMeta = new TypeMetadata(moduleMeta, type);

            if (pretty)
            {
                writer.WriteLine();
                writer.WriteLine();
                writer.WriteLine();
                writer.WriteLine("//### Type {0}", type.Type.ToString());
            }
            var createEnumType = system.GetScriptMethod(new Func<string, NetWebScript.Equivalents.Enum.EnumData[], Type>(NetWebScript.Equivalents.Enum.CreateEnumType).Method);


            writer.Write("{2}={0}.{1}('{2}',[", createEnumType.Owner.TypeId, createEnumType.ImplId, type.TypeId);
            bool first = true;
            foreach (var field in type.Type.GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                if (first)
                {
                    first = false;
                    writer.WriteLine();
                }
                else
                {
                    writer.WriteLine(",");
                }
                writer.Write("{{v:{0},n:'{1}'}}", Convert.ToInt32(field.GetValue(null)), field.Name);
            }
            writer.WriteLine("]);");
        }

        internal void WriteTypeExtensions(ModuleMetadata Metadata, TypeSystem.Imported.ImportedType type)
        {
            if (pretty)
            {
                writer.WriteLine();
                writer.WriteLine();
                writer.WriteLine();
                writer.WriteLine("//### Type {0}", type.Type.ToString());
            }

            foreach (var method in type.ExtensionMethods)
            {
                if (!method.Method.IsAbstract)
                {
                    if (method.Method.IsStatic)
                    {
                        WriteStaticMethod(type, null, method);
                    }
                    else
                    {
                        WriteInstanceMethod(type, null, method);
                    }
                }
            }

            var map = type.GetMapping();
            foreach (var pair in map.Mapping)
            {
                WriteSlot(pair.Key, type, pair.Value);
            }
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
            var ctor = type.MethodsToWrite.OfType<ScriptConstructor>().FirstOrDefault(c => c.Method.IsPublic);
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
