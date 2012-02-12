using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NetWebScript.JsClr.ScriptWriter.Declaration;
using NetWebScript.JsClr.TypeSystem.Invoker;
using NetWebScript.Metadata;

namespace NetWebScript.JsClr.TypeSystem.Remoting
{
    internal sealed class ServerSideType : ScriptTypeBase, IScriptTypeDeclaration, IScriptTypeDeclarationWriter
    {
        private readonly string typeId;
        private readonly TypeMetadata metadata;

        internal ServerSideType ( ScriptSystem system, Type type )
            : base(system, type)
        {
            typeId = system.CreateTypeId();

            InitBaseType(true);
            InitInterfaces();

            metadata = MetadataHelper.CreateTypeMetadata(system, this);

            system.AddTypeToWrite(this);
        }

        protected override IScriptConstructor CreateScriptConstructor(ConstructorInfo ctor)
        {
            if (!ctor.IsStatic && ctor.GetParameters().Length == 0)
            {
                return new ServerSideConstructor(system, this, ctor);
            }
            return null;
        }

        protected override IScriptMethod CreateScriptMethod(MethodInfo method)
        {
            if (!method.IsStatic)
            {
                return new ServerSideMethod(system, this, method);
            }
            return null;
        }

        protected override IScriptField CreateScriptField(FieldInfo field)
        {
            return null;
        }

        public override string TypeId
        {
            get { return typeId; }
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
            get { return true; }
        }

        public override TypeMetadata Metadata
        {
            get { return metadata; }
        }

        public bool CreateType
        {
            get { return true; }
        }

        public string PrettyName
        {
            get { return type.FullName; }
        }

        public string BaseTypeId
        {
            get { return BaseType.TypeId; }
        }

        public IEnumerable<string> InterfacesTypeIds
        {
            get { return Enumerable.Empty<string>(); }
        }

        public IEnumerable<IScriptMethodBaseDeclaration> Constructors
        {
            get { return Enumerable.Empty<IScriptMethodBaseDeclaration>(); }
        }

        public IEnumerable<IScriptMethodDeclaration> Methods
        {
            get { return Enumerable.Empty<IScriptMethodDeclaration>(); }
        }

        public IEnumerable<IScriptFieldDeclaration> Fields
        {
            get { return Enumerable.Empty<IScriptFieldDeclaration>(); }
        }

        public IEnumerable<ScriptSlotImplementation> InterfacesMapping
        {
            get { return Enumerable.Empty<ScriptSlotImplementation>(); }
        }


        public bool IsEmpty
        {
            get { return false; }
        }

        public void WriteDeclaration(System.IO.TextWriter writer, WriterContext context)
        {
            context.StandardDeclarationWriter.WriteDeclaration(writer, context, this);
        }
    }
}
