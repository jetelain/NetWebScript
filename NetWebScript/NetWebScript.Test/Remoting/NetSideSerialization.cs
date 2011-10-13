using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml;
using NetWebScript.Remoting.Serialization;

namespace NetWebScript.Test.Remoting
{
    [TestClass]
    public class NetSideSerialization
    {

        [TestMethod]
        public void NetDeserialize_Class()
        {
            var cache = new SerializerCache(ScriptSideSerialization.GetModule());
            var doc = new XmlDocument();
            doc.LoadXml(@"<root><obj c=""aTa""><str n=""aSa"">A field</str><num v=""12"" n=""aSb"" /><num v=""12.34"" n=""aSc"" /><null n=""aSd"" /><null n=""aSe"" /></obj></root>");
            var obj = new XmlDeserializer(cache).Deserialize<ClassA>((XmlElement)doc.DocumentElement.SelectSingleNode("*"));

            Assert.IsInstanceOfType(obj, typeof(ClassA));
            var objA = (ClassA)obj;
            Assert.AreEqual("A field", objA.StringField);
            Assert.AreEqual(12.34, objA.DoubleField);
            Assert.AreEqual(12, objA.IntField);
            Assert.AreEqual(null, objA.BField);
            Assert.AreEqual(null, objA.BArrayField);
        }
    }
}
