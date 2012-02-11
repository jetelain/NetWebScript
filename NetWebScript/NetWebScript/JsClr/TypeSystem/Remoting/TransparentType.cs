using System.Collections.Generic;
using System.Linq;
using NetWebScript.JsClr.ScriptAst;
using NetWebScript.JsClr.ScriptWriter.Declaration;
using NetWebScript.JsClr.TypeSystem.Standard;
using NetWebScript.Remoting;
using System.Reflection;

namespace NetWebScript.JsClr.TypeSystem.Remoting
{
    public class TransparentType : IScriptTypeDeclaration, IScriptTypeDeclarationWriter, IInvocableType
    {
        private static readonly MethodInfo ScriptRealProxyInvoke = typeof(ScriptRealProxy).GetMethod("Invoke", new[] { typeof(string), typeof(string), typeof(object[]) });

        private readonly ScriptSystem system;
        private readonly string proxyTypeId;
        private readonly IScriptType proxied;
        private readonly List<TransparentMethod> methods = new List<TransparentMethod>();
        private readonly TransparentConstructor constructor;
        private readonly TransparentField realProxyField;
        private readonly List<string> interfacesTypeIds = new List<string>();

        internal TransparentType ( ScriptSystem system, IScriptType proxied )
        {
            this.system = system;
            this.proxyTypeId = system.CreateTypeId();
            this.proxied = proxied;

            realProxyField = new TransparentField(this);
            constructor = new TransparentConstructor(this);

            foreach (var itf in proxied.Type.GetInterfaces())
            {
                var scriptInterface = system.GetScriptType(itf);
                if (scriptInterface != null)
                {
                    interfacesTypeIds.Add(scriptInterface.TypeId);
                }
            }

            if (proxied.Type.IsInterface)
            {
                interfacesTypeIds.Add(proxied.TypeId);
            }

            var parent = proxied;
            while (parent != null)
            {
                parent.RegisterProxyType(this);
                parent = parent.BaseType;
            }

            system.AddTypeToWrite(this);
        }


        public void CreateProxyMethod(IScriptMethod method)
        {
            if (method.ImplId != null || method.SlodId != null)
            {
                methods.Add(new TransparentMethod(this, method, system.GetScriptMethod(ScriptRealProxyInvoke)));
            }
            // TODO: check that method does not already exists

        }

        public bool CreateType
        {
            get { return true; }
        }

        public string PrettyName
        {
            get { return string.Format("{0}<>Proxy", proxied.Type.FullName); }
        }

        public string TypeId
        {
            get { return proxyTypeId; }
        }

        public string BaseTypeId
        {
            get 
            {
                if (proxied.Type.IsInterface)
                {
                    return "Object";
                }
                return proxied.TypeId;
            }
        }

        public IEnumerable<string> InterfacesTypeIds
        {
            get { return interfacesTypeIds; }
        }

        public IEnumerable<IScriptMethodBaseDeclaration> Constructors
        {
            get { yield return constructor; }
        }

        public IEnumerable<IScriptMethodDeclaration> Methods
        {
            get { return methods; }
        }

        public IEnumerable<IScriptFieldDeclaration> Fields
        {
            get { return Enumerable.Empty<IScriptFieldDeclaration>(); }
        }

        public IEnumerable<ScriptSlotImplementation> InterfacesMapping
        {
            get { return new ScriptInterfaceMapping(system,proxied).InterfacesMapping; }
        }


        public string DisplayName
        {
            get { return string.Format("{0}<>Proxy", proxied.Type.Name); }
        }

        public IInvocableConstructor Constructor
        {
            get
            {
                return constructor;
            }
        }

        public IInvocableField RealProxyField { get { return realProxyField; } }


        public bool IsEmpty
        {
            get { return false; }
        }

        public void WriteDeclaration(System.IO.TextWriter writer, WriterContext context)
        {
            context.StandardDeclarationWriter.WriteDeclaration(writer, context, this);
        }

        internal IScriptType Proxied
        {
            get { return proxied; }
        }
    }
}
