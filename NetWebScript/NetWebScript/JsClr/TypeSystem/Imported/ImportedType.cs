using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics.Contracts;
using NetWebScript.JsClr.TypeSystem.Inlined;
using NetWebScript.JsClr.TypeSystem.Invoker;
using NetWebScript.JsClr.TypeSystem.Standard;

namespace NetWebScript.JsClr.TypeSystem.Imported
{
    class ImportedType : IScriptType
    {
        private readonly Type type;
        private readonly List<ImportedConstructor> constructors = new List<ImportedConstructor>();
        private readonly List<IScriptMethod> methods = new List<IScriptMethod>();
        private readonly List<IScriptField> fields = new List<IScriptField>();
        private readonly List<ScriptMethod> extensions = new List<ScriptMethod>();

        private readonly CaseConvention convention;
        private readonly string name;
        private readonly ScriptSystem system;

        public ImportedType(ScriptSystem system, Type type, ImportedAttribute imported)
        {
            this.system = system;
            this.type = type;
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


            foreach (var method in type.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
            {
                if (Attribute.IsDefined(method, typeof(ImportedExtensionAttribute)))
                {
                    var scriptMethod = new ScriptMethod(system, this, method, null, null, false);
                    extensions.Add(scriptMethod);
                    methods.Add(scriptMethod);
                    system.MethodsToGenerate.Add(scriptMethod);
                }
            }
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

        public IScriptConstructor GetScriptConstructor(ConstructorInfo method)
        {
            Contract.Requires(method.DeclaringType == type);
            var info = constructors.FirstOrDefault(f => f.Method == method);
            if (info == null)
            {
                info = new ImportedConstructor(this, method);
                constructors.Add(info);
            }
            return info;
        }

        public IScriptMethod GetScriptMethod(MethodInfo method)
        {
            Contract.Requires(method.DeclaringType == type);
            var info = methods.FirstOrDefault(f => f.Method == method);
            if (info == null)
            {
                var properties = type.GetProperties();
                if (properties.Length > 0)
                {
                    var property = properties.FirstOrDefault(p => p.GetGetMethod() == method || p.GetSetMethod() == method);
                    if (property != null && Attribute.IsDefined(property, typeof(IntrinsicPropertyAttribute)))
                    {
                        info = new ImportedMethodProperty(this, method, property);
                        methods.Add(info);
                        return info;
                    }
                }
                if (method.IsStatic)
                {
                    var alias = (ScriptAliasAttribute)Attribute.GetCustomAttribute(method, typeof(ScriptAliasAttribute));
                    if (alias != null)
                    {
                        info = new ImportedMethodAlias(this, method, alias.Alias);
                        methods.Add(info);
                        return info;
                    }
                }
                var native = (ScriptBodyAttribute)Attribute.GetCustomAttribute(method, typeof(ScriptBodyAttribute));
                if (native != null && !string.IsNullOrEmpty(native.Inline))
                {
                    info = new InlinedMethod(this, method, native.Inline);
                }
                else
                {
                    info = new ImportedMethod(this, method);
                }
                methods.Add(info);
            }
            return info;
        }

        //public IScriptMethodBase GetScriptMethodBase(MethodBase method)
        //{
        //    Contract.Requires(method.DeclaringType == type);
        //    var info = method as MethodInfo;
        //    if (info != null)
        //    {
        //        return GetScriptMethod(info);
        //    }
        //    return GetScriptConstructor((ConstructorInfo)method);
        //}

        public IScriptField GetScriptField(FieldInfo field)
        {
            Contract.Requires(field.DeclaringType == type);
            IScriptField info = fields.FirstOrDefault(f => f.Field == field);
            if (info == null)
            {
                var native = (ScriptBodyAttribute)Attribute.GetCustomAttribute(field, typeof(ScriptBodyAttribute));
                if (native != null)
                {
                    info = new InlinedField(this, field, native.Inline);
                }
                else
                {
                    info = new ImportedField(this, field);
                }
                fields.Add(info);
            }
            return info;
        }

        public Type Type
        {
            get { return type; }
        }

        public string TypeId
        {
            get { return name; }
        }


        public ITypeBoxing Boxing
        {
            get { return null; }
        }

        public IValueSerializer Serializer
        {
            get { return null; }
        }

        public bool HaveCastInformation
        {
            get { return false; }
        }

        public List<ScriptMethod> ExtensionMethods
        {
            get { return extensions; }
        }

        internal ScriptInterfaceMapping GetMapping()
        {
            return new ScriptInterfaceMapping(system, this);
        }
    }
}
