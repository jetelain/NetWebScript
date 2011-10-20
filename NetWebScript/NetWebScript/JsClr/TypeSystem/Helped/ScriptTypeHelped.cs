using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.JsClr.TypeSystem.Invoker;
using System.Reflection;
using System.Diagnostics.Contracts;

namespace NetWebScript.JsClr.TypeSystem.Helped
{
    class ScriptTypeHelped : IScriptType
    {
        private readonly Type type;
        private readonly IScriptType helper;
        private readonly List<ScriptMethodHelped> methods = new List<ScriptMethodHelped>();
        private readonly List<ScriptContructorHelped> constructors = new List<ScriptContructorHelped>();
        private readonly List<ScriptFieldHelped> fields = new List<ScriptFieldHelped>();

        public ScriptTypeHelped(ScriptSystem system, Type type, Type helperType)
        {
            this.type = type;
            this.helper = system.GetScriptType(helperType);
            if (this.helper == null)
            {
                throw new Exception(string.Format("Type '{0}' asked by '{1}' must be script available.", type.FullName, helperType.FullName));
            }
            Serializer = helper.Serializer;
        }

        #region IScriptType Members

        public IScriptConstructor GetScriptConstructor(System.Reflection.ConstructorInfo method)
        {
            var scriptCtor = constructors.FirstOrDefault(m => m.Method == method);
            if (scriptCtor == null)
            {
                scriptCtor = CreateScriptConstructorHelper(method, this, helper);
                if (scriptCtor != null)
                {
                    constructors.Add(scriptCtor);
                }
            }
            return scriptCtor;
        }

        public IScriptMethod GetScriptMethod(MethodInfo method)
        {
            Contract.Requires(method.DeclaringType == type);
            var scriptMethod = methods.FirstOrDefault(m => m.Method == method);
            if (scriptMethod == null)
            {
                scriptMethod = CreateScriptMethodHelper(method, this, helper);
                if (scriptMethod != null)
                {
                    methods.Add(scriptMethod);
                }
            }
            return scriptMethod;
        }

        //public IScriptMethodBase GetScriptMethodBase(System.Reflection.MethodBase method)
        //{
        //    Contract.Requires(method.DeclaringType == type);
        //    var info = method as MethodInfo;
        //    if (info != null)
        //    {
        //        return GetScriptMethod(info);
        //    }
        //    return GetScriptConstructor((ConstructorInfo)method);
        //}

        public IScriptField GetScriptField(System.Reflection.FieldInfo field)
        {
            Contract.Requires(field.DeclaringType == type);
            var scriptField = fields.FirstOrDefault(m => m.Field == field);
            if (scriptField == null)
            {
                scriptField = CreateScriptFieldHelper(field, this, helper);
                if (scriptField != null)
                {
                    fields.Add(scriptField);
                }
            }
            return scriptField;
        }

        
        public Type Type
        {
            get { return type; }
        }
        
        public IScriptType Equivalent
        {
            get { return helper; }
        }

        public virtual string TypeId
        {
            get { return helper.TypeId; }
        }

        public virtual ITypeBoxing Boxing
        {
            get { return helper.Boxing; }
        }

        #endregion

        private static MethodInfo EquivalentMethod(Type owner, Type helper, MethodInfo method)
        {
            var parameters = method.GetParameters().Select(p => p.ParameterType).ToArray();
            var helperMethod = helper.GetMethod(method.Name, (method.IsStatic ? BindingFlags.Static : BindingFlags.Instance) | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly, null, parameters, null);
            if (helperMethod == null && !method.IsStatic)
            {
                List<Type> parametersList = new List<Type>();
                parametersList.Add(owner);
                parametersList.AddRange(parameters);
                helperMethod = helper.GetMethod(method.Name, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly, null, parametersList.ToArray(), null);
            }
            return helperMethod;
        }

        private static MethodInfo EquivalentGenericMethodDefinition(Type helper, MethodInfo genericMethodDefinition)
        {
            var methodArgs = genericMethodDefinition.GetGenericArguments();
            var methodParams = genericMethodDefinition.GetParameters();
            foreach(var method in helper.GetMethods((genericMethodDefinition.IsStatic ? BindingFlags.Static : BindingFlags.Instance) | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly))
            {
                if ( method.IsGenericMethodDefinition && method.Name == genericMethodDefinition.Name)
                {
                    var candidateArgs = method.GetGenericArguments();
                    var candidateParams = method.GetParameters();
                    if ( methodArgs.Length == candidateArgs.Length && candidateParams.Length == methodParams.Length )
                    {
                        // Couples des arguments générique
                        var argsTuples = methodArgs.Zip(candidateArgs, (a, b) => new Tuple<Type, Type>(a, b)).ToList();

                        // Couples des types des paramètres
                        var paramsTuples = methodParams.Zip(candidateParams, (a, b) => new Tuple<Type,Type>(a.ParameterType, b.ParameterType));

                        // Pour tout couple de paramètre leur type doit être égal ou 
                        // il doit exister un couple parmis les arguments génériques 
                        // qui corresponds à leur types respectifs
                        if (paramsTuples.All(tparam => tparam.Item1 == tparam.Item2 || argsTuples.Any(targ => tparam.Item1 == targ.Item1 && tparam.Item2 == targ.Item2)))
                        {
                            return method;
                        }
                    }
                }
            }
            return null;
        }

        internal static ScriptMethodHelped CreateScriptMethodHelper(MethodInfo method, IScriptType owner, IScriptType helperType)
        {
            MethodInfo helperMethod = null;
            if ( method.IsGenericMethod )
            {
                var genericHelperMethod = EquivalentGenericMethodDefinition(helperType.Type, method.GetGenericMethodDefinition());
                if (genericHelperMethod != null)
                {
                    helperMethod = genericHelperMethod.MakeGenericMethod(method.GetGenericArguments());
                }
            }
            else
            {
                helperMethod = EquivalentMethod(owner.Type, helperType.Type, method);
            }

            if (helperMethod != null)
            {
                var scriptHelperMethod = helperType.GetScriptMethod(helperMethod);
                if (scriptHelperMethod != null)
                {
                    return new ScriptMethodHelped(owner, method, scriptHelperMethod);
                }
            }
            return null;
        }


        internal static ScriptContructorHelped CreateScriptConstructorHelper(ConstructorInfo ctor, IScriptType owner, IScriptType helperType)
        {
            var helperCtor = helperType.Type.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly, null, ctor.GetParameters().Select(p => p.ParameterType).ToArray(), null);
            if (helperCtor != null)
            {
                var scriptHelperCtor = helperType.GetScriptConstructor(helperCtor);
                if (scriptHelperCtor != null)
                {
                    return new ScriptContructorHelped(owner, ctor, scriptHelperCtor);
                }
            }
            return null;
        }

        internal static ScriptFieldHelped CreateScriptFieldHelper(FieldInfo field, ScriptTypeHelped owner, IScriptType helperType)
        {
            var helperField = helperType.Type.GetField(field.Name, (field.IsStatic ? BindingFlags.Static : BindingFlags.Instance) | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
            if (helperField != null)
            {
                var scriptHelperField = helperType.GetScriptField(helperField);
                if (scriptHelperField != null)
                {
                    return new ScriptFieldHelped(owner, field, scriptHelperField);
                }
            }
            return null;
        }

        public bool HaveCastInformation
        {
            get { return helper.HaveCastInformation; }
        }

        public virtual IValueSerializer Serializer
        {
            get;
            set;
        }
    }
}
