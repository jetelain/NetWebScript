using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using NetWebScript.JsClr.TypeSystem.Invoker;
using NetWebScript.Metadata;
using NetWebScript.JsClr.TypeSystem.Remoting;

namespace NetWebScript.JsClr.TypeSystem
{
    public abstract class ScriptTypeBase : IScriptType
    {
        protected readonly ScriptSystem system;
        protected readonly Type type;
        protected readonly List<IScriptConstructor> constructors = new List<IScriptConstructor>();
        protected readonly List<IScriptMethod> methods = new List<IScriptMethod>();
        protected readonly List<IScriptField> fields = new List<IScriptField>();
        protected readonly List<IScriptType> interfaces = new List<IScriptType>();

        protected readonly List<IScriptType> children = new List<IScriptType>();
        private readonly List<TransparentType> proxies = new List<TransparentType>();
        protected IScriptType baseType;

        protected ScriptTypeBase(ScriptSystem system, Type type)
        {
            this.type = type;
            this.system = system;
        }

        protected void InitInterfaces()
        {
            foreach (var itf in type.GetInterfaces())
            {
                var scriptInterface = system.GetScriptType(itf);
                if (scriptInterface != null)
                {
                    interfaces.Add(scriptInterface);
                    scriptInterface.RegisterChildType(this);
                }
            }
        }

        protected void InitBaseType(bool mayIgnoreDirectBase)
        {
            if (type.BaseType != null
                && type != typeof(NetWebScript.Equivalents.ObjectHelper)
                && type != typeof(NetWebScript.Script.TypeSystemHelper))
            {
                this.baseType = system.GetScriptType(type.BaseType);
                if (this.baseType == null)
                {
                    if (mayIgnoreDirectBase)
                    {
                        var intermediateBase = type.BaseType;
                        while (intermediateBase != null && baseType == null)
                        {
                            baseType = system.GetScriptType(intermediateBase.BaseType);
                            intermediateBase = intermediateBase.BaseType;
                        }
                    }
                    else
                    {
                        throw new Exception(string.Format("'{0}' cannot be ScriptAvailable, because its base type '{1}' is not script available.", type.FullName, type.BaseType.FullName));
                    }
                }
                RegisterBase();
            }
        }

        private void RegisterBase()
        {
            var parent = baseType;
            while (parent != null)
            {
                parent.RegisterChildType(this);
                parent = parent.BaseType;
            }
        }

        public IScriptConstructor GetScriptConstructor(ConstructorInfo ctor)
        {
            if (ctor.DeclaringType != type)
            {
                throw new ArgumentException();
            }
            var info = constructors.FirstOrDefault(f => f.Method == ctor);
            if (info == null)
            {
                if (system.IsSealed)
                {
                    throw new InvalidOperationException(string.Format("Late discovering of '{0}' declared by type '{1}'.", ctor, type.FullName));
                }
                info = CreateScriptConstructor(ctor);
                if (info != null)
                {
                    constructors.Add(info);
                }
            }
            return info;
        }

        public IScriptMethod GetScriptMethod(MethodInfo method)
        {
            var info = GetScriptMethodIfUsed(method);
            if (info == null)
            {
                if (system.IsSealed)
                {
                    throw new InvalidOperationException(string.Format("Late discovering of '{0}' declared by type '{1}'.", method, type.FullName));
                }
                info = CreateScriptMethod(method);
                if (info != null)
                {
                    methods.Add(info);
                    if (method.IsVirtual)
                    {
                        // If type has childs, it must force them to have their eventual override
                        EnsureChildrenOverrides(method);
                    }
                    if (!method.IsStatic)
                    {
                        EnsureProxies(info);
                    }
                }
            }
            return info;
        }


        public IScriptMethod GetScriptMethodIfUsed(MethodInfo method)
        {
            if (method.DeclaringType != type)
            {
                throw new ArgumentException();
            }
            if (method.IsGenericMethodDefinition)
            {
                throw new ArgumentException();
            }
            if (method.ReflectedType != method.DeclaringType)
            {
                method = method.DeclaringType.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly).First(m => m.MethodHandle == method.MethodHandle);
            }

            return methods.FirstOrDefault(f => f.Method == method);
        }

