using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NetWebScript.JsClr.AstBuilder;
using NetWebScript.JsClr.TypeSystem.Helped;
using NetWebScript.JsClr.TypeSystem.Imported;
using NetWebScript.JsClr.TypeSystem.Standard;
using NetWebScript.Metadata;
using NetWebScript.JsClr.ScriptWriter.Declaration;

namespace NetWebScript.JsClr.TypeSystem
{
    public class ScriptSystem
    {
        private readonly ModuleMetadata moduleMetadata;

        private readonly StandardScriptTypeProvider stdProvider;
        private readonly EquivalentScriptTypeProvider equivProvider;
        private readonly EnumScriptTypeProvider enumProvider;
        private readonly List<IScriptTypeProvider> providers = new List<IScriptTypeProvider>();
             
        private readonly IdentifierGenerator slot = new IdentifierGenerator();
        private readonly IdentifierGenerator impl = new IdentifierGenerator();
        private readonly IdentifierGenerator type = new IdentifierGenerator();
        private readonly List<IScriptType> types = new List<IScriptType>();
        private readonly List<IScriptTypeDeclarationWriter> typesToWrite = new List<IScriptTypeDeclarationWriter>();

        private readonly Queue<ScriptMethodBase> astToGenerate = new Queue<ScriptMethodBase>();
        private bool isSealed = false;


        public ScriptSystem()
        {
            moduleMetadata = new ModuleMetadata();
            moduleMetadata.Name = "a";

            providers.Add(new NativeScriptTypeProvider(this));

            enumProvider = new EnumScriptTypeProvider(this);
            providers.Add(enumProvider);

            providers.Add(new AnonymousScriptTypeProvider(this));

            providers.Add(new ImportedScriptTypeProvider(this));

            stdProvider = new StandardScriptTypeProvider(this);
            providers.Add(stdProvider);

            equivProvider = new EquivalentScriptTypeProvider(this);
            providers.Add(equivProvider);
        }

        internal ModuleMetadata Metadata 
        {
            get { return moduleMetadata; }
        }

        internal string CreateTypeId()
        {
            return moduleMetadata.Name + "T" + type.TakeOne();
        }

        internal string CreateImplementationId()
        {
            return moduleMetadata.Name + "I" + impl.TakeOne();
        }

        internal string CreateSplotId()
        {
            return moduleMetadata.Name + "S" + slot.TakeOne();
        }

        internal static bool IsNumberType ( Type type )
        {
            return 
                type == typeof(double) ||
                type == typeof(float) ||
                type == typeof(long) ||
                type == typeof(ulong) ||
                type == typeof(int) ||
                type == typeof(uint) ||
                type == typeof(short) ||
                type == typeof(ushort) || 
                type == typeof(sbyte) ||
                type == typeof(byte);
        }

        private IScriptType CreateType(Type type)
        {
            IScriptType scriptType;
            foreach (var provider in providers)
            {
                if (provider.TryCreate(type, out scriptType))
                {
                    return scriptType;
                }
            }
            return null;
        }

        public IScriptType GetScriptType(Type type)
        {
            if (type.IsGenericTypeDefinition || type.ContainsGenericParameters)
            {
                throw new ArgumentException("Only closed constructed types can have a script version.", "type");
            }

            var scriptType = types.FirstOrDefault(t => t.Type == type);
            if (scriptType != null) 
            {
                return scriptType;
            }
            if (!isSealed)
            {
                scriptType = CreateType(type);
                if (scriptType != null)
                {
                    types.Add(scriptType);
                }
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

        internal IEnumerable<ScriptType> TypesToWrite
        {
            get { return stdProvider.TypesToWrite; }
        }

        internal IEnumerable<IScriptTypeDeclarationWriter> TypesToDeclare
        {
            get { return typesToWrite.Where(t => !t.IsEmpty); }
        }

        internal Queue<ScriptMethodBase> AstToGenerate
        {
            get { return astToGenerate; }
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

        internal IScriptMethod GetScriptMethod(MethodInfo methodInfo)
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
            foreach (var provider in providers)
            {
                provider.RegisterAssembly(assembly);
            }
        }

        public void AddTypeToWrite(IScriptTypeDeclarationWriter type)
        {
            typesToWrite.Add(type);
        }

        public virtual MethodAst GetMethodAst(IScriptMethodBase scriptMethod, MethodBase methodBase)
        {
            return MethodAst.GetMethodAst(methodBase);
        }

        internal bool IsSealed
        {
            get { return isSealed; }
        }

        internal void Seal()
        {
            isSealed = true;
        }
    }
}
