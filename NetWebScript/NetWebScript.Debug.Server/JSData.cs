using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using NetWebScript.Metadata;

namespace NetWebScript.Debug.Server
{
    /// <summary>
    /// Data dumped from runtime
    /// </summary>
    public sealed class JSData
    {
        private readonly string path;

        internal JSData(JSThread thread, XmlElement element, string path, string displayName)
        {
            this.path = path;

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

            if (element.GetAttribute("Retreive") == "true")
            {
                ShouldRetreiveChildren = true;
            }
            else
            {
                Children = CreateFromObjectChildren(path, thread, type, element);
            }
        }

        internal JSData(int frameIndex, JSThread thread, MethodBaseMetadata methodMetadata, XmlElement element)
        {
            this.path = string.Format("$s[{0}].context", frameIndex);
            DisplayName = "Locals";
            Value = element.GetAttribute("Value");
            Children = CreateFromMethodContext(path, thread, methodMetadata, element);
        }

        private static string CombinePath(string path, string name)
        {
            if (name.StartsWith("[", StringComparison.Ordinal) && name.EndsWith("]", StringComparison.Ordinal))
            {
                return path + name;
            }
            return path + "." + name;
        }

        private static List<JSData> CreateFromMethodContext(string path, JSThread thread, MethodBaseMetadata methodMetadata, XmlElement element)
        {
            var list = element.SelectNodes("P");
            if (list.Count > 0)
            {
                var children = new List<JSData>();
                foreach (XmlElement subelement in list)
                {
                    var name = subelement.GetAttribute("Name");
                    var displayName = GetDisplayName(methodMetadata, name);
                    children.Add(new JSData(thread, subelement, CombinePath(path, name), displayName));
                }
                return children;
            }
            return null;
        }

        private static List<JSData> CreateFromObjectChildren(string path, JSThread thread, TypeMetadata type, XmlElement element)
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
                        children.Add(new JSData(thread, subelement, CombinePath(path, name), displayName));
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

        /// <summary>
        /// Evaluation Path
        /// </summary>
        public string Path { get { return path; } }

        /// <summary>
        /// Type of the represented object 
        /// </summary>
        public string ValueTypeDisplayName { get; private set; }

        /// <summary>
        /// Name of the represented object within its parent 
        /// </summary>
        public string DisplayName { get; private set; }

        /// <summary>
        /// Result of "ToString()" on represented object
        /// </summary>
        public string Value { get; private set; }

        /// <summary>
        /// Children properties (or variables and argments for the "method locals" object)
        /// </summary>
        public List<JSData> Children { get; private set; }

        /// <summary>
        /// Children properties have not been yet retreived from runtime
        /// </summary>
        public bool ShouldRetreiveChildren { get; private set; }

        /// <summary>
        /// Specify if data has children (might not be yet retreived from runtime)
        /// </summary>
        public bool IsExpandable
        {
            get { return ShouldRetreiveChildren || (Children != null && Children.Count > 0); }
        }

        /// <summary>
        /// Merge with retreived data from runtime.
        /// </summary>
        /// <param name="expandedData"></param>
        public void Merge(JSData expandedData)
        {
            if (expandedData != null)
            {
                Children = expandedData.Children;
                ShouldRetreiveChildren = false;
            }
        }

    }
}
