using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.JsClr.TypeSystem.Invoker;
using NetWebScript.Metadata;

namespace NetWebScript.JsClr.TypeSystem.Native
{
    abstract class NativeType : ScriptTypeBase
    {
        private readonly string name;

        protected NativeType(ScriptSystem system, string name, Type type) : base(system, type)
        {
            this.name = name;
        }

        public override string TypeId
        {
            get { return name; }
        }

        public override ITypeBoxing Boxing
        {
            get { return null; }
        }

        public override IValueSerializer Serializer
        {
            get { return null; }
        }

        public override bool HaveCastInformation
        {
            get { return false; }
        }

        public override TypeMetadata Metadata
        {
            get { return null; }
        }

        protected override IScriptConstructor CreateScriptConstructor(System.Reflection.ConstructorInfo ctor)
        {
            return null;
        }

        protected override IScriptMethod CreateScriptMethod(System.Reflection.MethodInfo method)
        {
            return null;
        }

        protected override IScriptField CreateScriptField(System.Reflection.FieldInfo field)
        {
            return null;
        }
    }
}
