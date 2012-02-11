using System;
using System.IO;
using System.Linq;
using NetWebScript.JsClr.JsBuilder.JsSyntax;
using NetWebScript.JsClr.ScriptAst;
using NetWebScript.JsClr.ScriptWriter.Declaration;
using NetWebScript.JsClr.TypeSystem;
using NetWebScript.Script;

namespace NetWebScript.JsClr.Compiler
{
    public class TypeDeclarationWriter : IScriptTypeStandardDeclarationWriter
    {
        private readonly IScriptMethodBase createTypeMethod;

        public TypeDeclarationWriter(ScriptSystem system)
        {
            this.createTypeMethod = system.GetScriptMethod(new Func<string, Type, Type[], Type>(TypeSystemHelper.CreateType).Method);
        }

        public void WriteDeclaration(TextWriter writer, WriterContext context, IScriptTypeDeclaration type)
        {
            if (type.CreateType)
            {
                if (type.BaseTypeId != null && (type.BaseTypeId != "Object" || type.InterfacesTypeIds.Any()))
                {
                    writer.WriteLine("{1}={0}('{1}',{2},[{3}]);", createTypeMethod.ImplId, type.TypeId, type.BaseTypeId, string.Join(",", type.InterfacesTypeIds));
                }
                else
                {
                    writer.WriteLine("{1}={0}('{1}');", createTypeMethod.ImplId, type.TypeId);
                }
            }

            foreach (var ctor in type.Constructors)
            {
                if (ctor.IsStatic)
                {
                    WriteStaticConstructor(writer, context, type, ctor);
                }
                else
                {
                    WriteInstanceMethodBase(writer, context, type, ctor);
                }
            }

            foreach (var method in type.Methods)
            {
                if (method.IsStatic)
                {
                    WriteStaticMethod(writer, context, type, method);
                }
                else
                {
                    WriteInstanceMethod(writer, context, type, method);
                }
            }

            foreach (var pair in type.InterfacesMapping)
            {
                WriteSlot(writer, context, type, pair.SlotId, pair.ImplId);
            }

            foreach (var field in type.Fields)
            {
                writer.Write(type.TypeId);
                writer.Write('.');
                if (!field.IsStatic)
                {
                    writer.Write("prototype.");
                }
                writer.Write(field.SlodId);
                writer.Write("=");
                writer.Write(ToScript(field.InitialValue).Text);
                writer.WriteLine(";");
            }
        }

        private static JsToken ToScript(ScriptLiteralExpression literalExpression)
        {
            return new AstScriptWriter().Visit(literalExpression);
        }

        private static void WriteStaticConstructor(TextWriter writer, WriterContext context, IScriptTypeDeclaration stype, IScriptMethodBaseDeclaration method)
        {
            if (context.IsPretty)
            {
                writer.WriteLine();
                writer.WriteLine("// Static {0}", method.PrettyName);
            }
            writer.Write(stype.TypeId);
            writer.Write('.');
            writer.Write(method.ImplId);
            writer.Write('=');
            WriteMethodBody(writer, context, stype, method);
            writer.WriteLine(';');
        }

        private static void WriteStaticMethod(TextWriter writer, WriterContext context, IScriptTypeDeclaration stype, IScriptMethodDeclaration method)
        {
            if (context.IsPretty)
            {
                writer.WriteLine();
                writer.WriteLine("// Static {0}", method.PrettyName);
            }
            if (!method.IsGlobal)
            {
                writer.Write(stype.TypeId);
                writer.Write('.');
            }
            writer.Write(method.ImplId);
            writer.Write('=');
            WriteMethodBody(writer, context, stype, method);
            writer.WriteLine(';');
        }

        private static void WriteInstanceMethodBase(TextWriter writer, WriterContext context, IScriptTypeDeclaration stype, IScriptMethodBaseDeclaration method)
        {
            if (context.IsPretty)
            {
                writer.WriteLine();
                writer.WriteLine("// Instance {0}", method.PrettyName);
            } 
            writer.Write(stype.TypeId);
            writer.Write(".prototype.");
            writer.Write(method.ImplId);
            writer.Write('=');
            WriteMethodBody(writer, context, stype, method);
            writer.WriteLine(';');
        }

        private static void WriteSlot(TextWriter writer, WriterContext context, IScriptTypeDeclaration stype, string slotId, string implId)
        {
            writer.Write(stype.TypeId);
            writer.Write(".prototype.");
            writer.Write(slotId);
            writer.Write('=');
            writer.Write(stype.TypeId);
            writer.Write(".prototype.");
            writer.Write(implId);
            writer.WriteLine(';');
        }


        private static void WriteInstanceMethod(TextWriter writer, WriterContext context, IScriptTypeDeclaration stype, IScriptMethodDeclaration method)
        {
            WriteInstanceMethodBase(writer, context, stype, method);
            if (method.SlodId != null && method.SlodId != method.ImplId)
            {
                WriteSlot(writer, context, stype, method.SlodId, method.ImplId);
            }
        }

        private static void WriteMethodBody(TextWriter writer, WriterContext context, IScriptTypeDeclaration type, IScriptMethodBaseDeclaration method)
        {
            if (method.HasNativeBody)
            {
                writer.Write(method.NativeBody);
                return;
            }

            var astWriter = new AstScriptWriter(method, context.IsPretty, context.Instrumentation);

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
                throw new Exception(String.Format("Error generating method '{0}' declared by '{1}' : {2}.", method.PrettyName, type.PrettyName, e.Message), e);
            }
        }


    }
}
