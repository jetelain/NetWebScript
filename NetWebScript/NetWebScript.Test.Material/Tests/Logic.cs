
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetWebScript;

namespace NetWebScript.Test.Material.Tests
{
    [TestClass, ScriptAvailable]
	public class Logic
	{

        [TestMethod]
        public void And()
		{
			bool a = true;
			bool b = true;
			bool c = a && b;
			Assert.AreEqual(true,c);
			
			a = false;
			b = true;
			c = a && b;
			Assert.AreEqual(false,c);
			
			a = true;
			b = false;
			c = a && b;
			Assert.AreEqual(false,c);			
			
			a = false;
			b = false;
			c = a && b;
			Assert.AreEqual(false,c);	
		}

        [TestMethod]
        public void Or()
		{
			bool a = true;
			bool b = true;
			bool c = a || b;
			Assert.AreEqual(true,c);
			
			a = false;
			b = true;
			c = a || b;
			Assert.AreEqual(true,c);
			
			a = true;
			b = false;
			c = a || b;
			Assert.AreEqual(true,c);			
			
			a = false;
			b = false;
			c = a || b;
			Assert.AreEqual(false,c);	
		}

        [TestMethod]
        public void Not()
		{
			bool a = true;
			bool b = !a;
			Assert.AreEqual(false,b);
			
			a = false;
			b = !a;
			Assert.AreEqual(true,b);
		}

	}
}
