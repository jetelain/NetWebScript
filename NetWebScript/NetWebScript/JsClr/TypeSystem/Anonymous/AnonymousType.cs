using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using NetWebScript.JsClr.TypeSystem.Invoker;

namespace NetWebScript.JsClr.TypeSystem.Anonymous
{
    class AnonymousType : IScriptType
    {
        private readonly Type type;
        private readonly List<AnonymousField> fields = new List<AnonymousField>();
        private readonly AnonymousConstructor scriptCtor;

        public AnonymousType(Type type, CaseConvention convention)
        {
            this.type = type;

            foreach (var field in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly))
            {
                fields.Add(new AnonymousField(this, field, convention));
            }

            var ctor = type.GetConstructor(Type.EmptyTypes);
            if (ctor == null)
            {
                throw new Exception();
            }
            scriptCtor = new AnonymousConstructor(this, ctor);

        }

        public IScriptConstructor GetScriptConstructor(ConstructorInfo method)
        {
            if (scriptCtor.Method == method)
            {
                return scriptCtor;
            }
            return null;
        }

        public IScriptMethod GetScriptMethod(MethodInfo method)
        {
            return null;
        }

        public IScriptField GetScriptField(FieldInfo field)
        {
            return fields.FirstOrDefault(f => f.Field == field);
        }

        public Type Type
        {
            get { return type; }
        }

        public string TypeId
        {
            get { return null; }
        }

        public ITypeBoxing Boxing
        {
            get { return null; }
        }

        public bool HaveCastInformation
        {
            get { return false; }
        }

        public IValueSerializer Serializer
        {
            get { return null; }
        }

        public IScriptConstructor DefaultConstructor
        {
            get
            {
                var ctor = type.GetConstructor(Type.EmptyTypes);
                if (ctor != null)
                {
                    return GetScriptConstructor(ctor);
                }
                return null;
            }
        }
    }
}
