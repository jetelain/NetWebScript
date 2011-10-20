using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml;
using NetWebScript.Remoting.Serialization;
using NetWebScript.Metadata;
using System.IO;


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


        [TestMethod]
        public void NetSerialize_Equivalent()
        {
            var module = new ModuleMetadata();
            var ctorType = typeof(NetWebScript.Equivalents.Reflection.ConstructorInfo);
            module.Equivalents.Add(new EquivalentMetadata() {
                EquivalentCRef = CRefToolkit.GetCRef(ctorType),
                CRef = CRefToolkit.GetCRef(typeof(System.Reflection.ConstructorInfo))
            });
            var type = new TypeMetadata() { Name = "aTa", CRef = CRefToolkit.GetCRef(ctorType) };
            type.Fields.Add(new FieldMetadata() { Name = "aSa", CRef = CRefToolkit.GetCRef(ctorType.GetField("type", System.Reflection.BindingFlags.NonPublic|System.Reflection.BindingFlags.Instance)) });
            type.Fields.Add(new FieldMetadata() { Name = "aSb", CRef = CRefToolkit.GetCRef(ctorType.GetField("method", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)) });
            module.Types.Add(type);

            var listType = typeof(NetWebScript.Equivalents.Collections.Generic.List<object>);
            var ctor = listType.GetConstructor(Type.EmptyTypes);
            type = new TypeMetadata() { Name = "aTb", CRef = CRefToolkit.GetCRef(listType) };
            type.Methods.Add(new MethodBaseMetadata() { Name = "aSc", CRef = CRefToolkit.GetCRef(ctor) });
            module.Types.Add(type);

            var cache = new SerializerCache(module);

            var serializer = new EvaluableSerializer(cache);

            var writer = new StringWriter();

            serializer.Serialize(writer, ctor);

            Assert.AreEqual("", writer.ToString());

        }
    }
}
