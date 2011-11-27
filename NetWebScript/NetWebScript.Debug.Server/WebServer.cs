using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml;

namespace NetWebScript.Debug.Server
{
    public class WebServer : IDisposable, IJSServer
    {
        private readonly HttpListener listener;
        private readonly List<JSProgram> programs = new List<JSProgram>();
        private readonly Dictionary<String,JSThread> threads = new Dictionary<String,JSThread>();

        private readonly Thread thread;

        private readonly List<IJSServerCallback> callbacks = new List<IJSServerCallback>();

        public WebServer(string uriPrefix)
        {
            listener = new HttpListener();
            listener.Prefixes.Add(uriPrefix);
            thread = new Thread(Work);
        }

        public void Start()
        {
            thread.Start();
        }

        private void Work()
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


        private JSProgram GetProgram(Uri pageUri)
        {
            foreach (JSProgram candidate in programs)
            {
                if (candidate.IsPartOfProgram(pageUri))
                {
                    return candidate;
                }
            }
            return null;
        }

        private JSProgram GetOrCreateProgram(Uri pageUri, List<ModuleInfo> modules)
        {
            var program = GetProgram(pageUri);
            if (program == null)
            {
                program = new JSProgram(this, programs.Count + 1, pageUri, modules);
                programs.Add(program);
                lock (callbacks)
                {
                    foreach (var callback in callbacks)
                    {
                        callback.OnNewProgram(program);
                    }
                }
            }
            else if ( modules != null )
            {
                program.MergeModules(modules);
            }
            return program;
        }

        public IJSProgram GetOrCreateProgram(Uri pageUri)
        {
            return GetOrCreateProgram(pageUri, null);
        }


        private String Start(String postData)
        {
            XmlDocument document = new XmlDocument();
            document.LoadXml(postData);
            var pageUri = new Uri(document.DocumentElement.GetAttribute("Url"));
 
            var modules = new List<ModuleInfo>();
            foreach (XmlElement element in document.DocumentElement.SelectNodes("Module"))
            {
                string name = element.GetAttribute("Name");
                string version = element.GetAttribute("Version");
                string timestamp = element.GetAttribute("Timestamp");
                string relpath = element.GetAttribute("Filename");
                Uri moduleUri = new Uri(pageUri, relpath);
                modules.Add(new ModuleInfo() { Name = name, Version = version, Uri = moduleUri, Timestamp = timestamp });
            }

            var program = GetOrCreateProgram(pageUri, modules);
            var thread = program.CreateThread();
            threads.Add(thread.UId, thread);
            return thread.InitMessage();
        }

        private void ProcessRequest(HttpListenerRequest request, HttpListenerResponse response)
        {
            String postData = null;
            if (request.HttpMethod == "POST")
            {
                using (Stream stream = request.InputStream)
                {
                    using (StreamReader reader = new StreamReader(stream, request.ContentEncoding))
                    {
                        postData = reader.ReadToEnd();
                    }
                }
            }

            response.ContentType = "text/plain; charset=utf-8";

            String cmd = request.QueryString.Get("cmd");
            if (cmd == "start")
            {
                using (Stream output = response.OutputStream)
                {
                    using (StreamWriter writer = new StreamWriter(output, Encoding.UTF8))
                    {
                        lock (this)
                        {
                            writer.Write(Start(postData));
                        }
                    }
                }
                return;
            }

            String t = request.QueryString.Get("t");
            JSThread thread;
            if (!threads.TryGetValue(t, out thread))
            {
                using (Stream output = response.OutputStream)
                {
                    using (StreamWriter writer = new StreamWriter(output, Encoding.UTF8))
                    {
                        writer.Write("dead");
                    }
                }
                return;
            }

            String data = request.QueryString.Get("data");
            String mode = request.QueryString.Get("mode");
            Trace.TraceInformation("Query: t={0} cmd={1} data={2} mode={3}", t, cmd, data, mode);
            using (Stream output = response.OutputStream)
            {
                using (StreamWriter writer = new StreamWriter(output, Encoding.UTF8))
                {
                    String msg = thread.Query(cmd, data, postData, mode == "wait");
                    Trace.TraceInformation("Response: t={0} msg={1}", t, msg);
                    writer.Write(msg);
                }
            }
        }

        private void ProcessRequest(object listenerContext)
        {
            var context = (HttpListenerContext)listenerContext;
            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;
            try
            {
                ProcessRequest(request, response);
            }
            catch (Exception e)
            {
                response.StatusCode = 500;
                Trace.TraceError("Error in processing request : {0}", e.ToString());
            }
            finally
            {
                response.Close();
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

        public ICollection<IJSProgram> Programs
        {
            get 
            {
                List<IJSProgram> list = new List<IJSProgram>();
                foreach (JSProgram thread in programs)
                {
                    list.Add(thread);
                }
                return list;
            }
        }

        public void RegisterCallback(IJSServerCallback callback)
        {
            lock (callbacks)
            {
                callbacks.Add(callback);
            }
        }

        public void UnRegisterCallback(IJSServerCallback callback)
        {
            lock (callbacks)
            {
                callbacks.Remove(callback);
            }
        }

    }
}