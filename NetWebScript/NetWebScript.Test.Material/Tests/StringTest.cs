
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetWebScript;

namespace NetWebScript.Test.Material.Tests
{
    
    [TestClass, ScriptAvailable]
    public class StringTest
	{
        [TestMethod]
        public void String_Equality()
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
        public void String_Inequality()
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
        public void String_Concat()
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
        public void String_Substring()
		{
			String a = "abcdef";
			String b = a.Substring(2,2);
			Assert.AreEqual("cd",b);

            b = a.Substring(2);
            Assert.AreEqual("cdef", b);
		}

        [TestMethod]
        public void String_Chars()
		{
			String a = "abcdef";
			char b = a[2];
			Assert.AreEqual('c',b);
		}

        [TestMethod]
        public void String_Length()
		{
			String a = "abcdef";
			int b = a.Length;
			Assert.AreEqual(6,b);
		}

        [TestMethod]
        public void String_Trim()
        {
            Assert.AreEqual("abcd", "  abcd  ".Trim());
            Assert.AreEqual("abcd", "  abcd".Trim());
            Assert.AreEqual("abcd", "abcd  ".Trim());
            Assert.AreEqual("abcd", "abcd".Trim());
            Assert.AreEqual("", " ".Trim());
        }

        [TestMethod]
        public void String_TrimStart()
        {
            Assert.AreEqual("abcd  ", "  abcd  ".TrimStart());
            Assert.AreEqual("abcd", "  abcd".TrimStart());
            Assert.AreEqual("abcd  ", "abcd  ".TrimStart());
            Assert.AreEqual("abcd", "abcd".TrimStart());
            Assert.AreEqual("", " ".TrimStart());
        }

        [TestMethod]
        public void String_TrimEnd()
        {
            Assert.AreEqual("  abcd", "  abcd  ".TrimEnd());
            Assert.AreEqual("  abcd", "  abcd".TrimEnd());
            Assert.AreEqual("abcd", "abcd  ".TrimEnd());
            Assert.AreEqual("abcd", "abcd".TrimEnd());
            Assert.AreEqual("", " ".Trim());
        }

        [TestMethod]
        public void String_Format()
        {
            Assert.AreEqual("Hello world !", string.Format("Hello {0} !", "world"));
            Assert.AreEqual("Hello world !", string.Format("{1} {0} !", "world", "Hello"));
            Assert.AreEqual("Hello world !", string.Format("{1} {0} {2}", "world", "Hello", "!"));
            Assert.AreEqual("Hello { world } !", string.Format("Hello {{ {0} }} !", "world"));
            Assert.AreEqual("Hello } world { !", string.Format("Hello }} {0} {{ !", "world"));
        }

        [TestMethod]
        public void String_Format_Alignement()
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
        public void String_Concat_Objects()
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

        [TestMethod]
        public void String_Split_Chars()
        {
            var str = "a,b;c,d";

            var array = str.Split(',');
            Assert.AreEqual(3, array.Length);
            Assert.AreEqual("a", array[0]);
            Assert.AreEqual("b;c", array[1]);
            Assert.AreEqual("d", array[2]);

            array = str.Split('$');
            Assert.AreEqual(1, array.Length);
            Assert.AreEqual("a,b;c,d", array[0]);

            array = str.Split(',', ';');
            Assert.AreEqual(4, array.Length);
            Assert.AreEqual("a", array[0]);
            Assert.AreEqual("b", array[1]);
            Assert.AreEqual("c", array[2]);
            Assert.AreEqual("d", array[3]);

            array = str.Split('$', '@');
            Assert.AreEqual(1, array.Length);
            Assert.AreEqual("a,b;c,d", array[0]);
        }

