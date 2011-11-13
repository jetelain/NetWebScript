using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NetWebScript.JsClr.AstBuilder;
using NetWebScript.JsClr.TypeSystem.Anonymous;
using NetWebScript.JsClr.TypeSystem.Helped;
using NetWebScript.JsClr.TypeSystem.Imported;
using NetWebScript.JsClr.TypeSystem.Native;
using NetWebScript.JsClr.TypeSystem.Standard;
using NetWebScript.Script;

namespace NetWebScript.JsClr.TypeSystem
{
    public class ScriptSystem
    {
        private readonly string moduleId;
        private readonly IdentifierGenerator slot = new IdentifierGenerator();
        private readonly IdentifierGenerator impl = new IdentifierGenerator();
        private readonly IdentifierGenerator type = new IdentifierGenerator();
        private readonly List<IScriptType> types = new List<IScriptType>();
        private readonly List<ScriptType> typesToGenerate = new List<ScriptType>();
        private readonly List<ScriptMethod> methodsToGenerate = new List<ScriptMethod>();
        private readonly List<ScriptEnumType> enumsToGenerate = new List<ScriptEnumType>();
        private readonly List<ScriptTypeHelped> equivalents = new List<ScriptTypeHelped>();
        private readonly HashSet<Type> implicitScriptAvailable = new HashSet<Type>();
        private readonly Dictionary<Type, Type> scriptEquivalent = new Dictionary<Type, Type>();

        public ScriptSystem(string moduleId) 
        {
            this.moduleId = moduleId;
        }

        public ScriptSystem() : this("a")
        {

        }

        internal string ModuleId
        {
            get { return moduleId; }
        }

        internal string CreateTypeId()
        {
            return moduleId+ "T" + type.TakeOne();
        }

        internal string CreateImplementationId()
        {
            return moduleId + "I" + impl.TakeOne();
        }

        internal string CreateSplotId()
        {
            return moduleId + "S" + slot.TakeOne();
        }

        protected virtual IScriptType CreateType(Type type)
        {
            if (type == typeof(string))
            {
                return new StringType(this);
            }
            if (type == typeof(object))
            {
                return new ObjectType(this);
            }
            if (type == typeof(char))
            {
                return new CharType();
            }
            if (type == typeof(int) || type == typeof(double) || type == typeof(float) || type == typeof(long) || type == typeof(short) || type == typeof(byte))
            {
                return new NumberType(this, type);
            }
            if (type == typeof(bool))
            {
                return new BooleanType();
            }
            if (typeof(Delegate).IsAssignableFrom(type))
            {
                return new DelegateType(this, type);
            }
            if (typeof(Type).IsAssignableFrom(type)/* || typeof(MethodInfo).IsAssignableFrom(type)*/)
            {
                return new FunctionType(this, type);
            }
            if (typeof(MethodInfo).IsAssignableFrom(type))
            {
                var helped = new ScriptTypeHelped(this, type, typeof(Equivalents.Reflection.MethodInfo));
                equivalents.Add(helped);
                helped.Serializer = new FunctionType(this, type);
                return helped;
            }
            var imported = (ImportedAttribute)Attribute.GetCustomAttribute(type, typeof(ImportedAttribute), false);
            if (imported != null)
            {
                return new ImportedType(this, type, imported);
            }
            var anonymous = (AnonymousObjectAttribute)Attribute.GetCustomAttribute(type, typeof(AnonymousObjectAttribute));
            if (anonymous != null)
            {
                return new AnonymousType(type, anonymous.Convention);
            }
            if (IsScriptAvailable(type))
            {
                if (type.IsEnum)
                {
                    var scriptEnumType = new ScriptEnumType(this, type);
                    enumsToGenerate.Add(scriptEnumType);
                    return scriptEnumType;
                }
                var scriptType = new ScriptType(this, type);
                typesToGenerate.Add(scriptType);
                return scriptType;
            }
            Type helperType = GetEquivalent(type);
            if (helperType != null)
            {
                var helped = new ScriptTypeHelped(this, type, helperType);
                equivalents.Add(helped);
                
                return helped;
            }
            if (type.IsArray)
            {
                if (GetScriptType(type.GetElementType()) != null)
                {
                    return new ArrayType(this, type);
                }
            }
            return null;
        }

