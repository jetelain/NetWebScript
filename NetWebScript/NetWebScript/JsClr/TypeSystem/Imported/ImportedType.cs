using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using NetWebScript.JsClr.TypeSystem.Inlined;
using NetWebScript.JsClr.TypeSystem.Invoker;
using NetWebScript.JsClr.TypeSystem.Standard;
using NetWebScript.Metadata;
using NetWebScript.JsClr.ScriptWriter.Declaration;

namespace NetWebScript.JsClr.TypeSystem.Imported
{
    class ImportedType : ScriptTypeBase, IScriptType, IScriptTypeDeclaration, IScriptTypeDeclarationWriter
    {
        private readonly List<ScriptMethod> extensions = new List<ScriptMethod>();
        private readonly List<IScriptTypeExtender> extenders = new List<IScriptTypeExtender>();

        private readonly CaseConvention convention;
        private readonly string name;

        public ImportedType(ScriptSystem system, Type type, ImportedAttribute imported) : base(system, type)
        {
            this.convention = imported.Convention;
            
            string typeName = imported.Name;
            if (typeName == null)
            {
                typeName = type.Name;
                if (type.IsGenericType)
                {
                    typeName = typeName.Substring(0, typeName.IndexOf('`'));
                }
            }

            if (!string.IsNullOrEmpty(type.Namespace) && !imported.IgnoreNamespace)
            {
                this.name = type.Namespace + "." + typeName;
            }
            else
            {
                this.name = typeName;
            }

            InitBaseType(false);
            InitInterfaces();

            system.AddTypeToWrite(this);
        }

        internal string Name(MemberInfo member)
        {
            if ( Attribute.IsDefined(member, typeof(PreserveCaseAttribute)) )
            {
                return member.Name;
            }
            return Name(member.Name, convention);
        }

        internal static string Name(string name, CaseConvention convention)
        {
            if (convention == CaseConvention.JavaScriptConvention)
            {
                return name.Substring(0, 1).ToLowerInvariant() + name.Substring(1);
            }
            return name;
        }

        protected sealed override IScriptConstructor CreateScriptConstructor(ConstructorInfo ctor)
        {
            return new ImportedConstructor(this, ctor);
        }

        protected sealed override IScriptMethod CreateScriptMethod(MethodInfo method)
        {
            if (Attribute.IsDefined(method, typeof(ImportedExtensionAttribute)))
            {
                var scriptMethod = new ScriptMethod(system, this, method, null, null, false);
                extensions.Add(scriptMethod);
                system.AstToGenerate.Enqueue(scriptMethod);
                return scriptMethod;
            }

            var properties = type.GetProperties();
            if (properties.Length > 0)
            {
                var property = properties.FirstOrDefault(p => p.GetGetMethod() == method || p.GetSetMethod() == method);
                if (property != null && Attribute.IsDefined(property, typeof(IntrinsicPropertyAttribute)))
                {
                    return new ImportedMethodProperty(this, method, property);
                }
            }
            if (method.IsStatic)
            {
                var alias = (ScriptAliasAttribute)Attribute.GetCustomAttribute(method, typeof(ScriptAliasAttribute));
                if (alias != null)
                {
                    return new ImportedMethodAlias(this, method, alias.Alias);
                }
            }
            var native = (ScriptBodyAttribute)Attribute.GetCustomAttribute(method, typeof(ScriptBodyAttribute));
            if (native != null && !string.IsNullOrEmpty(native.Inline))
            {
                return new InlinedMethod(this, method, native.Inline);
            }
            return new ImportedMethod(this, method);
        }

        protected sealed override IScriptField CreateScriptField(FieldInfo field)
        {
            var native = (ScriptBodyAttribute)Attribute.GetCustomAttribute(field, typeof(ScriptBodyAttribute));
            if (native != null)
            {
                return new InlinedField(this, field, native.Inline);
            }
            return new ImportedField(this, field);
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

        internal ScriptInterfaceMapping GetMapping()
        {
            return new ScriptInterfaceMapping(system, this);
        }

        public override TypeMetadata Metadata
        {
            get { return null; }
        }

        public bool CreateType
        {
            get { return false; }
        }

        public string PrettyName
        {
            get { return type.FullName; }
        }

        public string BaseTypeId
        {
            get { throw new NotSupportedException(); }
        }

        public IEnumerable<string> InterfacesTypeIds
        {
            get { throw new NotSupportedException(); }
        }

        public IEnumerable<IScriptMethodBaseDeclaration> Constructors
        {
            get { return Enumerable.Empty<IScriptMethodBaseDeclaration>(); }
        }

        public IEnumerable<IScriptMethodDeclaration> Methods
        {
            get
            {
                return extensions.Cast<IScriptMethodDeclaration>()
                    .Concat(extenders.SelectMany(e => e.Extensions.Cast<IScriptMethodDeclaration>()));
            }
        }

        public IEnumerable<IScriptFieldDeclaration> Fields
        {
            get { return Enumerable.Empty<IScriptFieldDeclaration>(); }
        }

        public IEnumerable<ScriptSlotImplementation> InterfacesMapping
        {
            get 
            {
                if (extensions.Count > 0)
                {
                    return GetMapping().InterfacesMapping; 
                }
                return Enumerable.Empty<ScriptSlotImplementation>();
            }
        }

        public bool IsEmpty
        {
            get { return extensions.Count == 0 && extenders.Count == 0; }
        }


        public void WriteDeclaration(System.IO.TextWriter writer, WriterContext context)
        {
            context.StandardDeclarationWriter.WriteDeclaration(writer, context, this);
        }

        internal void AddExtender(IScriptTypeExtender extender)
        {
            extenders.Add(extender);
        }
    }
}
