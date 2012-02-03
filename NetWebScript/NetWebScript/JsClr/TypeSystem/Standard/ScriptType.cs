using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using NetWebScript.JsClr.TypeSystem.Inlined;
using NetWebScript.JsClr.TypeSystem.Invoker;
using NetWebScript.Metadata;
using NetWebScript.JsClr.ScriptWriter.Declaration;

namespace NetWebScript.JsClr.TypeSystem.Standard
{
    public class ScriptType : ScriptTypeBase, IScriptTypeDeclaration, IScriptTypeDeclarationWriter
    {
        private readonly string typeId;
        private readonly bool isGlobals;

        private readonly CaseConvention exportCaseConvention;

        private readonly bool isExported;
        private readonly string exportedName;
        private readonly string exportedNamespace;
        private IScriptConstructor exportedConstructor;

        private readonly IScriptConstructor staticScriptConstructor;
        private readonly TypeMetadata metadata;


        public ScriptType(ScriptSystem system, Type type) : base(system, type)
        {
            if (!type.IsGenericType)
            {
                // Generic types are not allowed to be exported !
                var exported = (ExportedAttribute)Attribute.GetCustomAttribute(type, typeof(ExportedAttribute));
                if (exported != null)
                {
                    this.isExported = true;
                    this.exportCaseConvention = exported.Convention;
                    if (exported.Name != null)
                    {
                        exportedName = exported.Name;
                    }
                    else
                    {
                        exportedName = CaseToolkit.GetMemberName(exportCaseConvention, type.Name);
                    }
                    if (exported.IgnoreNamespace)
                    {
                        exportedNamespace = string.Empty;
                    }
                    else
                    {
                        exportedNamespace = type.Namespace;
                    }
                }
            }

            this.typeId = system.CreateTypeId();
            isGlobals = Attribute.IsDefined(type, typeof(GlobalsAttribute));

            InitBaseType(false);
            InitInterfaces();

            this.metadata = CreateTypeMetadata();

            var staticCtor = type.GetConstructor(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly, null, Type.EmptyTypes, null);
            if ( staticCtor != null )
            {
                staticScriptConstructor = GetScriptConstructor(staticCtor);
            }

            if (type.IsInterface)
            {
                // Generic methods within an interface is not supported.
                // It would require to force generation of concreate method in all types
                // implementing interface.
                foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly))
                {
                    if (method.IsGenericMethodDefinition)
                    {
                        throw new NotSupportedException("An interface should have generic method.");
                    }
                }
            }
            
