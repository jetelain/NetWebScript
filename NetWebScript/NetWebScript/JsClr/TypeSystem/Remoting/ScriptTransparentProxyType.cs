using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.JsClr.TypeSystem.Invoker;
using NetWebScript.Remoting;

namespace NetWebScript.JsClr.TypeSystem.Remoting
{
    class ScriptTransparentProxyType : ScriptTypeBase
    {
        internal ScriptTransparentProxyType(ScriptSystem system)
            : base(system, typeof(ScriptTransparentProxy))
        {

        }

        protected override IScriptConstructor CreateScriptConstructor(System.Reflection.ConstructorInfo ctor)
        {
            return null;
        }

        protected override IScriptMethod CreateScriptMethod(System.Reflection.MethodInfo method)
        {
            if (method.Name != "Create" || !method.IsGenericMethod)
            {
                return null;
            }

            var targetType = method.GetGenericArguments()[0];
            var targetScriptType = system.GetScriptType(targetType);
            if (targetScriptType == null)
            {
                return null;
            }

            var proxy = targetScriptType.TransparentProxy;

            return new ScriptTransparentProxyCreate(this, method, proxy);
        }

        protected override IScriptField CreateScriptField(System.Reflection.FieldInfo field)
        {
            return null;
        }

        public override string TypeId
        {
            get { return null; }
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

        public override Metadata.TypeMetadata Metadata
        {
            get { return null; }
        }
    }
}
