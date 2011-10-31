using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using NetWebScript.JsClr.TypeSystem.Inlined;
using NetWebScript.JsClr.TypeSystem.Invoker;

namespace NetWebScript.JsClr.TypeSystem.Standard
{
    public class ScriptType : IScriptType
    {
        private readonly Type type;
        private readonly string typeId;
        private readonly List<IScriptMethodBase> methods = new List<IScriptMethodBase>();
        private readonly List<IScriptField> fields = new List<IScriptField>();
        private readonly IScriptType baseType;
        private readonly List<IScriptType> interfaces = new List<IScriptType>();
        private readonly ScriptSystem system;
        private readonly bool isGlobals;

        public ScriptType(ScriptSystem system, Type type)
        {
            this.system = system;

           
            if (type.BaseType != null && type != typeof(NetWebScript.Equivalents.ObjectHelper) && type != typeof(NetWebScript.Script.TypeSystemHelper))
            {
                this.baseType = system.GetScriptType(type.BaseType);
                if (this.baseType == null)
                {
                    throw new Exception(string.Format("'{0}' cannot be ScriptAvailable, because its base type '{1}' is not ScriptAvailable, Imported or native.", type.FullName, type.BaseType.FullName));
                }
            }

            // Look for interfaces
            foreach (var itf in type.GetInterfaces())
            {
                system.GetScriptType(itf);
            }

            this.type = type;
            this.typeId = system.CreateTypeId();

            isGlobals = Attribute.IsDefined(type, typeof(GlobalsAttribute));

            foreach (var method in type.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly))
            {
                if (!method.IsGenericMethodDefinition)
                {
                    CreateMethod(method, false);
                }
                else 
                {
                    if (type.IsInterface)
                    {
                        // Generic methods within an interface is not supported.
                        // It would require to force generation of concreate method in all types
                        // implementing interface.
                        throw new NotSupportedException("An interface should have generic method.");
                    }
                }
            }

            foreach (var ctor in type.GetConstructors(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly))
            {
                methods.Add(new ScriptConstructor(system, this, ctor));
            }

            foreach (var field in type.GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly))
            {
                var native = (ScriptBodyAttribute)Attribute.GetCustomAttribute(field, typeof(ScriptBodyAttribute));
                if (native != null)
                {
                    fields.Add(new InlinedField(this, field, native.Inline));
                }
                else
                {
                    fields.Add(new ScriptField(system, this, field));
                }
            }
        }

        internal ScriptInterfaceMapping GetMapping()
        {
            return new ScriptInterfaceMapping(system, this);
        }

        private IScriptMethod CreateMethod(MethodInfo method, bool late)
        {
            IScriptMethod scriptMethod;
            var native = (ScriptBodyAttribute)Attribute.GetCustomAttribute(method, typeof(ScriptBodyAttribute));
            if (native != null && !string.IsNullOrEmpty(native.Inline))
            {
                scriptMethod = new InlinedMethod(this, method, native.Inline);
            }
            else 
            {
                var body = native != null ? native.Body : null;
                var generated = new ScriptMethod(system, this, method, body);
                if (late)
                {
                    system.MethodsToGenerate.Add(generated);
                }
                scriptMethod = generated;
            }
            methods.Add(scriptMethod);
            return scriptMethod;
        }

        public IScriptMethod GetScriptMethod(MethodInfo method)
        {
            var result = (IScriptMethod)GetScriptMethodBase(method);
            if (result == null && method.IsGenericMethod)
            {
                return CreateMethod(method, true);
            }
            return result;
        }

        public IScriptConstructor GetScriptConstructor(ConstructorInfo method)
        {
            return (IScriptConstructor)GetScriptMethodBase(method);
        }

        private IScriptMethodBase GetScriptMethodBase(MethodBase method)
        {
            return methods.FirstOrDefault(m => m.Method == method);
        }

        public IScriptField GetScriptField(FieldInfo field)
        {
            return fields.FirstOrDefault(f => f.Field == field);
        }

        public Type Type
        {
            get { return type; }
        }

        public string TypeId
        {
            get { return typeId; }
        }

        internal IEnumerable<ScriptMethodBase> Methods
        {
            get { return methods.OfType<ScriptMethodBase>(); }
        }

        internal IEnumerable<ScriptField> Fields
        {
            get { return fields.OfType<ScriptField>(); }
        }

        public ITypeBoxing Boxing
        {
            get { return null; }
        }

        public IValueSerializer Serializer
        {
            get { return null; }
        }

        public IScriptType BaseType
        {
            get { return baseType; }
        }

        public IList<IScriptType> Interfaces
        {
            get { return interfaces; }
        }

        public bool IsGlobals
        {
            get { return isGlobals; }
        }

        public bool HaveCastInformation
        {
            get { return true; }
        }
    }
}
