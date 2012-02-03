using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.JsClr.TypeSystem.Invoker;
using NetWebScript.Metadata;

namespace NetWebScript.JsClr.TypeSystem.Native
{
    abstract class NativeType : IScriptType
    {
        private readonly string name;
        protected readonly Type type;
        protected readonly List<IScriptMethodBase> methods = new List<IScriptMethodBase>();
        protected readonly List<IScriptField> fields = new List<IScriptField>();

        protected NativeType(string name, Type type)
        {
            this.name = name;
            this.type = type;
        }

        #region IScriptType Members

        public IScriptConstructor GetScriptConstructor(System.Reflection.ConstructorInfo method)
        {
            return (IScriptConstructor)GetScriptMethodBase(method);
        }

        public IScriptMethod GetScriptMethod(System.Reflection.MethodInfo method)
        {
            return (IScriptMethod)GetScriptMethodBase(method);
        }

        private IScriptMethodBase GetScriptMethodBase(System.Reflection.MethodBase method)
        {
            return methods.FirstOrDefault(m => m.Method == method);
        }

        public IScriptField GetScriptField(System.Reflection.FieldInfo field)
        {
            return fields.FirstOrDefault(f => f.Field == field);
        }

        public Type Type
        {
            get { return type ; }
        }

        public string TypeId
        {
            get { return name; }
        }

        public virtual ITypeBoxing Boxing
        {
            get { return null; }
        }

        public virtual IValueSerializer Serializer
        {
            get { return null; }
        }

        public virtual bool HaveCastInformation
        {
            get { return false; }
        }

        #endregion

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

        public TypeMetadata Metadata
        {
            get { return null; }
        }


        public void RegisterChildType(IScriptType type)
        {

        }

        public IScriptType BaseType
        {
            get { return null; }
        }


        public IScriptMethod GetScriptMethodIfUsed(System.Reflection.MethodInfo method)
        {
            return (IScriptMethod)GetScriptMethodBase(method);
        }
    }
}