            if (isExported)
            {
                EnsurePublicMembersForExport();
            }


        }

        private void EnsurePublicMembersForExport()
        {
            var pubCtors = type.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
            if (pubCtors.Length > 1)
            {
                throw new Exception(string.Format("Type '{0}' is exported, it cannot have more than one public constructor", type.FullName));
            }

            foreach (var ctor in pubCtors)
            {
               exportedConstructor = GetScriptConstructor(ctor);
            }

            foreach (var method in type.GetMethods( BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly))
            {
                if (!method.IsGenericMethodDefinition)
                {
                    GetScriptMethod(method);
                }
            }

            foreach (var field in type.GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public))
            {
                if (field.DeclaringType == type)
                {
                    GetScriptField(field);
                }
            }
        }


        private TypeMetadata CreateTypeMetadata()
        {
            var meta = new TypeMetadata();
            meta.Module = system.Metadata;
            meta.Name = TypeId;
            meta.CRef = CRefToolkit.GetCRef(Type);
            if (BaseType != null)
            {
                meta.BaseTypeName = BaseType.TypeId;
            }
            system.Metadata.Types.Add(meta);
            return meta;
        }

        internal ScriptInterfaceMapping GetMapping()
        {
            return new ScriptInterfaceMapping(system, this);
        }

        protected sealed override IScriptConstructor CreateScriptConstructor(ConstructorInfo ctor)
        {
            var scriptCtor = new ScriptConstructor(system, this, ctor);
            system.AstToGenerate.Enqueue(scriptCtor);
            return scriptCtor;
        }

        protected sealed override IScriptMethod CreateScriptMethod(MethodInfo method)
        {
            var native = (ScriptBodyAttribute)Attribute.GetCustomAttribute(method, typeof(ScriptBodyAttribute));
            if (native != null && !string.IsNullOrEmpty(native.Inline))
            {
                return new InlinedMethod(this, method, native.Inline);
            }

            string exported = null;
            if (isExported && method.IsPublic && !method.IsStatic)
            {
                // Static methods will have correct name on the export object
                exported = CaseToolkit.GetMemberName(exportCaseConvention, method.Name);
                // FIXME: check for conflicts
            }
            var body = native != null ? native.Body : null;
            var generated = new ScriptMethod(system, this, method, body, exported);
            system.AstToGenerate.Enqueue(generated);
            return generated;
        }

        protected sealed override IScriptField CreateScriptField(FieldInfo field)
        {
            var native = (ScriptBodyAttribute)Attribute.GetCustomAttribute(field, typeof(ScriptBodyAttribute));
            if (native != null)
            {
                return new InlinedField(this, field, native.Inline);
            }

            string exported = null;
            if (isExported && field.IsPublic && !field.IsStatic)
            {
                exported = CaseToolkit.GetMemberName(exportCaseConvention, field.Name);
                // FIXME: check for conflicts
            }
            return new ScriptField(system, this, field, exported);
        }

        public override string TypeId
        {
            get { return typeId; }
        }

        internal virtual IEnumerable<ScriptMethodBase> MethodsToWrite
        {
            get { return methods.OfType<ScriptMethodBase>(); }
        }

        internal IEnumerable<ScriptField> Fields
        {
            get { return fields.OfType<ScriptField>(); }
        }

        public override ITypeBoxing Boxing
        {
            get { return null; }
        }

        public override IValueSerializer Serializer
        {
            get { return null; }
        }

        public IScriptConstructor ExportedConstructor
        {
            get { return exportedConstructor; }
        }

        public IList<IScriptType> Interfaces
        {
            get { return interfaces; }
        }

        public bool IsGlobals
        {
            get { return isGlobals; }
        }

        public override bool HaveCastInformation
        {
            get { return true; }
        }

        public bool IsExported
        {
            get { return isExported; }
        }

        public string ExportName
        {
            get { return exportedName; }
        }

        public string ExportNamespace
        {
            get { return exportedNamespace; }
        }

        public CaseConvention ExportCaseConvention
        {
            get { return exportCaseConvention; }
        }

        public IScriptConstructor StaticConstructor
        {
            get { return staticScriptConstructor; }
        }

        public override TypeMetadata Metadata
        {
            get { return metadata; }
        }

        public bool CreateType
        {
            get { return !IsGlobals; }
        }

        public string PrettyName
        {
            get { return type.FullName; }
        }

        public string BaseTypeId
        {
            get 
            { 
                if ( baseType != null )
                {
                    return baseType.TypeId;
                }
                return null;
            }
        }

        public IEnumerable<string> InterfacesTypeIds
        {
            get { return interfaces.Select(i => i.TypeId); }
        }

        public IEnumerable<IScriptMethodBaseDeclaration> Constructors
        {
            get { return constructors.Cast<IScriptMethodBaseDeclaration>(); }
        }

        public virtual IEnumerable<IScriptMethodDeclaration> Methods
        {
            get { return methods.OfType<ScriptMethod>().Where(m => !m.Method.IsAbstract).Cast<IScriptMethodDeclaration>(); }
        }

        IEnumerable<IScriptFieldDeclaration> IScriptTypeDeclaration.Fields
        {
            get { return fields.OfType<IScriptFieldDeclaration>(); }
        }

        public IEnumerable<ScriptSlotImplementation> InterfacesMapping
        {
            get { return GetMapping().InterfacesMapping; }
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
