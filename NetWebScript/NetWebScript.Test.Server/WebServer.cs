using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

namespace NetWebScript.Test.Server
{
    public class WebServer : IDisposable
    {
        private readonly HttpListener listener;
        private readonly Dictionary<String, TestRunner> programs = new Dictionary<String, TestRunner>();
        private readonly Thread thread;
        private readonly String uriPrefix;

        public WebServer(string uriPrefix)
        {
            this.uriPrefix = uriPrefix;
            listener = new HttpListener();
            listener.Prefixes.Add(uriPrefix);
            thread = new Thread(Work);
        }

        public event EventHandler ProgramAdded;

        public void Start()
        {
            thread.Start();
        }

        public void Work()
        {
            listener.Start();
            while (true)
            {
                try
                {
                    HttpListenerContext request = listener.GetContext();
                    ThreadPool.QueueUserWorkItem(ProcessRequest, request);
                }
                catch (HttpListenerException)
                {
                    return;
                }
            }
        }

        private void ServeFile ( string resource, HttpListenerResponse response)
        {
            //"NetWebScript.Test.Server"
            String content;
            using (Stream stream = typeof(WebServer).Assembly.GetManifestResourceStream("NetWebScript.Test.Server." + resource))
            {
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    content = reader.ReadToEnd();
                }
            }
            content = content.Replace("%%URL%%", uriPrefix);

            if (resource.EndsWith(".htm"))
            {
                response.ContentType = "text/html; charset=utf-8";
            }
            else if (resource.EndsWith(".js"))
            {
                response.ContentType = "text/javascript; charset=utf-8";
            }

            using (Stream output = response.OutputStream)
            {
                using (StreamWriter writer = new StreamWriter(output, Encoding.UTF8))
                {
                    writer.Write(content);
                }
            }
        }


        private void RunTest(HttpListenerRequest request, HttpListenerResponse response)
        {
            List<TestRunner> runners;
            lock (this)
            {
                runners = new List<TestRunner>(programs.Values);
            }
            String method = request.QueryString.Get("method");
            String script;
            using (Stream stream = request.InputStream)
            {
                using (StreamReader reader = new StreamReader(stream, request.ContentEncoding))
                {
                    script = reader.ReadToEnd();
                }
            }
            using (Stream output = response.OutputStream)
            {
                using (StreamWriter writer = new StreamWriter(output, request.ContentEncoding))
                {
                    foreach (TestRunner runner in runners)
                    {
                        writer.Write("{0}:",runner.Id.Replace(':',' '));
                        writer.WriteLine(runner.RunTest(method, script));
                    }
                    writer.Write("done");
                }
            }
        }

        private void TestRunnerDispatch(String cmd, HttpListenerRequest request, HttpListenerResponse response)
        {
            String p = request.QueryString.Get("p");
            TestRunner prog;
            if (!programs.TryGetValue(p, out prog))
            {
                lock (this)
                {
                    if (!programs.TryGetValue(p, out prog))
                    {
                        prog = new TestRunner(p);
                        programs.Add(p, prog);
                    }
                }
            }

            response.ContentType = "text/plain; charset=utf-8";

            String data = request.QueryString.Get("data");
            String mode = request.QueryString.Get("mode");

            Console.WriteLine("Query: p={0} cmd={1} data={2} mode={3}", p, cmd, data, mode);

            using (Stream output = response.OutputStream)
            {
                using (StreamWriter writer = new StreamWriter(output, Encoding.UTF8))
                {
                    String msg = prog.Query(cmd, data, mode == "wait");
                    //Console.WriteLine("Response: p={0} msg={1}", p, msg);
                    writer.Write(msg);
                }
            }
        }

        private void ProcessRequest(object listenerContext)
        {
            var context = (HttpListenerContext)listenerContext;
            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;

            String cmd = request.QueryString.Get("cmd");
            if (string.IsNullOrEmpty(cmd))
            {
                ServeFile("Test.Runner.htm", response);
            }
            else if (cmd == "script")
            {
                ServeFile("Test.Client.js", response);
            }
            else if (cmd == "run")
            {
                RunTest(request, response);
            }
            else
            {
                TestRunnerDispatch(cmd, request, response);
            }
        }

        public void Dispose()
        {
            if (thread.IsAlive)
            {
                listener.Close();
                thread.Join();
            }
        }

    }
}