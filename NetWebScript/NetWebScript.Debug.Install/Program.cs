using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetWebScript.Debug.Install
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                if (args[0] == "uninstall")
                {
                    Console.WriteLine("UnInstall");
                    Engine.Register.UnInstall();
                    Console.WriteLine("Done.");
                    return;
                }
            }
            Console.WriteLine("Install");
            Engine.Register.Install();
            Console.WriteLine("Done.");
        }
    }
}
