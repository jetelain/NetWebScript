using System;
using System.Collections.Generic;
using System.Reflection;
using NetWebScript.JsClr.Ast;
using NetWebScript.JsClr.TypeSystem.Invoker;
using ScriptEnum = NetWebScript.Equivalents.Enum;
using NetWebScript.Metadata;
using NetWebScript.JsClr.ScriptWriter.Declaration;

namespace NetWebScript.JsClr.TypeSystem.Standard
{
    class ScriptEnumType : IScriptType, ITypeBoxing, IScriptTypeDeclarationWriter
    {
        private readonly Type type;
        private readonly string typeId;
        private readonly IScriptMethodBase createEnumType;

        private static readonly MethodInfo EnumBox = new Func<Type, double, NetWebScript.Equivalents.Enum>(NetWebScript.Equivalents.Enum.ToObject).Method;

        public ScriptEnumType(ScriptSystem system, Type type)
        {
            this.type = type;
            this.typeId = system.CreateTypeId();
            this.createEnumType = system.GetScriptMethod(new Func<string, NetWebScript.Equivalents.Enum.EnumData[], Type>(NetWebScript.Equivalents.Enum.CreateEnumType).Method);
        }

        #region IScriptType Members

        public IScriptConstructor GetScriptConstructor(ConstructorInfo method)
        {
            return null; // No constructor is declared by an enum type
        }

        public IScriptMethod GetScriptMethod(MethodInfo method)
        {
            return null; // No method is declared by an enum type
        }

        public IScriptField GetScriptField(System.Reflection.FieldInfo field)
        {
            throw new NotImplementedException();
        }

        public Type Type
        {
            get { return type; }
        }

        public string TypeId
        {
            get { return typeId; }
        }

        public ITypeBoxing Boxing
        {
            get { return this; }
        }

        public IValueSerializer Serializer
        {
            get { return null; }
        }

        #endregion

        #region ITypeInvoker Members

        public Expression BoxValue(IScriptType type, BoxExpression boxExpression)
        {
            return new MethodInvocationExpression(boxExpression.IlOffset,
                false,
                new Func<Type, double, ScriptEnum>(ScriptEnum.ToObject).Method,
                null,
                new List<Ast.Expression>() { new LiteralExpression(type.Type), boxExpression.Value });
        }

        public Expression UnboxValue(IScriptType type, UnboxExpression boxExpression)
        {
            return new MethodInvocationExpression(boxExpression.IlOffset,
                false,
                new Func<ScriptEnum, double>(ScriptEnum.ToValue).Method,
                null,
                new List<Ast.Expression>() { boxExpression.Value });
        }

        #endregion

        #region IScriptType Members


        public bool HaveCastInformation
        {
            get { return true; }
        }

        #endregion


        public IScriptConstructor DefaultConstructor
        {
            get { return null; }
        }

        public TypeMetadata Metadata
        {
            get { return null; }
        }

        public void WriteDeclaration(System.IO.TextWriter writer, WriterContext context)
        {
            writer.Write("{2}={0}.{1}('{2}',[", createEnumType.Owner.TypeId, createEnumType.ImplId, TypeId);
            bool first = true;
            foreach (var field in Type.GetFields(BindingFlags.Public | BindingFlags.Static))
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

        public bool IsEmpty
        {
            get { return false; }
        }

        public string PrettyName
        {
            get { return type.FullName; }
        }


        public void RegisterChildType(IScriptType type)
        {
            throw new NotSupportedException();
        }

        public IScriptType BaseType
        {
            get { throw new NotSupportedException(); }
        }


        public IScriptMethod GetScriptMethodIfUsed(MethodInfo method)
        {
            return null; // No method is declared by an enum type
        }
    }
}
