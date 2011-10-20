using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Reflection;
using NetWebScript.JsClr.TypeSystem;
using NetWebScript.JsClr.TypeSystem.Standard;
using NetWebScript.Script;
using NetWebScript.Metadata;

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

            foreach (var ctor in type.Methods.OfType<ScriptConstructor>())
            {
                WriteInstanceMethodBase(type, classMeta, ctor);
            }

            foreach (var method in type.Methods.OfType<ScriptMethod>())
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
        }

        


        private void WriteStaticMethod(ScriptType stype, TypeMetadata classMeta, ScriptMethod method)
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
        private void WriteInstanceMethodBase(ScriptType stype, TypeMetadata classMeta, ScriptMethodBase method)
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

        private void WriteSlot(string slotId, ScriptType stype, IScriptMethod method)
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


        private void WriteInstanceMethod(ScriptType stype, TypeMetadata classMeta, ScriptMethod method)
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

            var astWriter = new AstScriptWriter(system, method.Method, debuggable ? meta : null, pretty);

            writer.Write("function(");
            ParameterInfo[] parameters = method.Method.GetParameters();
            for (int i = 0; i < parameters.Length; ++i)
            {
                if (i > 0)
                {
                    writer.Write(',');
                }
                writer.Write(astWriter.ArgumentName(parameters[i]));
            }
            writer.Write(")");
            
            try
            {
                astWriter.Body(writer, method.Ast);
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
    }
}
