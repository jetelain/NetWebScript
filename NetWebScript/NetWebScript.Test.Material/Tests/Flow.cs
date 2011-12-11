
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetWebScript;

namespace NetWebScript.Test.Material.Tests
{
    [TestClass, ScriptAvailable]
    public class Flow
	{
        [TestMethod]
        public void If()
		{
			int ok = 0;
			int ko = 0;
			bool isTrue = true;
			bool isFalse = false;
			
			if ( isTrue )
			{
				ok++;
			}
		
			if ( isFalse )
			{
				ko++;	
			}
			
			if ( isTrue )
			{
				ok++;
			}
			else
			{
				ko++;
			}
			
			if ( isFalse )
			{
				ko++;
			}
			else
			{
				ok++;
			}
			
			if ( isFalse )
			{
				ko++;
			}
			else if ( isTrue )
			{
				ok++;
			}
			else
			{
				ko++;	
			}
			
			Assert.AreEqual(0,ko);
			Assert.AreEqual(4,ok);
		}

        [TestMethod]
        public void For()
		{
			int ok = 0;
			for(int i = 0;i<10;++i)
			{
				ok++;
			}
			Assert.AreEqual(10,ok);
		}

        [TestMethod]
        public void While()
		{
			int ok = 0;
			while(ok<10)
			{
				ok++;
			}
			while(ok<10)
			{
				ok++;
			}
			Assert.AreEqual(10,ok);
		}

        [TestMethod]
        public void DoWhile2()
        {
            int ok = 1;
            do
            {
                ok++;
            } while (ok < 1);
            Assert.AreEqual(2, ok);
            do
            {
                ok++;
            } while (ok < 10);
            Assert.AreEqual(10, ok);
        }

        [TestMethod]
        public void DoWhile()
        {
            int a = 0, b = 0, c = 0;
            do
            {
                a++;
                if (a < 5)
                {
                    b++;
                }
                else
                {
                    b += 2;
                }
                c++;
            } while (a < 10);

            Assert.AreEqual(10, a);
            Assert.AreEqual(10, c);
            Assert.AreEqual(16, b);
        }


        [TestMethod]
        public void AlmostSwitch()
		{
			int ok =0;
			int ko =0;
			
			int a = 1;
			switch ( a )
			{
			case 0: ko++; break;
			case 1: ok++; break;
			default: ko++; break;
			}
			
			a = 0;
			switch ( a )
			{
			case 0: ok+=2; break;
			case 1: ko++; break;
			default: ko++; break;
			}
			
			a = 33;
			switch ( a )
			{
			case 0: ko++; break;
			case 1: ko++; break;
			default: ok+=4; break;
			}		
			
			Assert.AreEqual(7,ok);
			Assert.AreEqual(0,ko);
	
	
		}

        [TestMethod]
        public void Switch()
		{
			int ok =0;
			int ko =0;
			
			int a = 1;
			switch ( a )
			{
			case 0: ko++; break;
			case 1: ok++; break;
			case 2: ko++; break;
			case 3: ko++; break;
			default: ko++; break;
			}
			
			a = 0;
			switch ( a )
			{
			case 0: ok+=2; break;
			case 1: ko++; break;
			case 2: ko++; break;
			case 3: ko++; break;
			default: ko++; break;
			}
			
			a = 33;
			switch ( a )
			{
			case 0: ko++; break;
			case 1: ko++; break;
			case 2: ko++; break;
			case 3: ko++; break;	
			default: ok+=4; break;
			}		
			
			
			
			
			Assert.AreEqual(7,ok);
			Assert.AreEqual(0,ko);
	
	
		}

        [TestMethod]
        public void BaseCondition()
		{
			bool isTrue = true;
			if ( isTrue )
			{
				Assert.AreEqual(true,true);	
			}
			else
			{
				Assert.AreEqual(true,false);
			}
		}

        [TestMethod]
        public void Ternary()
		{
			int a = 10;
			int b = 20;
			int c = ( a > b ) ? a : b;
			Assert.AreEqual(c,20);
			
			int d = ( a > b ) ? ( c == b ? b : a ) : ( c == b ? a : b); 
			Assert.AreEqual(d,10);
		}


