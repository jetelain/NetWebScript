using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.Metadata;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Serialization;
using NetWebScript;
using NetWebScript.Script;

namespace NetWebScript.Remoting.Serialization
{
    public sealed class SerializerCache
    {
        private readonly Dictionary<Type, IObjectSerializer> serializers = new Dictionary<Type, IObjectSerializer>();

        private readonly List<ModuleMetadata> scriptModules = new List<ModuleMetadata>();
        private readonly string copyMethod;
        private readonly Converter converter;

        [CLSCompliant(false)]
        public SerializerCache(params ModuleMetadata[] modules)
        {
            scriptModules.AddRange(modules);

            var meta = GetTypeMetadataByCRef(CRefToolkit.GetCRef(typeof(XmlSerializer)));
            if (meta != null)
            {
                var key = CRefToolkit.GetCRef(typeof(XmlSerializer).GetMethod("CopyTo"));
                var methodMeta = meta.Methods.FirstOrDefault(m => m.CRef == key);
                if (methodMeta != null)
                {
                    copyMethod = meta.Name + "." + methodMeta.Name;
                }
            }
            converter = new Converter(this);
        }

        [CLSCompliant(false)]
        public TypeMetadata GetTypeMetadataByScriptName(string name)
        {
            return scriptModules.SelectMany(m => m.Types.Where(t => t.Name == name)).FirstOrDefault();
        }

        [CLSCompliant(false)]
        public TypeMetadata GetTypeMetadataByCRef(string cref)
        {
            return scriptModules.SelectMany(m => m.Types.Where(t => t.CRef == cref)).FirstOrDefault();
        }

        private IObjectSerializer CreateSerializer(Type type)
        {
            if (type == typeof(JSObject))
            {
                return UnknownObjectSerializer.Instance;
            }
            var attribute = (AnonymousObjectAttribute)Attribute.GetCustomAttribute(type, typeof(AnonymousObjectAttribute));
            if (attribute != null)
            {
                return CreateSerializer(attribute, type);
            }
            var cref = CRefToolkit.GetCRef(type);
            var scriptType = GetTypeMetadataByCRef(cref);
            if (scriptType != null)
            {
                return CreateSerializer(scriptType, type);
            }
            var equiv = scriptModules.SelectMany(m => m.Equivalents.Where(t => t.CRef == cref)).FirstOrDefault();
            if (equiv != null)
            {
                var equivalentType = CRefToolkit.ResolveType(equiv.EquivalentCRef);
                var equivalentSerializer = GetSerializer(equivalentType);
                if (equivalentSerializer != null)
                {
                    return new EquivalentSerializer(type, equivalentType, equivalentSerializer);
                }
            }
            var baseType = type.BaseType;
            if (baseType != null)
            {
                return GetSerializer(baseType);
            }
            return null;
        }

        private IObjectSerializer CreateSerializer(TypeMetadata scriptType, Type type)
        {
            IObjectSerializer parent = null;
            if (type.BaseType != typeof(object))
            {
                parent = GetSerializer(type.BaseType);
            }
            return new ScriptTypeSerializer(copyMethod, parent, scriptType, type);
        }

        private IObjectSerializer CreateSerializer(AnonymousObjectAttribute attribute, Type type)
        {
            IObjectSerializer parent = null;
            if (type.BaseType != typeof(object))
            {
                parent = GetSerializer(type.BaseType);
            }
            return new AnonymousSerializer(parent, attribute, type);
        }

        public IObjectSerializer GetSerializer(Type type)
        {
            IObjectSerializer serializer;
            if (!serializers.TryGetValue(type, out serializer))
            {
                serializer = CreateSerializer(type);
                serializers.Add(type, serializer);
            }
            return serializer;
        }

        public Converter Converter { get { return converter; } }

        internal List<ModuleMetadata> ScriptModules
        {
            get { return scriptModules; }
        }
    }
}
