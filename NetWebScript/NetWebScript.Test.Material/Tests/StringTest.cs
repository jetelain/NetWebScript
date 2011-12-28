
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

        [TestMethod]
        public void Trim()
        {
            Assert.AreEqual("abcd", "  abcd  ".Trim());
            Assert.AreEqual("abcd", "  abcd".Trim());
            Assert.AreEqual("abcd", "abcd  ".Trim());
            Assert.AreEqual("abcd", "abcd".Trim());
            Assert.AreEqual("", " ".Trim());
        }

        [TestMethod]
        public void TrimStart()
        {
            Assert.AreEqual("abcd  ", "  abcd  ".TrimStart());
            Assert.AreEqual("abcd", "  abcd".TrimStart());
            Assert.AreEqual("abcd  ", "abcd  ".TrimStart());
            Assert.AreEqual("abcd", "abcd".TrimStart());
            Assert.AreEqual("", " ".TrimStart());
        }

        [TestMethod]
        public void TrimEnd()
        {
            Assert.AreEqual("  abcd", "  abcd  ".TrimEnd());
            Assert.AreEqual("  abcd", "  abcd".TrimEnd());
            Assert.AreEqual("abcd", "abcd  ".TrimEnd());
            Assert.AreEqual("abcd", "abcd".TrimEnd());
            Assert.AreEqual("", " ".Trim());
        }

        [TestMethod]
        public void Format()
        {
            Assert.AreEqual("Hello world !", string.Format("Hello {0} !", "world"));
            Assert.AreEqual("Hello world !", string.Format("{1} {0} !", "world", "Hello"));
            Assert.AreEqual("Hello world !", string.Format("{1} {0} {2}", "world", "Hello", "!"));
            Assert.AreEqual("Hello { world } !", string.Format("Hello {{ {0} }} !", "world"));
            Assert.AreEqual("Hello } world { !", string.Format("Hello }} {0} {{ !", "world"));
        }

        [TestMethod]
        public void Format_Alignement()
        {
            Assert.AreEqual("|  abcd|", string.Format("|{0,6}|", "abcd"));
            Assert.AreEqual("|abcdef|", string.Format("|{0,6}|", "abcdef"));
            Assert.AreEqual("|abcdefgh|", string.Format("|{0,6}|", "abcdefgh"));
            Assert.AreEqual("|abcd  |", string.Format("|{0,-6}|", "abcd"));
            Assert.AreEqual("|abcdef|", string.Format("|{0,-6}|", "abcdef"));
            Assert.AreEqual("|abcdefgh|", string.Format("|{0,-6}|", "abcdefgh"));
        }

        private class SomeObject
        {
            public override string ToString()
            {
                return "SomeObject";
            }
        }

        [TestMethod]
        public void Concat_Objects()
        {
            object a = 32;
            object b = "word";
            object c = new SomeObject();

            Assert.AreEqual("32", string.Concat(a));
            //Assert.AreEqual("word", string.Concat(b));
            Assert.AreEqual("SomeObject", string.Concat(c));
            Assert.AreEqual("", string.Concat(arg0:null));

            Assert.AreEqual("3232", string.Concat(a, a));
            Assert.AreEqual("32word", string.Concat(a, b));
            Assert.AreEqual("word32", string.Concat(b, a));
            Assert.AreEqual("SomeObjectword", string.Concat(c, b));
            Assert.AreEqual("word", string.Concat(null, b));
            Assert.AreEqual("", string.Concat(null, null));
        }
	}
}
