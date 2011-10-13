using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetWebScript.Metadata;
using NetWebScript.Remoting.Serialization;

namespace NetWebScript.Test.Remoting
{
    [TestClass]
    public class ScriptSideSerialization
    {
        public static ModuleMetadata GetModule()
        {
            ModuleMetadata module = new ModuleMetadata();
            module.Name = "a";
            var a = new TypeMetadata() { Name = "aTa", BaseTypeName = "Object", CRef = CRefToolkit.GetCRef(typeof(ClassA)) };
            var fieldCref = "F:" + typeof(ClassA).FullName;
            a.Fields.Add(new FieldMetadata() { Name = "aSa", CRef = fieldCref + ".StringField" });
            a.Fields.Add(new FieldMetadata() { Name = "aSb", CRef = fieldCref + ".IntField" });
            a.Fields.Add(new FieldMetadata() { Name = "aSc", CRef = fieldCref + ".DoubleField" });
            a.Fields.Add(new FieldMetadata() { Name = "aSd", CRef = fieldCref + ".BField" });
            a.Fields.Add(new FieldMetadata() { Name = "aSe", CRef = fieldCref + ".BArrayField" });
            module.Types.Add(a);
            return module;
        }

        private void SetModule()
        {
            MetadataProvider.Current = new MetadataProvider(GetModule());
        }


        [TestMethod]
        public void ScriptSerialize_Class()
        {
            SetModule();

            var a = new ClassA() { DoubleField = 12.34, IntField = 12, StringField = "A field" };

            var doc = XmlSerializer.Serialize(a);

            Assert.AreEqual(@"<root><obj c=""aTa""><str n=""aSa"">A field</str><num v=""12"" n=""aSb"" /><num v=""12.34"" n=""aSc"" /><null n=""aSd"" /><null n=""aSe"" /></obj></root>", doc.Xml);
        }

    }
}
