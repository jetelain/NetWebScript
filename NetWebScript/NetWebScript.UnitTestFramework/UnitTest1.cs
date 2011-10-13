//using System;
//using System.Text;
//using System.Collections.Generic;
//using System.Linq;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using NetWebScript.JsClr.AstBuilder.Cil;
//using NetWebScript.Test.Material;
//using NetWebScript.JsClr.AstBuilder.Flow;
//using NetWebScript;
//using NetWebScript.JsClr.Ast;
//using System.IO;
//using NetWebScript.JsClr.JsBuilder.Generated;
//using NetWebScript.JsClr.AstBuilder.AstFilter;
//using NetWebScript.JsClr.AstBuilder;
//using System.Net;
//using System.Reflection;
//using System.Xml;
//using NetWebScript.Test.Client;

//namespace NetWebScript.Test
//{
//    /// <summary>
//    /// Summary description for UnitTest1
//    /// </summary>
//    [TestClass]
//    public class UnitTest1
//    {
//        public UnitTest1()
//        {
//            //
//            // TODO: Add constructor logic here
//            //
//        }

//        private TestContext testContextInstance;

//        /// <summary>
//        ///Gets or sets the test context which provides
//        ///information about and functionality for the current test run.
//        ///</summary>
//        public TestContext TestContext
//        {
//            get
//            {
//                return testContextInstance;
//            }
//            set
//            {
//                testContextInstance = value;
//            }
//        }

//        #region Additional test attributes
//        //
//        // You can use the following additional attributes as you write your tests:
//        //
//        // Use ClassInitialize to run code before running the first test in the class
//        // [ClassInitialize()]
//        // public static void MyClassInitialize(TestContext testContext) { }
//        //
//        // Use ClassCleanup to run code after all tests in a class have run
//        // [ClassCleanup()]
//        // public static void MyClassCleanup() { }
//        //
//        // Use TestInitialize to run code before running each test 
//        // [TestInitialize()]
//        // public void MyTestInitialize() { }
//        //
//        // Use TestCleanup to run code after each test has run
//        // [TestCleanup()]
//        // public void MyTestCleanup() { }
//        //
//        #endregion

//        [TestMethod]
//        public void TestMethod1()
//        {
//            /*Display(new Func<bool,object,object>(Flow.If));
//            Display(new Func<bool, object, object>(Flow.IfRet));
//            Display(new Func<bool, object, object, object>(Flow.IfElse));
//            Display(new Func<bool, object, object, object>(Flow.IfElseRet));
//            Display(new Func<int, int, int>(Flow.While));
//            Display(new Func<int, int, int, int, int>(Flow.WhileBreakContinue));
//            Display(new Func<int, int, int, int>(Flow.WhileReturn));
//            Display(new Func<int, int, int>(Flow.Swicth));
//            Display(new Func<int, int, int>(Flow.DoWhile));
//            Display(new Action(new NetWebScript.Test.Material.Tests.Logic().Not));
//            Display(new Action(new NetWebScript.Test.Material.Tests.Logic().Or));
//            Display(new Action(new NetWebScript.Test.Material.Tests.Arrays().CreateAndLength));
//            //
//            //Display(new Action(new NetWebScript.Test.Material.Tests.Flow().TryCatch));
//            //Display(new Action(new NetWebScript.Test.Material.Tests.Flow().TryCatch2));
//            //Display(new Action(new NetWebScript.Test.Material.Tests.Flow().TryCatchFinally));
//            //Display(new Action(new NetWebScript.Test.Material.Tests.Delegates().AnonymousDelegate));
//            Display(new Action(new NetWebScript.Test.Material.Tests.Delegates().StaticDelegate));
//            Display(new Action(new NetWebScript.Test.Material.Tests.Delegates().InstanceDelegate));
//            //
//            //Display(new Func<String, int>(Flow.SwitchStrRet));*/
//            //Display(new Action(new NetWebScript.Test.Material.Tests.Collections.Generic.ListTest().Foreach));

//            Display(new Func<String, String, bool>(new Channel().Send));
//        }

//        private void Display(Delegate d)
//        {
//            ControlFlowGraph graph = ControlFlowGraph.Create(d);
//            Console.WriteLine(graph.ToString());
//            Console.WriteLine();
//            List<Sequence> sequences = new FlowTransform(graph).Transform();
//            foreach (Sequence seq in sequences)
//            {
//                Console.WriteLine(seq.ToString());
//            }
//            Console.WriteLine();

//            MethodAst ast = StatementBuilder.Transform(graph.MethodBody, sequences);
//            StringBuilder builder = new StringBuilder();
//            builder.AppendLine("{");
//            Statement.Append(builder, ast.Statements);
//            builder.Append("}");

//            Console.WriteLine(builder.ToString());
//            builder = new StringBuilder();

//            AstFilters.Filter(ast);
//            builder.AppendLine("{");
//            Statement.Append(builder, ast.Statements);
//            builder.Append("}");
//            Console.WriteLine(builder.ToString());

//            Console.WriteLine();
//            Console.WriteLine();
//        }



//        [TestMethod]
//        public void TestMethod2()
//        {
//            /*foreach (MethodInfo method in typeof(NetWebScript.Test.Material.Tests.StringTest).GetMethods(BindingFlags.Instance|BindingFlags.DeclaredOnly|BindingFlags.Public))
//            {
//                UnitTestRemoteRunner.RunTest("NWS."+method.DeclaringType.FullName + "." + method.Name);
//            }*/
//            UnitTestGenerator gen = new UnitTestGenerator();

//            using (TextWriter writer = File.CreateText(@"C:\Priv\Projects\test\NetWebScript\NetWebScript.Test\app.js"))
//            {
//                using (TextWriter metaWriter = File.CreateText(@"C:\Priv\Projects\test\NetWebScript\NetWebScript.Test\app.js.xml"))
//                {
//                    using (XmlWriter xml = XmlWriter.Create(metaWriter))
//                    {
//                        gen.WriteTests(writer, xml);
//                    }
//                }
//            }
//        }

//    }
//}
