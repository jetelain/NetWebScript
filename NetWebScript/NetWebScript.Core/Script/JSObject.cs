using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Reflection;
using NetWebScript.Remoting.Serialization;

namespace NetWebScript.Script
{
    /// <summary>
    /// JavaScript unknown object (no C# corresponding found).
    /// </summary>
    [Imported(Name="Object")]
    public sealed class JSObject
    {
        [ScriptBody(Inline = "undefined")]
        public static readonly object Undefined = new object();

        internal readonly Dictionary<string, object> data = new Dictionary<string, object>();

        public JSObject()
        {
        }

        [ScriptBody(Inline = "obj instanceof fn")]
        public static bool IsInstanceOf(object obj, JSFunction fn)
        {
            throw new PlatformNotSupportedException();
        }

        [ScriptBody(Inline = "typeof obj")]
        public static string TypeOf(object obj)
        {
            if (MetadataProvider.Current.IsPlainObjectType(obj.GetType()))
            {
                return "object";
            }
            // FIXME: the behaviour is incomplete
            return "undefined";
        }

        [ScriptBody(Inline = "obj.constructor")]
        public static JSFunction GetConstructor(object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }
            Contract.EndContractBlock();
            return new JSFunction(obj.GetType());
        }

        /// <summary>
        /// Set a field on any script available object
        /// </summary>
        /// <param name="obj">Script available object</param>
        /// <param name="name">Name of the field</param>
        /// <param name="value">Value to set</param>
        [ScriptBody(Inline = "obj[name]=value")]
        public static void Set(object obj, string name, object value)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            Contract.EndContractBlock();
            JSObject scriptObject = obj as JSObject;
            if (scriptObject != null)
            {
                scriptObject[name] = value;
            }
            else
            {
                MetadataProvider.Current.GetTypeMapping(obj.GetType())[name].SetValue(obj, value);
            }
        }

        /// <summary>
        /// Get a field on any script available object
        /// </summary>
        /// <param name="obj">Script available object</param>
        /// <param name="name">Name of the field</param>
        /// <returns>value, or <see cref="Undefined"/>.</returns>
        [ScriptBody(Inline = "obj[name]")]
        public static object Get(object obj, string name)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            Contract.EndContractBlock();
            JSObject scriptObject = obj as JSObject;
            if (scriptObject != null)
            {
                return scriptObject[name];
            }
            else
            {
                FieldInfo field;
                if (MetadataProvider.Current.GetTypeMapping(obj.GetType()).TryGetValue(name, out field))
                {
                    return field.GetValue(obj);
                }
                return Undefined;
            }
        }

        /// <summary>
        /// Reads or set a field in the object
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [IntrinsicProperty]
        public object this[string name]
        {
            get
            {
                if (name == null)
                {
                    throw new ArgumentNullException("name");
                }
                Contract.EndContractBlock();
                object value;
                if ( data.TryGetValue(name, out value ))
                {
                    return value;
                }
                return Undefined;
            }
            set
            {
                if (name == null)
                {
                    throw new ArgumentNullException("name");
                }
                Contract.EndContractBlock();
                data[name] = value;
            }
        }

        [ScriptBody(Inline = "delete this[name]")]
        public void Delete(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            Contract.EndContractBlock();
            data.Remove(name);
        }
    }
}