        private Type GetEquivalent(Type type)
        {
            Type helperType;
            if (scriptEquivalent.TryGetValue(type, out helperType))
            {
                return helperType;
            }
            if (type.IsGenericType)
            {
                if (scriptEquivalent.TryGetValue(type.GetGenericTypeDefinition(), out helperType))
                {
                    return helperType.MakeGenericType(type.GetGenericArguments());
                }
            }
            if ( type.DeclaringType != null )
            {
                Type declaring = GetEquivalent(type.DeclaringType);
                if (declaring != null)
                {
                    if (type.IsGenericType)
                    {
                        return declaring.GetNestedType(type.Name).MakeGenericType(type.GetGenericArguments());
                    }
                    else
                    {
                        return declaring.GetNestedType(type.Name);
                    }
                }
            }
            return null;
        }

        private bool IsScriptAvailable(Type type)
        {
            if (Attribute.IsDefined(type, typeof(ScriptAvailableAttribute)) || Attribute.IsDefined(type.Assembly, typeof(ScriptAvailableAttribute)))
            {
                return true;
            }
            if (type.DeclaringType != null && IsScriptAvailable(type.DeclaringType))
            {
                return true;
            }
            if (implicitScriptAvailable.Contains(type))
            {
                return true;
            }
            if (type.IsGenericType && implicitScriptAvailable.Contains(type.GetGenericTypeDefinition()))
            {
                return true;
            }
            return false;
        }

        public IScriptType GetScriptType(Type type)
        {
            if (type.IsGenericTypeDefinition || type.ContainsGenericParameters)
            {
                throw new Exception("You should never do this");
            }

            var scriptType = types.FirstOrDefault(t => t.Type == type);
            if (scriptType != null) {
                return scriptType;
            }
            scriptType = CreateType(type);
            if (scriptType != null) {
                types.Add(scriptType);
            }
            return scriptType;
        }

        internal string GetSlotId(MethodInfo baseMethod)
        {
            var type = GetScriptType(baseMethod.DeclaringType);
            if (type != null)
            {
                var method = type.GetScriptMethod(baseMethod);
                if (method != null)
                {
                    return method.SlodId;
                }
            }
            return null;
        }

        internal List<ScriptType> TypesToGenerate
        {
            get { return typesToGenerate; }
        }

        internal IEnumerable<ImportedType> ImportedTypes
        {
            get { return types.OfType<ImportedType>(); }
        }

        internal List<ScriptEnumType> EnumToGenerate
        {
            get { return enumsToGenerate; }
        }
        internal List<ScriptMethod> MethodsToGenerate
        {
            get { return methodsToGenerate; }
        }
        internal List<ScriptTypeHelped> Equivalents
        {
            get { return equivalents; }
        }

        internal IScriptMethodBase GetScriptMethodBase(MethodBase methodBase)
        {
            var info = methodBase as MethodInfo;
            if (info != null)
            {
                return GetScriptMethod(info);
            }
            return GetScriptConstructor((ConstructorInfo)methodBase);
        }

        internal IScriptMethodBase GetScriptMethod(MethodInfo methodInfo)
        {
            var type = GetScriptType(methodInfo.DeclaringType);
            if (type != null)
            {
                var method = type.GetScriptMethod(methodInfo);
                if (method != null)
                {
                    return method;
                }
            }
            return null;
        }

        internal IScriptConstructor GetScriptConstructor(ConstructorInfo constructorInfo)
        {
            var type = GetScriptType(constructorInfo.DeclaringType);
            if (type != null)
            {
                var method = type.GetScriptConstructor(constructorInfo);
                if (method != null)
                {
                    return method;
                }
            }
            return null;
        }

        internal IScriptField GetScriptField(FieldInfo fieldInfo)
        {
            var type = GetScriptType(fieldInfo.DeclaringType);
            if (type != null)
            {
                var field = type.GetScriptField(fieldInfo);
                if (field != null)
                {
                    return field;
                }
            }
            return null;
        }

        public void ImportAssemblyMappings(Assembly assembly)
        {
            foreach (ForceScriptAvailableAttribute force in Attribute.GetCustomAttributes(assembly, typeof(ForceScriptAvailableAttribute)))
            {
                implicitScriptAvailable.Add(force.Type);
            }
            foreach (Type type in assembly.GetTypes())
            {
                var attr = (ScriptEquivalentAttribute)Attribute.GetCustomAttribute(type, typeof(ScriptEquivalentAttribute), false);
                if (attr != null)
                {
                    if (scriptEquivalent.ContainsKey(attr.Type))
                    {
                        throw new Exception(string.Format("Type '{0}' has more than one equivalent : at least '{1}' and '{2}'", attr.Type, type.FullName, scriptEquivalent[attr.Type].FullName));
                    }
                    else
                    {
                        scriptEquivalent.Add(attr.Type, type);
                    }
                }
            }
        }

        public virtual MethodAst GetMethodAst(IScriptMethodBase scriptMethod, MethodBase methodBase)
        {
            return MethodAst.GetMethodAst(methodBase);
        }
    }
}