        [TestMethod]
        public void Pipeline()
        {
            int a = 10;
            int b;
            int c;

            a = b = c = a + 10;

            Assert.AreEqual(a, 20);
            Assert.AreEqual(b, 20);
            Assert.AreEqual(c, 20);
        }

        [TestMethod]
        public void TryCatch()
        {
            int ok = 0, ko = 0;
            try
            {
                ok++;
            }
            catch
            {
                ko++;
            }
            Assert.AreEqual(1, ok);
            Assert.AreEqual(0, ko);
        }

        [TestMethod]
        public void TryCatch2()
        {
            int ok = 0, ko = 0;
            try
            {
                ok++;
            }
            catch ( InvalidOperationException )
            {
                ko++;
            }
            catch
            {
                ko++;
            }
            Assert.AreEqual(1, ok);
            Assert.AreEqual(0, ko);
        }

        [TestMethod]
        public void TryCatch_CatchSpecific()
        {
            int ok = 0, ko = 0;
            try
            {
                ok++;
                throw new InvalidOperationException();
            }
            catch (InvalidOperationException)
            {
                ok++;
            }
            catch
            {
                ko++;
            }
            Assert.AreEqual(2, ok);
            Assert.AreEqual(0, ko);

            try
            {
                ok++;
                throw new NotImplementedException();
            }
            catch (InvalidOperationException)
            {
                ko++;
            }
            catch
            {
                ok++;
            }
            Assert.AreEqual(4, ok);
            Assert.AreEqual(0, ko);

            try
            {
                try
                {
                    ok++;
                    throw new NotImplementedException();
                }
                catch (InvalidOperationException)
                {
                    ko++;
                }
                ko++;
            }
            catch (NotImplementedException)
            {
                ok++;
            }
            Assert.AreEqual(6, ok);
            Assert.AreEqual(0, ko);
        }

        [TestMethod]
        public void TryCatchFinally()
        {
            int ok = 0, ko = 0;
            try
            {
                ok++;
            }
            catch
            {
                ko++;
            }
            finally
            {
                ok++;
            }
            Assert.AreEqual(2, ok);
            Assert.AreEqual(0, ko);
        }

        [TestMethod]
        public void TryNested()
        {
            int ok = 0, ko = 0;
            try
            {
                try
                {
                    try
                    {
                        ok++;
                    }
                    catch
                    {
                        ko+=3;
                    }
                }
                catch
                {
                    ko+=2;
                }
            }
            catch
            {
                ko++;
            }
            Assert.AreEqual(1, ok);
            Assert.AreEqual(0, ko);
        }

        //[TestMethod]
        //public void SwitchString()
        //{
        //    int ok = 0;
        //    int ko = 0;
        //    String a = "1";
        //    switch (a)
        //    {
        //        case "0": ko++; break;
        //        case "1": ok++; break;
        //        case "2": ko++; break;
        //        case "3": ko++; break;
        //        case "4": ko++; break;
        //        case "5": ko++; break;
        //        case "6": ko++; break;
        //        case "7": ko++; break;
        //        case "8": ko++; break;
        //        case "9": ko++; break;
        //        case "10": ko++; break;
        //        default: ko++; break;
        //    }
        //    a = "0";
        //    switch (a)
        //    {
        //        case "0": ok += 2; break;
        //        case "1": ko++; break;
        //        case "2": ko++; break;
        //        case "3": ko++; break;
        //        default: ko++; break;
        //    }
        //    a = "33";
        //    switch (a)
        //    {
        //        case "0": ko++; break;
        //        case "1": ko++; break;
        //        case "2": ko++; break;
        //        case "3": ko++; break;
        //        default: ok += 4; break;
        //    }
        //    Assert.AreEqual(7, ok);
        //    Assert.AreEqual(0, ko);
        //}


        [TestMethod]
        public void NullCoalesceExpression()
        {
            String a = "value";
            String b = null;
            b = a ?? "badvalue";
            Assert.AreEqual("value", b);
            a = null;
            b = a ?? "newvalue";
            Assert.AreEqual("newvalue", b);
        }
        

	}
}
