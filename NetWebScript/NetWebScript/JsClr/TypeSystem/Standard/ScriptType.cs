using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NetWebScript.JsClr.ScriptWriter.Declaration;
using NetWebScript.JsClr.TypeSystem.Inlined;
using NetWebScript.JsClr.TypeSystem.Invoker;
using NetWebScript.Metadata;

namespace NetWebScript.JsClr.TypeSystem.Standard
{
    public class ScriptType : ScriptTypeBase, IScriptTypeDeclaration, IScriptTypeDeclarationWriter
    {
        private readonly string typeId;
        private readonly bool isGlobals;

        private readonly ExportDefinition exportDefinition;
        private readonly TypeMetadata metadata;

        public ScriptType(ScriptSystem system, Type type) : base(system, type)
        {
            exportDefinition = ExportDefinition.GetExportDefinition(type);

            this.typeId = system.CreateTypeId();
            isGlobals = Attribute.IsDefined(type, typeof(GlobalsAttribute));

            InitBaseType(false);
            InitInterfaces();

            this.metadata = MetadataHelper.CreateTypeMetadata(system, this);

            var staticCtor = type.GetConstructor(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly, null, Type.EmptyTypes, null);
            if ( staticCtor != null )
            {
                var staticScriptConstructor = GetScriptConstructor(staticCtor);
                system.AddStaticConstructor(staticScriptConstructor);
            }

            if (type.IsInterface && HasGenericMethod(type))
            {
                // Generic methods within an interface is not supported.
                // It would require to force generation of concreate method in all types
                // implementing interface.
                throw new NotSupportedException("An interface should have generic method.");
            }

            if (exportDefinition != null)
            {
                exportDefinition.InitMappedType(this);
                system.AddExport(exportDefinition);
            }

            system.AddTypeToWrite(this);
        }
        internal static bool HasGenericMethod(Type type)
        {
            foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly))
            {
                if (method.IsGenericMethodDefinition)
                {
                    return true;
                }
            }
            return false;
        }

        internal ScriptInterfaceMapping GetMapping()
        {
            return new ScriptInterfaceMapping(system, this);
        }

        protected sealed override IScriptConstructor CreateScriptConstructor(ConstructorInfo ctor)
        {
            var native = (ScriptBodyAttribute)Attribute.GetCustomAttribute(ctor, typeof(ScriptBodyAttribute));
            if (native != null && !string.IsNullOrEmpty(native.Inline))
            {
                return new InlinedConstructor(this, ctor, native.Inline);
            }
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
            if (exportDefinition != null && method.IsPublic && !method.IsStatic)
            {
                // Static methods will have correct name on the export object
                exported = CaseToolkit.GetMemberName(exportDefinition.ExportCaseConvention, method.Name);
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
            if (exportDefinition != null && field.IsPublic && !field.IsStatic)
            {
                exported = CaseToolkit.GetMemberName(exportDefinition.ExportCaseConvention, field.Name);
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

        public ExportDefinition ExportDefinition
        {
            get { return exportDefinition; }
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
            get { return constructors.OfType<IScriptMethodBaseDeclaration>(); }
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