        [TestMethod]
        public void String_Split_Chars_Count()
        {
            var str = "a,b;c,d";

            var array = str.Split(new []{','}, 2);
            Assert.AreEqual(2, array.Length);
            Assert.AreEqual("a", array[0]);
            Assert.AreEqual("b;c,d", array[1]);

            array = str.Split(new []{'$'}, 2);
            Assert.AreEqual(1, array.Length);
            Assert.AreEqual("a,b;c,d", array[0]);

            array = str.Split(new []{',', ';'},3);
            Assert.AreEqual(3, array.Length);
            Assert.AreEqual("a", array[0]);
            Assert.AreEqual("b", array[1]);
            Assert.AreEqual("c,d", array[2]);

            array = str.Split(new[] { '$', '@' }, 3);
            Assert.AreEqual(1, array.Length);
            Assert.AreEqual("a,b;c,d", array[0]);
        }

        [TestMethod]
        public void String_StartsWith()
        {
            Assert.IsTrue("Hello world !".StartsWith("Hello"));
            Assert.IsFalse("Hello world !".StartsWith("world !"));
        }

        [TestMethod]
        public void String_EndsWith()
        {
            Assert.IsTrue("Hello world !".EndsWith("world !"));
            Assert.IsFalse("Hello world !".EndsWith("Hello"));
        }

        [TestMethod]
        public void String_Equals_StringComparison()
        {
            var a = "aBc";
            var b = "AbC";
            var c = "c";
            var d = "aBc";

            Assert.IsTrue(string.Equals(a, d, StringComparison.CurrentCulture));
            Assert.IsFalse(string.Equals(a, b, StringComparison.CurrentCulture));
            Assert.IsFalse(string.Equals(null, a, StringComparison.CurrentCulture));
            Assert.IsFalse(string.Equals(a, null, StringComparison.CurrentCulture));
            Assert.IsTrue(string.Equals(null, null, StringComparison.CurrentCulture));
            Assert.IsTrue(string.Equals(a, a, StringComparison.CurrentCulture));

            Assert.IsTrue(string.Equals(a, d, StringComparison.InvariantCulture));
            Assert.IsFalse(string.Equals(a, b, StringComparison.InvariantCulture));

            Assert.IsTrue(string.Equals(a, d, StringComparison.Ordinal));
            Assert.IsFalse(string.Equals(a, b, StringComparison.Ordinal));

            Assert.IsTrue(string.Equals(a, d, StringComparison.CurrentCultureIgnoreCase));
            Assert.IsTrue(string.Equals(a, b, StringComparison.CurrentCultureIgnoreCase));
            Assert.IsFalse(string.Equals(a, c, StringComparison.CurrentCultureIgnoreCase));

            Assert.IsTrue(string.Equals(a, d, StringComparison.InvariantCultureIgnoreCase));
            Assert.IsTrue(string.Equals(a, b, StringComparison.InvariantCultureIgnoreCase));
            Assert.IsFalse(string.Equals(a, c, StringComparison.InvariantCultureIgnoreCase));

            Assert.IsTrue(string.Equals(a, d, StringComparison.OrdinalIgnoreCase));
            Assert.IsTrue(string.Equals(a, b, StringComparison.OrdinalIgnoreCase));
            Assert.IsFalse(string.Equals(a, c, StringComparison.OrdinalIgnoreCase));
        }

