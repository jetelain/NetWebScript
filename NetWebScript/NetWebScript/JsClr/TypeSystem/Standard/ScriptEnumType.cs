using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.JsClr.TypeSystem.Invoker;
using System.Reflection;
using NetWebScript.JsClr.JsBuilder.JsSyntax;
using NetWebScript.JsClr.Ast;
using ScriptEnum = NetWebScript.Equivalents.Enum;

namespace NetWebScript.JsClr.TypeSystem.Standard
{
    class ScriptEnumType : IScriptType, ITypeBoxing
    {
        private readonly Type type;
        private readonly string typeId;

        private static readonly MethodInfo EnumBox = new Func<Type, double, NetWebScript.Equivalents.Enum>(NetWebScript.Equivalents.Enum.ToObject).Method;

        public ScriptEnumType(ScriptSystem system, Type type)
        {
            this.type = type;
            this.typeId = system.CreateTypeId();
            system.GetScriptType(typeof(NetWebScript.Equivalents.Enum));
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
    }
}
