using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.Metadata;
using System.Xml;

namespace NetWebScript.Debug.Server
{
    public sealed class JSData
    {
        internal JSData(JSThread thread, XmlElement element, string name, string displayName)
        {
            DisplayName = displayName;
            Value = element.GetAttribute("Value");
            var valueType = element.GetAttribute("Type");

            var type = thread.GetTypeById(valueType);
            if (type == null)
            {
                ValueTypeDisplayName = valueType;
            }
            else
            {
                ValueTypeDisplayName = CRefToolkit.GetDisplayName(type.CRef);
            }

            Children = CreateFromObjectChildren(thread, type, element);
        }

        internal JSData(JSThread thread, MethodBaseMetadata methodMetadata, XmlElement element)
        {
            DisplayName = "t";
            Value = element.GetAttribute("Value");

            Children = CreateFromMethodContext(thread, methodMetadata, element);
        }

        private static List<JSData> CreateFromMethodContext(JSThread thread, MethodBaseMetadata methodMetadata, XmlElement element)
        {
            var list = element.SelectNodes("P");
            if (list.Count > 0)
            {
                var children = new List<JSData>();
                foreach (XmlElement subelement in list)
                {
                    var name = subelement.GetAttribute("Name");
                    var displayName = GetDisplayName(methodMetadata, name);
                    children.Add(new JSData(thread, subelement, name, displayName));
                }
                return children;
            }
            return null;
        }

        private static List<JSData> CreateFromObjectChildren(JSThread thread, TypeMetadata type, XmlElement element)
        {
            var list = element.SelectNodes("P");
            if (list.Count > 0)
            {
                var children = new List<JSData>();
                foreach (XmlElement subelement in list)
                {
                    var name = subelement.GetAttribute("Name");
                    if (!name.StartsWith("$", StringComparison.Ordinal))
                    {
                        var displayName = GetDisplayName(thread, type, name);
                        children.Add(new JSData(thread, subelement, name, displayName));
                    }
                }
                return children;
            }
            return null;
        }


        private static string GetDisplayName(MethodBaseMetadata method, string name)
        {
            if (name == "$this")
            {
                return "this";
            }
            if (method != null)
            {
                var variable = method.Variables.FirstOrDefault(v => v.Name == name);
                if (variable != null)
                {
                    return variable.CName;
                }
            }
            return name;
        }

        private static string GetDisplayName(JSThread thread, TypeMetadata type, string name)
        {
            if (type != null)
            {
                var field = type.Fields.FirstOrDefault(f => f.Name == name);
                if (field != null)
                {
                    return CRefToolkit.GetDisplayName(field.CRef);
                }
                if (!string.IsNullOrEmpty(type.BaseTypeName))
                {
                    var baseType = thread.GetTypeById(type.BaseTypeName);
                    if (baseType != null)
                    {
                        return GetDisplayName(thread, baseType, name);
                    }
                }
            }
            return name;
        }

        //internal string ValueTypeName { get; private set; }

        //internal string Name { get; private set; }

        public string ValueTypeDisplayName { get; private set; }

        public string DisplayName { get; private set; }

        public string Value { get; private set; }

        public List<JSData> Children { get; private set; }

        public bool IsExpandable()
        {
            return Children != null && Children.Count > 0;
        }
    }
}
