
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetWebScript;

namespace NetWebScript.Test.Material.Tests
{
    
    [TestClass, ScriptAvailable]
    public class StringTest
	{
        [TestMethod]
        public void Equality()
		{
			String a = "some value";
			String b = "some value";
			bool c = a == b;
			Assert.AreEqual(true,c);
			
			b = "other value";
			c = a == b;
			Assert.AreEqual(false,c);
		}

        [TestMethod]
        public void Inequality()
		{
			String a = "some value";
			String b = "some value";
			bool c = a != b;
			Assert.AreEqual(false,c);
			
			b = "other value";
			c = a != b;
			Assert.AreEqual(true,c);
		}

        [TestMethod]
        public void Concat()
		{
			String a = "a";
			String b = "b";
			String c = "c";
			String d = "d";
			String e = "e";
			String f = "f";
			String g = "g";
			String v = a + b;
			Assert.AreEqual("ab",v);
			
			v = a + b + c;
			Assert.AreEqual("abc",v);
			
			v = a + b + c + d;
			Assert.AreEqual("abcd",v);
			
			v = a + b + c + d + e;
			Assert.AreEqual("abcde",v);
			
			v = a + b + c + d + e + f;
			Assert.AreEqual("abcdef",v);
			
			v = a + b + c + d + e + f +g;
			Assert.AreEqual("abcdefg",v);
			
			String[] ee = new String[4]{"a","b","c","d"};
			v = String.Concat(ee);
			Assert.AreEqual("abcd",v);
		}

        [TestMethod]
        public void Substring()
		{
			String a = "abcdef";
			String b = a.Substring(2,2);
			Assert.AreEqual("cd",b);
		}

        [TestMethod]
        public void Chars()
		{
			String a = "abcdef";
			char b = a[2];
			Assert.AreEqual('c',b);
		}

        [TestMethod]
        public void Length()
		{
			String a = "abcdef";
			int b = a.Length;
			Assert.AreEqual(6,b);
		}
	}
}
