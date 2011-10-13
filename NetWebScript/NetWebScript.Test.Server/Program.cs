using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetWebScript.Test.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Open one or more browser on http://localhost:9091/");
            WebServer server = new WebServer("http://localhost:9091/");
            server.Work();
        }
    }
}
