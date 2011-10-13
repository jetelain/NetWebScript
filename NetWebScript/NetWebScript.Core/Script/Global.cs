using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetWebScript.Script
{
    [Imported]
    public static class Global
    {
        [ScriptAlias("alert")]
        public static void Alert(string messgae)
        {
            throw new PlatformNotSupportedException();
        }

        [ScriptAlias("prompt")]
        public static string Prompt(string message)
        {
            throw new PlatformNotSupportedException();
        }

        [ScriptAlias("prompt")]
        public static string Prompt(string message, string defaultValue)
        {
            throw new PlatformNotSupportedException();
        }

        [ScriptAlias("setTimeout")]
        public static int SetTimeout(Action action, int miliseconds)
        {
            throw new PlatformNotSupportedException();
        }

        [ScriptAlias("clearTimeout")]
        public static void ClearTimeout(int timeoutId)
        {
            throw new PlatformNotSupportedException();
        }

        [ScriptAlias("setInterval")]
        public static int SetInterval(Action action, int miliseconds)
        {
            throw new PlatformNotSupportedException();
        }

        [ScriptAlias("clearTimeout")]
        public static void ClearInterval(int intervalId)
        {
            throw new PlatformNotSupportedException();
        }

        [ScriptAlias("eval")]
        public static object Eval(string jscode)
        {
            throw new PlatformNotSupportedException();
        }
    }
}
