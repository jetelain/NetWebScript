using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Runtime.Serialization;
using NetWebScript.Metadata;
using System.Globalization;
using NetWebScript.Script;

namespace NetWebScript.Remoting.Serialization
{
    public class XmlDeserializer
    {
        private readonly SerializerCache cache;

        public XmlDeserializer(SerializerCache cache)
        {
            this.cache = cache;
        }

        public T Deserialize<T>(XmlElement element)
        {
            var obj = Deserialize(element);
            if (!(obj is T))
            {
                obj = cache.Converter.Convert(obj, typeof(T));
            }
            return (T)obj;
        }

        public object Deserialize(XmlElement element)
        {
            if (element.Name == "null")
            {
                return null;
            }
            if (element.Name == "str")
            {
                return element.InnerText;
            }
            if (element.Name == "num")
            {
                return double.Parse(element.GetAttribute("v"), CultureInfo.InvariantCulture);
            }
            if (element.Name == "arr")
            {
                int size = int.Parse(element.GetAttribute("s"), CultureInfo.InvariantCulture);
                int pos = 0;
                object[] list = new object[size];
                foreach (XmlElement child in element.ChildNodes)
                {
                    list[pos] = Deserialize(child);
                    pos++;
                }
                return list;
            }
            if (element.Name == "obj")
            {
                string name = element.GetAttribute("c");
                TypeMetadata meta = cache.GetTypeMetadataByScriptName(name);
                IObjectSerializer serializer;
                Type type;
                if (meta != null)
                {
                    type = CRefToolkit.ResolveType(meta.CRef);
                    serializer = cache.GetSerializer(type);
                }
                else
                {
                    type = typeof(JSObject);
                    serializer = UnknownObjectSerializer.Instance;
                }
                SerializationInfo info = new SerializationInfo(type, cache.Converter);
                foreach (XmlElement child in element.ChildNodes)
                {
                    info.AddValue(child.GetAttribute("n"),Deserialize(child));
                }
                return serializer.CreateAndDeserialize(info);
            }
            throw new Exception(element.Name);
        }
    }
}
