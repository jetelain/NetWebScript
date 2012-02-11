using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetWebScript.Remoting;
using NetWebScript.Metadata;
using NetWebScript.Remoting.Serialization;

namespace NetWebScript.Test.Material.Tests.Remoting
{
    [TestClass]
    [ScriptAvailable]
    public class ProxiesTest
    {
        class Proxy : ScriptRealProxy
        {
            public override object Invoke(string typeId, string methodId, object[] arguments)
            {
                return "Hello " + arguments[0] + " !";
            }
        }

        class SampleClass : MarshalByRefObject 
        {
            public string Hello(string a)
            {
                return "Not throw proxy";
            }
        }


        interface ISampleInterface
        {
            string Hello(string a);
        }

        [ScriptBody(Body = "function(){}")] // Method has no meaning if run at script type
        private void CreateSampleClassMetadata()
        {
            var metadata = new ModuleMetadata();
            var classMetadata = new TypeMetadata();
            classMetadata.CRef = CRefToolkit.GetCRef(typeof(SampleClass));
            metadata.Types.Add(classMetadata);
            var methodMetadata = new MethodBaseMetadata();
            methodMetadata.CRef = CRefToolkit.GetCRef(typeof(SampleClass).GetMethod("Hello"));
            methodMetadata.Name = "ImplId";
            classMetadata.Methods.Add(methodMetadata);
            MetadataProvider.Current = new MetadataProvider(metadata);
        }

        [ScriptBody(Body = "function(){}")] // Method has no meaning if run at script type
        private void CreateSampleInterfaceMetadata()
        {
            var metadata = new ModuleMetadata();
            var classMetadata = new TypeMetadata();
            classMetadata.CRef = CRefToolkit.GetCRef(typeof(ISampleInterface));
            metadata.Types.Add(classMetadata);
            var methodMetadata = new MethodBaseMetadata();
            methodMetadata.CRef = CRefToolkit.GetCRef(typeof(ISampleInterface).GetMethod("Hello"));
            methodMetadata.Name = "ImplId";
            classMetadata.Methods.Add(methodMetadata);
            MetadataProvider.Current = new MetadataProvider(metadata);
        }


        [TestMethod]
        public void TransparentProxy_CreateClass()
        {
            CreateSampleClassMetadata();

            var proxy = new Proxy();
            var tranparent = ScriptTransparentProxy.Create<SampleClass>(proxy);

            var result = tranparent.Hello("world");
            Assert.AreEqual("Hello world !", result);

            object box = tranparent;
            Assert.AreEqual(true, box is SampleClass);
            Assert.AreSame(tranparent, box as SampleClass);
        }

        [TestMethod]
        public void TransparentProxy_CreateInterface()
        {
            CreateSampleInterfaceMetadata();

            var proxy = new Proxy();
            var tranparent = ScriptTransparentProxy.Create<ISampleInterface>(proxy);

            var result = tranparent.Hello("world");
            Assert.AreEqual("Hello world !", result);

            object box = tranparent;
            Assert.AreEqual(true, box is ISampleInterface);
            Assert.AreSame(tranparent, box as ISampleInterface);
        }


    }
}
