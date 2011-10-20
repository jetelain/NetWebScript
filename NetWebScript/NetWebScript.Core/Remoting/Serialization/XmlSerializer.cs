using NetWebScript;
using NetWebScript.Script;
using NetWebScript.Script.Xml;

namespace NetWebScript.Remoting.Serialization
{
    [ScriptAvailable]
    public static class XmlSerializer
    {
        public static IXmlDocument Serialize(object obj)
        {
            var doc = XmlToolkit.CreateDocument("root");
            doc.DocumentElement.AppendChild(Serialize(obj, doc));
            return doc;
        }

        public static IXmlElement Serialize(object obj, IXmlDocument document)
        {
            if (obj == null)
            {
                return document.CreateElement("null");
            }
            IXmlElement element;
            var typename = Unsafe.GetScriptTypeName(obj);
            if (typename == "string")
            {
                element = document.CreateElement("str");
                element.AppendChild(document.CreateTextNode((string)obj));
            }
            else if (typename == "undefined")
            {
                element = document.CreateElement("null");
            }
            else if (typename == "number")
            {
                element = document.CreateElement("num");
                element.SetAttribute("v", Unsafe.NumberToString(obj));
            }
            else if (typename == "array")
            {
                var array = (object[])obj;
                element = document.CreateElement("arr");
                element.SetAttribute("s", Unsafe.NumberToString(array.Length));
                for (int i = 0; i < array.Length; ++i)
                {
                    element.AppendChild(Serialize(array[i], document));
                }
            }
            else
            {
                element = document.CreateElement("obj");
                element.SetAttribute("c", typename);
                var members = Unsafe.GetFields(obj);
                for (int i = 0; i < members.Length; ++i)
                {
                    var member = members[i];
                    var value = JSObject.Get(obj, member);
                    var node = Serialize(value, document);
                    node.SetAttribute("n", member);
                    element.AppendChild(node);
                }
            }
            return element;
        }

        public static object CopyTo(object target, object source)
        {
            var members = Unsafe.GetAll(source);
            for (int i = 0; i < members.Length; ++i)
            {
                var member = members[i];
                JSObject.Set(target, member, JSObject.Get(source, member));
            }
            return target;
        }
    }
}
