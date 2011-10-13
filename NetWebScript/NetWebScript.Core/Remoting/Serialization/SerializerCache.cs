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

        public SerializerCache(params ModuleMetadata[] modules)
        {
            scriptModules.AddRange(modules);

            var meta = GetTypeMetadata(typeof(RemoteInvoker));
            if (meta != null)
            {
                var key = CRefToolkit.GetCRef(typeof(RemoteInvoker).GetMethod("CopyTo"));
                var methodMeta = meta.Methods.FirstOrDefault(m => m.CRef == key);
                if (methodMeta != null)
                {
                    copyMethod = meta.Name + "." + methodMeta.Name;
                }
            }
            converter = new Converter(this);
        }

        public TypeMetadata GetTypeMetadata(string name)
        {
            return scriptModules.SelectMany(m => m.Types.Where(t => t.Name == name)).FirstOrDefault();
        }

        public TypeMetadata GetTypeMetadata(Type type)
        {
            var key = CRefToolkit.GetCRef(type);
            return scriptModules.SelectMany(m => m.Types.Where(t => t.CRef == key)).FirstOrDefault();
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
            var scriptType = GetTypeMetadata(type);
            if (scriptType != null)
            {
                return CreateSerializer(scriptType, type);
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
    }
}