        private void EnsureChildrenOverrides(MethodInfo virtualMethod)
        {
            Contract.Requires(virtualMethod.IsVirtual);
            if (type.IsInterface)
            {
                foreach (var child in children)
                {
                    EnsureChildImplments(child, virtualMethod);
                }
            }
            else
            {
                foreach (var child in children)
                {
                    EnsureChildDirectOverrides(child, virtualMethod);
                }
            }
        }

        private void EnsureChildOverrides(IScriptType child, MethodInfo virtualMethod)
        {
            Contract.Requires(virtualMethod.IsVirtual);
            if (type.IsInterface)
            {
                EnsureChildImplments(child, virtualMethod);
            }
            else
            {
                EnsureChildDirectOverrides(child, virtualMethod);
            }
        }

        private const BindingFlags OverrideInstanceMethods = 
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;

        private void EnsureChildDirectOverrides(IScriptType child, MethodInfo virtualMethod)
        {
            Contract.Requires(virtualMethod.IsVirtual);
            Contract.Requires(!type.IsInterface);
            var method = child.Type.GetMethods(OverrideInstanceMethods).FirstOrDefault(m => m.GetBaseDefinition() == virtualMethod);
            if (method != null)
            {
                child.GetScriptMethod(method);
            }
        }

        private void EnsureChildImplments(IScriptType child, MethodInfo virtualMethod)
        {
            Console.WriteLine("EnsureChildrenImplments {0}", virtualMethod);
            Contract.Requires(virtualMethod.IsVirtual);
            Contract.Requires(type.IsInterface);

            if (!child.Type.IsInterface)
            {
                var map = child.Type.GetInterfaceMap(type);
                for (int i = 0; i < map.InterfaceMethods.Length; ++i)
                {
                    if (map.InterfaceMethods[i] == virtualMethod)
                    {
                        var targetMethod = map.TargetMethods[i];
                        if (targetMethod.DeclaringType == child.Type)
                        {
                            child.GetScriptMethod(targetMethod);
                        }
                        else
                        {
                            system.GetScriptMethod(targetMethod);
                        }
                    }
                }
            }
        }

        private void EnsureProxies(IScriptMethod method)
        {
            foreach (var proxy in proxies)
            {
                proxy.CreateProxyMethod(method);
            }
        }

        public IScriptField GetScriptField(FieldInfo field)
        {
            if (field.DeclaringType != type)
            {
                throw new ArgumentException();
            }
            IScriptField info = fields.FirstOrDefault(f => f.Field == field);
            if (info == null)
            {
                if (system.IsSealed)
                {
                    throw new InvalidOperationException(string.Format("Late discovering of '{0}' declared by type '{1}'.", field, type.FullName));
                }
                info = CreateScriptField(field);
                if (info != null)
                {
                    fields.Add(info);
                }
            }
            return info;
        }

        public Type Type
        {
            get { return type; }
        }

        public IScriptConstructor DefaultConstructor
        {
            get
            {
                var ctor = type.GetConstructor(Type.EmptyTypes);
                if (ctor != null)
                {
                    return GetScriptConstructor(ctor);
                }
                return null;
            }
        }

        protected abstract IScriptConstructor CreateScriptConstructor(ConstructorInfo ctor);

        protected abstract IScriptMethod CreateScriptMethod(MethodInfo method);

        protected abstract IScriptField CreateScriptField(FieldInfo field);

        public abstract string TypeId { get; }

        public abstract ITypeBoxing Boxing { get; }

        public abstract IValueSerializer Serializer { get; }

        public abstract bool HaveCastInformation { get; }

        public abstract TypeMetadata Metadata { get; }

        public void RegisterChildType(IScriptType child)
        {
            children.Add(child);
            foreach (var method in methods)
            {
                if (method.Method.IsVirtual)
                {
                    EnsureChildOverrides(child, (MethodInfo)method.Method);
                }
            }
        }

        public IScriptType BaseType
        {
            get { return baseType; }
        }

        public string DisplayName
        {
            get { return type.Name; }
        }

        public void RegisterProxyType(TransparentType transparentProxyScriptType)
        {
            proxies.Add(transparentProxyScriptType);
            foreach (var method in methods)
            {
                if (!method.IsStatic)
                {
                    transparentProxyScriptType.CreateProxyMethod(method);
                }
            }
        }

        private TransparentType transparentProxy;

        public TransparentType TransparentProxy
        {
            get
            {
                if (transparentProxy == null)
                {
                    transparentProxy = new TransparentType(system, this);
                }
                return transparentProxy;
            }
        }
    }
}
