//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Net;
//using System.IO;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System.Diagnostics;

//namespace NetWebScript.Test
//{
//    public static class UnitTestRemoteRunner
//    {
//        private static String script;

//        public static void RunTest(String method)
//        {
//            if (script == null)
//            {
//                Trace.TraceInformation("Generate script");
//                UnitTestGenerator gen = new UnitTestGenerator();
//                StringWriter writer = new StringWriter();

//                using (StreamReader reader = File.OpenText(@"C:\Priv\Projects\test\NetWebScript\NetWebScript\JsClr\Runtime\Core.js"))
//                {
//                    writer.WriteLine(reader.ReadToEnd());
//                }

//                using (StreamReader reader = File.OpenText(@"C:\Priv\Projects\test\NetWebScript\NetWebScript.Test\UnitTest.js"))
//                {
//                    writer.WriteLine(reader.ReadToEnd());
//                }

//                gen.WriteTests(writer, null);
//                script = writer.ToString();
//                Trace.TraceInformation("Generated");
                
//            }

//            Trace.TraceInformation(method);

//            WebClient client = new WebClient();
//            String response = client.UploadString("http://localhost:9091/?cmd=run&method=" + method, script);
//            StringWriter report = new StringWriter();
//            bool failed = false;
//            String[] lines = response.Split('\n');
//            for (int i = 0; i < lines.Length - 1; ++i)
//            {
//                String info = lines[i].Trim();
//                int a = info.IndexOf(':');
//                int b = info.IndexOf(':', a + 1);
//                int c = info.IndexOf(':', b + 1);
//                string browser = info.Substring(0, a);
//                string result = info.Substring(a + 1, b - a - 1);
//                string time = info.Substring(b + 1, c - b - 1);
//                string message = info.Substring(c + 1);
//                Trace.TraceInformation(info);
//                if (result != "OK")
//                {
//                    failed = true;
//                    report.WriteLine("{0} : {1}", browser, message);
//                }
//            }
//            if (lines.Length == 1)
//            {
//                Assert.Fail("No browser avaliable");
//            }
//            if (failed)
//            {
//                Assert.Fail(report.ToString());
//            }
//        }



//    }
//}
