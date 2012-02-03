using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.JsClr.TypeSystem.Invoker;
using System.Reflection;
using System.Diagnostics.Contracts;
using NetWebScript.JsClr.TypeSystem.Serializers;
using NetWebScript.Metadata;

namespace NetWebScript.JsClr.TypeSystem.Helped
{
    class ScriptTypeHelped : ScriptTypeBase
    {
        protected readonly IScriptType helper;
        protected readonly IValueSerializer serializer;
        protected readonly ITypeBoxing boxing;

        public ScriptTypeHelped(ScriptSystem system, Type type, Type helperType) : base(system, type)
        {
            this.helper = system.GetScriptType(helperType);
            if (this.helper == null)
            {
                throw new Exception(string.Format("Type '{0}' asked by '{1}' must be script available.", type.FullName, helperType.FullName));
            }
            serializer = DefaultSerializer.GetSerializer(system, type) ?? helper.Serializer;
            if (type.IsValueType)
            {
                if (Serializer != null)
                {
                    boxing = new TypeBoxing(helperType);
                }
                else
                {
                    boxing = new NoneTypeBoxing();
                }
            }

            InitBaseType(true);
            InitInterfaces();
        }

        #region IScriptType Members

        protected sealed override IScriptConstructor CreateScriptConstructor(ConstructorInfo ctor)
        {
            return CreateScriptConstructorHelper(ctor, this, helper);
        }

        protected sealed override IScriptField CreateScriptField(FieldInfo field)
        {
            return CreateScriptFieldHelper(field, this, helper);
        }

        protected sealed override IScriptMethod CreateScriptMethod(MethodInfo method)
        {
            return CreateScriptMethodHelper(method, this, helper);
        }

        public IScriptType Equivalent
        {
            get { return helper; }
        }

        public override string TypeId
        {
            get { return helper.TypeId; }
        }

        public override ITypeBoxing Boxing { get { return boxing; } }

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
                        if (paramsTuples.All(tparam => IsSameType(tparam.Item1, tparam.Item2, argsTuples)))
                        {
                            return method;
                        }
                    }
                }
            }
            return null;
        }

        private static bool IsSameType(Type a, Type b, IEnumerable<Tuple<Type, Type>> equalities)
        {
            if (a == b)
            {
                return true;
            }
            if (equalities.Any(targ => a == targ.Item1 && b == targ.Item2))
            {
                return true;
            }
            if (a.IsGenericType && b.IsGenericType)
            {
                var defA = a.GetGenericTypeDefinition();
                var defB = b.GetGenericTypeDefinition();
                if (IsSameType(defA, defB, equalities))
                {
                    return defA.GetGenericArguments().Zip(defB.GetGenericArguments(), (subA, subB) => IsSameType(subA, subB, equalities)).All(i => i);
                }
            }
            return false;
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
                else
                {

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

        public override bool HaveCastInformation
        {
            get { return helper.HaveCastInformation; }
        }

        public override IValueSerializer Serializer
        {
            get { return serializer; }
        }

        public override TypeMetadata Metadata
        {
            get { return null; }
        }

    }
}
