using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace NetWebScript.JsClr.TypeSystem
{
    public sealed class ExportDefinition
    {
        private readonly Type type;
        private readonly string exportedName;
        private readonly string exportedNamespace;
        private readonly CaseConvention exportCaseConvention;
        private readonly List<IScriptMethod> staticMethods = new List<IScriptMethod>();
        private IScriptConstructor exportedConstructor;

        private ExportDefinition(Type type, ExportedAttribute exported)
        {
            this.type = type;
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

        public static ExportDefinition GetExportDefinition(Type type)
        {
            if (!type.IsGenericType)
            {
                // Generic types are not allowed to be exported !
                var exported = (ExportedAttribute)Attribute.GetCustomAttribute(type, typeof(ExportedAttribute));
                if (exported != null)
                {
                    return new ExportDefinition(type, exported);
                }
            }
            return null;
        }

        public string ExportName
        {
            get { return exportedName; }
        }

        public string PrettyName
        {
            get { return type.FullName; }
        }

        public string ExportNamespace
        {
            get { return exportedNamespace; }
        }

        public CaseConvention ExportCaseConvention
        {
            get { return exportCaseConvention; }
        }

        public IScriptConstructor ExportedConstructor
        {
            get { return exportedConstructor; }
        }

        public IEnumerable<IScriptMethod> ExportedStaticMethods
        {
            get { return staticMethods; }
        }

        public void InitMappedType(IScriptType scriptType)
        {
            if (scriptType.Type != type)
            {
                throw new ArgumentException();
            }

            var pubCtors = type.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
            if (pubCtors.Length > 1)
            {
                throw new Exception(string.Format("Type '{0}' is exported, it cannot have more than one public constructor", type.FullName));
            }

            foreach (var ctor in pubCtors)
            {
                exportedConstructor = scriptType.GetScriptConstructor(ctor);
            }

            foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly))
            {
                if (!method.IsGenericMethodDefinition)
                {
                    var scriptMethod = scriptType.GetScriptMethod(method);
                    if (method.IsStatic)
                    {
                        staticMethods.Add(scriptMethod);
                    }
                }
            }

            foreach (var field in type.GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public))
            {
                if (field.DeclaringType == type)
                {
                    scriptType.GetScriptField(field);
                }
            }
        }

    }
}
