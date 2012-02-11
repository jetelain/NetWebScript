using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using NetWebScript.JsClr.TypeSystem.Invoker;
using NetWebScript.Metadata;

namespace NetWebScript.JsClr.TypeSystem.Anonymous
{
    class AnonymousType : ScriptTypeBase
    {

        public AnonymousType(ScriptSystem system, Type type, CaseConvention convention) : base(system, type)
        {

            foreach (var field in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly))
            {
                fields.Add(new AnonymousField(this, field, convention));
            }

            var ctor = type.GetConstructor(Type.EmptyTypes);
            if (ctor == null)
            {
                throw new Exception();
            }
            constructors.Add(new AnonymousConstructor(this, ctor));

        }


        public override string TypeId
        {
            get { return null; }
        }

        public override ITypeBoxing Boxing
        {
            get { return null; }
        }

        public override bool HaveCastInformation
        {
            get { return false; }
        }

        public override IValueSerializer Serializer
        {
            get { return null; }
        }

        public override TypeMetadata Metadata
        {
            get { return null; }
        }

        protected override IScriptConstructor CreateScriptConstructor(ConstructorInfo ctor)
        {
            return null;
        }

        protected override IScriptMethod CreateScriptMethod(MethodInfo method)
        {
            return null;
        }

        protected override IScriptField CreateScriptField(FieldInfo field)
        {
            return null;
        }
    }
}
