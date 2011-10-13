using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Debugger.Interop;
using System.Xml;
using NetWebScript.Metadata;

namespace NetWebScript.Debug.Engine.Debug
{
    class Property : IDebugProperty2
    {
        private readonly Frame frame;
        private readonly TypeMetadata type;

        internal Property(Frame frame, XmlElement element, string name, string displayName)
        {
            this.frame = frame;

            Name = name;
            DisplayName = displayName;
            
            Value = element.GetAttribute("Value");
            TypeName = element.GetAttribute("Type");

            type = frame.Thread.ResolveTypeMetadata(TypeName);
            CreateChildren(element);
        }

        internal Property ( Frame frame, XmlElement element )
        {
            this.frame = frame;
            DisplayName = Name = "t";
            Value = element.GetAttribute("Value");
            TypeName = element.GetAttribute("Type");
            CreateChildren(element, frame.MethodMetadata);
        }

        private void CreateChildren(XmlElement element, MethodBaseMetadata methodMetadata)
        {
            var list = element.SelectNodes("P");
            if (list.Count > 0)
            {
                Children = new List<Property>();
                foreach (XmlElement subelement in list)
                {
                    var name = subelement.GetAttribute("Name");
                    var displayName = GetDisplayName(methodMetadata, name);
                    Children.Add(new Property(frame, subelement, name, displayName));
                }
            }
        }

        private void CreateChildren(XmlElement element)
        {
            var list = element.SelectNodes("P");
            if (list.Count > 0)
            {
                Children = new List<Property>();
                foreach (XmlElement subelement in list)
                {
                    var name = subelement.GetAttribute("Name");
                    if (!name.StartsWith("$", StringComparison.Ordinal))
                    {
                        var displayName = GetDisplayName(type, name);
                        Children.Add(new Property(frame, subelement, name, displayName));
                    }
                }
            }
        }


        private string GetDisplayName(MethodBaseMetadata parentType, string name)
        {
            if (name == "$this")
            {
                return "this";
            }
            var variable = parentType.Variables.FirstOrDefault(v => v.Name == name);
            if (variable != null)
            {
                return variable.CName;
            }
            return name;
        }

