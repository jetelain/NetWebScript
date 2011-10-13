using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NetWebScript;
using NetWebScript.Script;
using NetWebScript.Metadata;

namespace NetWebScript.Remoting.Serialization
{
    public class MetadataProvider
    {
        private readonly List<ModuleMetadata> modules = new List<ModuleMetadata>();
        private readonly Dictionary<Type, Dictionary<string, FieldInfo>> mappings = new Dictionary<Type, Dictionary<string, FieldInfo>>();

        public static MetadataProvider Current { get; set; }

        public MetadataProvider(params ModuleMetadata[] newModules)
        {
            modules.AddRange(newModules);
        }

        public IDictionary<string, FieldInfo> GetTypeMapping(Type type)
        {
            if (type == typeof(System.Object))
            {
                return new Dictionary<string, FieldInfo>();
            }
            Dictionary<string, FieldInfo> map;
            if (!mappings.TryGetValue(type, out map))
            {
                map = GenerateMapping(type);
                mappings.Add(type, map);
            }
            return map;
        }
        
        private TypeMetadata GetTypeMetadata(Type type)
        {
            var key = CRefToolkit.GetCRef(type);
            return modules.SelectMany(m => m.Types.Where(t => t.CRef == key)).FirstOrDefault();
        }

        private Dictionary<string, FieldInfo> GenerateMapping(Type type)
        {
            Dictionary<string, FieldInfo> map;
            var parent = GetTypeMapping(type.BaseType);
            if (parent != null)
            {
                map = new Dictionary<string,FieldInfo>(parent);
            }
            else
            {
                map = new Dictionary<string,FieldInfo>();
            }
            var scriptType = GetTypeMetadata(type);
            if (scriptType != null)
            {
                foreach (var field in type.GetFields(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic))
                {
                    var key = CRefToolkit.GetCRef(field);
                    var meta = scriptType.Fields.FirstOrDefault(f => f.CRef == key);
                    if (meta != null)
                    {
                        map.Add(meta.Name, field);
                    }
                }
            }
            else
            {
                var attribute = (AnonymousObjectAttribute)Attribute.GetCustomAttribute(type, typeof(AnonymousObjectAttribute));
                if (attribute != null)
                {
                    foreach (var field in type.GetFields(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.DeclaredOnly))
                    {
                        string name = CaseToolkit.GetMemberName(attribute.Convention, field.Name);
                        map.Add(name, field);
                    }
                }
                return null; // Non scriptable !
            }
            return map;
        }

        public string GetScriptTypeName(Type type)
        {
            if (type.IsArray)
            {
                return "array";
            }
            if (type == typeof(string))
            {
                return "string";
            }
            if (type == typeof(int) || type == typeof(double))
            {
                return "number";
            }
            var scriptType = GetTypeMetadata(type);
            if (scriptType != null)
            {
                return scriptType.Name;
            }
            return "object";
        }

        public bool IsPlainObjectType(Type type)
        {
            if (type.IsArray || type == typeof(string) || type == typeof(int) || type == typeof(double))
            {
                return false;
            }
            return true;
        }


    }
}