        [TestMethod]
        public void String_Compare_StringComparison()
        {
            var a = "aB";
            var b = "Ab";
            var c = "bC";
            var d = "Bc";
            var e = "bA";

            Assert.AreEqual(0, Math.Sign(string.Compare(a, a, StringComparison.Ordinal)));
            Assert.AreEqual(1, Math.Sign(string.Compare(a, b, StringComparison.Ordinal)));
            Assert.AreEqual(-1, Math.Sign(string.Compare(a, c, StringComparison.Ordinal)));
            Assert.AreEqual(1, Math.Sign(string.Compare(a, d, StringComparison.Ordinal)));
            Assert.AreEqual(-1, Math.Sign(string.Compare(b, a, StringComparison.Ordinal)));
            Assert.AreEqual(-1, Math.Sign(string.Compare(b, c, StringComparison.Ordinal)));
            Assert.AreEqual(-1, Math.Sign(string.Compare(b, d, StringComparison.Ordinal)));
            Assert.AreEqual(1, Math.Sign(string.Compare(c, a, StringComparison.Ordinal)));
            Assert.AreEqual(1, Math.Sign(string.Compare(c, b, StringComparison.Ordinal)));
            Assert.AreEqual(1, Math.Sign(string.Compare(c, d, StringComparison.Ordinal)));
            Assert.AreEqual(-1, Math.Sign(string.Compare(d, a, StringComparison.Ordinal)));
            Assert.AreEqual(1, Math.Sign(string.Compare(d, b, StringComparison.Ordinal)));
            Assert.AreEqual(-1, Math.Sign(string.Compare(d, c, StringComparison.Ordinal)));
            Assert.AreEqual(-1, Math.Sign(string.Compare(d, e, StringComparison.Ordinal)));

            Assert.AreEqual(0, Math.Sign(string.Compare(a, a, StringComparison.OrdinalIgnoreCase)));
            Assert.AreEqual(0, Math.Sign(string.Compare(a, b, StringComparison.OrdinalIgnoreCase)));
            Assert.AreEqual(-1, Math.Sign(string.Compare(a, c, StringComparison.OrdinalIgnoreCase)));
            Assert.AreEqual(-1, Math.Sign(string.Compare(a, d, StringComparison.OrdinalIgnoreCase)));
            Assert.AreEqual(0, Math.Sign(string.Compare(b, a, StringComparison.OrdinalIgnoreCase)));
            Assert.AreEqual(-1, Math.Sign(string.Compare(b, c, StringComparison.OrdinalIgnoreCase)));
            Assert.AreEqual(-1, Math.Sign(string.Compare(b, d, StringComparison.OrdinalIgnoreCase)));
            Assert.AreEqual(1, Math.Sign(string.Compare(c, a, StringComparison.OrdinalIgnoreCase)));
            Assert.AreEqual(1, Math.Sign(string.Compare(c, b, StringComparison.OrdinalIgnoreCase)));
            Assert.AreEqual(0, Math.Sign(string.Compare(c, d, StringComparison.OrdinalIgnoreCase)));
            Assert.AreEqual(1, Math.Sign(string.Compare(d, a, StringComparison.OrdinalIgnoreCase)));
            Assert.AreEqual(1, Math.Sign(string.Compare(d, b, StringComparison.OrdinalIgnoreCase)));
            Assert.AreEqual(0, Math.Sign(string.Compare(d, c, StringComparison.OrdinalIgnoreCase)));
            Assert.AreEqual(1, Math.Sign(string.Compare(d, e, StringComparison.OrdinalIgnoreCase)));


            Assert.AreEqual(0, Math.Sign(string.Compare(a, a, StringComparison.CurrentCultureIgnoreCase)));
            Assert.AreEqual(0, Math.Sign(string.Compare(a, b, StringComparison.CurrentCultureIgnoreCase)));
            Assert.AreEqual(-1, Math.Sign(string.Compare(a, c, StringComparison.CurrentCultureIgnoreCase)));
            Assert.AreEqual(-1, Math.Sign(string.Compare(a, d, StringComparison.CurrentCultureIgnoreCase)));
            Assert.AreEqual(0, Math.Sign(string.Compare(b, a, StringComparison.CurrentCultureIgnoreCase)));
            Assert.AreEqual(-1, Math.Sign(string.Compare(b, c, StringComparison.CurrentCultureIgnoreCase)));
            Assert.AreEqual(-1, Math.Sign(string.Compare(b, d, StringComparison.CurrentCultureIgnoreCase)));
            Assert.AreEqual(1, Math.Sign(string.Compare(c, a, StringComparison.CurrentCultureIgnoreCase)));
            Assert.AreEqual(1, Math.Sign(string.Compare(c, b, StringComparison.CurrentCultureIgnoreCase)));
            Assert.AreEqual(0, Math.Sign(string.Compare(c, d, StringComparison.CurrentCultureIgnoreCase)));
            Assert.AreEqual(1, Math.Sign(string.Compare(d, a, StringComparison.CurrentCultureIgnoreCase)));
            Assert.AreEqual(1, Math.Sign(string.Compare(d, b, StringComparison.CurrentCultureIgnoreCase)));
            Assert.AreEqual(0, Math.Sign(string.Compare(d, c, StringComparison.CurrentCultureIgnoreCase)));
            Assert.AreEqual(1, Math.Sign(string.Compare(d, e, StringComparison.CurrentCultureIgnoreCase)));


            Assert.AreEqual(0, Math.Sign(string.Compare(a, a, StringComparison.InvariantCultureIgnoreCase)));
            Assert.AreEqual(0, Math.Sign(string.Compare(a, b, StringComparison.InvariantCultureIgnoreCase)));
            Assert.AreEqual(-1, Math.Sign(string.Compare(a, c, StringComparison.InvariantCultureIgnoreCase)));
            Assert.AreEqual(-1, Math.Sign(string.Compare(a, d, StringComparison.InvariantCultureIgnoreCase)));
            Assert.AreEqual(0, Math.Sign(string.Compare(b, a, StringComparison.InvariantCultureIgnoreCase)));
            Assert.AreEqual(-1, Math.Sign(string.Compare(b, c, StringComparison.InvariantCultureIgnoreCase)));
            Assert.AreEqual(-1, Math.Sign(string.Compare(b, d, StringComparison.InvariantCultureIgnoreCase)));
            Assert.AreEqual(1, Math.Sign(string.Compare(c, a, StringComparison.InvariantCultureIgnoreCase)));
            Assert.AreEqual(1, Math.Sign(string.Compare(c, b, StringComparison.InvariantCultureIgnoreCase)));
            Assert.AreEqual(0, Math.Sign(string.Compare(c, d, StringComparison.InvariantCultureIgnoreCase)));
            Assert.AreEqual(1, Math.Sign(string.Compare(d, a, StringComparison.InvariantCultureIgnoreCase)));
            Assert.AreEqual(1, Math.Sign(string.Compare(d, b, StringComparison.InvariantCultureIgnoreCase)));
            Assert.AreEqual(0, Math.Sign(string.Compare(d, c, StringComparison.InvariantCultureIgnoreCase)));
            Assert.AreEqual(1, Math.Sign(string.Compare(d, e, StringComparison.InvariantCultureIgnoreCase)));

            // FIXME:
            // StringComparison.InvariantCulture and StringComparison.CurrentCulture does not behave the same way
            // in CLR and in browser
        }

        [TestMethod]
        public void String_IsNullOrEmpty()
        {
            Assert.IsTrue(string.IsNullOrEmpty(""));
            Assert.IsTrue(string.IsNullOrEmpty(null));
            Assert.IsFalse(string.IsNullOrEmpty("Hello world!"));
        }

        [TestMethod]
        public void String_IsNullOrWhiteSpace()
        {
            Assert.IsTrue(string.IsNullOrWhiteSpace(""));
            Assert.IsTrue(string.IsNullOrWhiteSpace(" "));
            Assert.IsTrue(string.IsNullOrWhiteSpace("\t"));
            Assert.IsTrue(string.IsNullOrWhiteSpace("\n"));
            Assert.IsTrue(string.IsNullOrWhiteSpace("\r"));
            Assert.IsTrue(string.IsNullOrWhiteSpace("\r\n\t "));
            Assert.IsTrue(string.IsNullOrWhiteSpace(null));
            Assert.IsFalse(string.IsNullOrWhiteSpace("Hello world!"));
        }
	}
}