        private string GetDisplayName(TypeMetadata type, string name)
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
                    var baseType = frame.Thread.ResolveTypeMetadata(type.BaseTypeName);
                    if (baseType != null)
                    {
                        return GetDisplayName(baseType, name);
                    }
                }
            }
            return name;
        }

        internal String TypeName { get; private set; }

        internal String TypeDisplayName 
        { 
            get 
            {
                if (type != null)
                {
                    return type.CRef;
                }
                return TypeName;
            } 
        }

        internal String DisplayName { get; private set; }

        internal String Name { get; private set; }

        internal String Value { get; private set; }

        internal List<Property> Children { get; private set; }

        internal bool IsExpandable()
        {
            return Children != null && Children.Count > 0;
        }

        internal DEBUG_PROPERTY_INFO GetDebugPropertyInfo(enum_DEBUGPROP_INFO_FLAGS dwFields)
        {
            DEBUG_PROPERTY_INFO propertyInfo = new DEBUG_PROPERTY_INFO();

            if ((dwFields & enum_DEBUGPROP_INFO_FLAGS.DEBUGPROP_INFO_FULLNAME) != 0)
            {
                propertyInfo.bstrFullName = DisplayName;
                propertyInfo.dwFields |= enum_DEBUGPROP_INFO_FLAGS.DEBUGPROP_INFO_FULLNAME;
            }

            if ((dwFields & enum_DEBUGPROP_INFO_FLAGS.DEBUGPROP_INFO_NAME) != 0)
            {
                propertyInfo.bstrName = DisplayName;
                propertyInfo.dwFields |= enum_DEBUGPROP_INFO_FLAGS.DEBUGPROP_INFO_NAME;
            }

            if ((dwFields & enum_DEBUGPROP_INFO_FLAGS.DEBUGPROP_INFO_TYPE) != 0)
            {
                propertyInfo.bstrType = TypeDisplayName;
                propertyInfo.dwFields |= enum_DEBUGPROP_INFO_FLAGS.DEBUGPROP_INFO_TYPE;
            }

            if ((dwFields & enum_DEBUGPROP_INFO_FLAGS.DEBUGPROP_INFO_VALUE) != 0)
            {
                propertyInfo.bstrValue = Value;
                propertyInfo.dwFields |= enum_DEBUGPROP_INFO_FLAGS.DEBUGPROP_INFO_VALUE;
            }

            if ((dwFields & enum_DEBUGPROP_INFO_FLAGS.DEBUGPROP_INFO_ATTRIB) != 0)
            {
                propertyInfo.dwAttrib = enum_DBG_ATTRIB_FLAGS.DBG_ATTRIB_VALUE_READONLY;

                if (IsExpandable())
                {
                    propertyInfo.dwAttrib |= enum_DBG_ATTRIB_FLAGS.DBG_ATTRIB_OBJ_IS_EXPANDABLE;
                }
                propertyInfo.dwFields |= enum_DEBUGPROP_INFO_FLAGS.DEBUGPROP_INFO_ATTRIB;
            
            }

            // If the debugger has asked for the property, or the property has children (meaning it is a pointer in the sample)
            // then set the pProperty field so the debugger can call back when the chilren are enumerated.
            if (((dwFields & enum_DEBUGPROP_INFO_FLAGS.DEBUGPROP_INFO_PROP) != 0) ||
                IsExpandable())
            {
                propertyInfo.pProperty = (IDebugProperty2)this;
                propertyInfo.dwFields |= enum_DEBUGPROP_INFO_FLAGS.DEBUGPROP_INFO_PROP;
            }

            return propertyInfo;
        }

        #region IDebugProperty2 Members

        public int EnumChildren(enum_DEBUGPROP_INFO_FLAGS dwFields, uint dwRadix, ref Guid guidFilter, enum_DBG_ATTRIB_FLAGS dwAttribFilter, string pszNameFilter, uint dwTimeout, out IEnumDebugPropertyInfo2 ppEnum)
        {
            if (IsExpandable())
            {
                DEBUG_PROPERTY_INFO[] properties = new DEBUG_PROPERTY_INFO[Children.Count];
                for (int i = 0; i < Children.Count; ++i)
                {
                    properties[i] = Children[i].GetDebugPropertyInfo(dwFields);
                }
                ppEnum = new PropertyInfos(properties);
                return Constants.S_OK;
            }

            ppEnum = null;
            return Constants.S_FALSE;
        }

        public int GetDerivedMostProperty(out IDebugProperty2 ppDerivedMost)
        {
            throw new NotImplementedException();
        }

        public int GetExtendedInfo(ref Guid guidExtendedInfo, out object pExtendedInfo)
        {
            throw new NotImplementedException();
        }

        public int GetMemoryBytes(out IDebugMemoryBytes2 ppMemoryBytes)
        {
            throw new NotImplementedException();
        }

        public int GetMemoryContext(out IDebugMemoryContext2 ppMemory)
        {
            throw new NotImplementedException();
        }

        public int GetParent(out IDebugProperty2 ppParent)
        {
            throw new NotImplementedException();
        }

        public int GetPropertyInfo(enum_DEBUGPROP_INFO_FLAGS dwFields, uint dwRadix, uint dwTimeout, IDebugReference2[] rgpArgs, uint dwArgCount, DEBUG_PROPERTY_INFO[] pPropertyInfo)
        {
            rgpArgs = null;
            pPropertyInfo[0] = GetDebugPropertyInfo(dwFields);
            return Constants.S_OK;
        }

        public int GetReference(out IDebugReference2 ppReference)
        {
            throw new NotImplementedException();
        }

        public int GetSize(out uint pdwSize)
        {
            throw new NotImplementedException();
        }

        public int SetValueAsReference(IDebugReference2[] rgpArgs, uint dwArgCount, IDebugReference2 pValue, uint dwTimeout)
        {
            throw new NotImplementedException();
        }

        public int SetValueAsString(string pszValue, uint dwRadix, uint dwTimeout)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
