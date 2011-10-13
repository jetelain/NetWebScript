//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.IO;
//using NetWebScript.JsClr.JsBuilder.Generated;
//using NetWebScript.Test.Material;
//using System.Reflection;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System.Xml;

//namespace NetWebScript.Test
//{
//    public class UnitTestGenerator
//    {
//        GeneratedRegistry registry = new GeneratedRegistry();
		

//        public void WriteTests(TextWriter writer, XmlWriter metaWriter)
//        {
//            Assembly assembly = typeof(Flow).Assembly;
//            Generator gen = new Generator(writer, metaWriter);

//            //gen.WriteAssembly(typeof(NetWebScript.Test.Client.Channel).Assembly);
//            gen.WriteAssembly(assembly);
            
//            foreach (Type type in assembly.GetTypes())
//            {
//                if (Is<TestClassAttribute>(type) && registry.IsScriptAvailable(type))
//                {
//                    WriteTestClassMethods(writer, type);
//                }
//            }
            

//        }

//        private static bool Is<T>(MemberInfo member)
//        {
//            return member.GetCustomAttributes(typeof(T), true).Length > 0;
//        }

//        private void WriteTestClassMethods(TextWriter writer, Type type )
//        {
//            writer.WriteLine("NWS.UnitTest.Add({");
//            writer.Write("FullName: '");
//            writer.Write(type.FullName);
//            writer.WriteLine("',");
//            writer.Write("Ref: ");
//            writer.Write(registry.ClassName(type));
//            writer.WriteLine(",");
//            writer.Write("Methods:[");

//            bool first = true;

//            foreach (MethodInfo method in type.GetMethods())
//            {
//                if (Is<TestMethodAttribute>(method) && registry.IsScriptAvailable(method))
//                {
//                    if (first)
//                    {
//                        first = false;
//                    }
//                    else
//                    {
//                        writer.Write(',');
//                    }
//                    writer.Write("'");
//                    writer.Write(method.Name);
//                    writer.Write("'");
//                }
//            }
//            writer.WriteLine("]");
//            writer.WriteLine("});");
//        }



//    }
//}
